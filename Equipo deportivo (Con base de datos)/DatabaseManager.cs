
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RegistroEquiposDeportivos
{
    public class ColumnInfo
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public bool HasDefault { get; set; }
    }

    public class DatabaseManager
    {
        private readonly string connectionString =
            @"Data Source=(localdb)\Itla;
              Initial Catalog=Registro de Equipos Deportivos;
              Integrated Security=True;
              TrustServerCertificate=True;";

        private readonly string[] allowedTables =
        {
            "Deportes",
            "Categorias",
            "Equipos",
            "Posiciones",
            "Jugadores",
            "EquipoJugador",
            "Entrenadores",
            "EquipoEntrenador",
            "Temporadas",
            "Partidos"
        };

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void ValidateTable(string tableName)
        {
            if (!allowedTables.Contains(tableName))
            {
                throw new Exception("La tabla seleccionada no es valida.");
            }
        }

        public List<string> GetTables()
        {
            return new List<string>(allowedTables);
        }

        public List<ColumnInfo> GetColumns(string tableName)
        {
            ValidateTable(tableName);

            List<ColumnInfo> columns = new List<ColumnInfo>();

            string query = @"
                SELECT
                    c.name AS ColumnName,
                    t.name AS DataType,
                    c.is_identity AS IsIdentity,
                    CASE WHEN pk.column_id IS NULL
                         THEN 0 ELSE 1 END AS IsPrimaryKey,
                    c.is_nullable AS IsNullable,
                    CASE WHEN c.default_object_id <> 0
                         OR dc.object_id IS NOT NULL
                         THEN 1 ELSE 0 END AS HasDefault
                FROM sys.columns c
                INNER JOIN sys.tables tb
                    ON c.object_id = tb.object_id
                INNER JOIN sys.types t
                    ON c.user_type_id = t.user_type_id
                LEFT JOIN sys.default_constraints dc
                    ON c.default_object_id = dc.object_id
                LEFT JOIN
                (
                    SELECT ic.object_id, ic.column_id
                    FROM sys.indexes i
                    INNER JOIN sys.index_columns ic
                        ON i.object_id = ic.object_id
                        AND i.index_id = ic.index_id
                    WHERE i.is_primary_key = 1
                ) pk
                    ON c.object_id = pk.object_id
                    AND c.column_id = pk.column_id
                WHERE tb.name = @tableName
                ORDER BY c.column_id;";

            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@tableName", SqlDbType.NVarChar)
                    .Value = tableName;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(new ColumnInfo
                        {
                            Name = reader["ColumnName"].ToString(),
                            DataType = reader["DataType"].ToString(),
                            IsIdentity = Convert.ToBoolean(reader["IsIdentity"]),
                            IsPrimaryKey = Convert.ToBoolean(reader["IsPrimaryKey"]),
                            IsNullable = Convert.ToBoolean(reader["IsNullable"]),
                            HasDefault = Convert.ToBoolean(reader["HasDefault"])
                        });
                    }
                }
            }

            return columns;
        }

        public DataTable ListRecords(string tableName)
        {
            ValidateTable(tableName);

            DataTable data = new DataTable();

            string query = "SELECT * FROM [" + tableName + "]";

            using (SqlConnection connection = GetConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                adapter.Fill(data);
            }

            return data;
        }

        public DataTable SearchRecords(string tableName, string columnName, string value)
        {
            ValidateTable(tableName);

            List<ColumnInfo> columns = GetColumns(tableName);

            if (!columns.Any(c => c.Name == columnName))
            {
                throw new Exception("La columna seleccionada no existe.");
            }

            string query =
                "SELECT * FROM [" + tableName + "] " +
                "WHERE CONVERT(NVARCHAR(4000), [" + columnName +
                "], 121) LIKE @value";

            DataTable data = new DataTable();

            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@value", SqlDbType.NVarChar)
                    .Value = "%" + value + "%";

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(data);
                }
            }

            return data;
        }

        public int AddRecord(string tableName, Dictionary<string, string> values)
        {
            ValidateTable(tableName);

            List<ColumnInfo> columns = GetColumns(tableName);

            List<string> insertColumns = new List<string>();
            List<string> parameters = new List<string>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    int index = 0;

                    foreach (ColumnInfo column in columns)
                    {
                        if (column.IsIdentity)
                            continue;

                        if (!values.ContainsKey(column.Name))
                            continue;

                        string value = values[column.Name];

                        if (string.IsNullOrWhiteSpace(value))
                            continue;

                        string parameterName = "@p" + index;

                        insertColumns.Add("[" + column.Name + "]");
                        parameters.Add(parameterName);

                        object parameterValue =
                            value.Trim().Equals("NULL",
                            StringComparison.OrdinalIgnoreCase)
                            ? DBNull.Value
                            : (object)value;

                        command.Parameters.AddWithValue(
                            parameterName, parameterValue);

                        index++;
                    }

                    string query;

                    if (insertColumns.Count == 0)
                    {
                        query = "INSERT INTO [" + tableName +
                                "] DEFAULT VALUES";
                    }
                    else
                    {
                        query = "INSERT INTO [" + tableName + "] (" +
                                string.Join(", ", insertColumns) +
                                ") VALUES (" +
                                string.Join(", ", parameters) + ")";
                    }

                    command.CommandText = query;

                    return command.ExecuteNonQuery();
                }
            }
        }

        public int UpdateRecord(
            string tableName,
            string primaryKey,
            string primaryValue,
            Dictionary<string, string> values)
        {
            ValidateTable(tableName);

            List<ColumnInfo> columns = GetColumns(tableName);

            if (!columns.Any(c => c.Name == primaryKey && c.IsPrimaryKey))
            {
                throw new Exception("Clave primaria no valida.");
            }

            List<string> updates = new List<string>();

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    int index = 0;

                    foreach (ColumnInfo column in columns)
                    {
                        if (column.IsIdentity || column.IsPrimaryKey)
                            continue;

                        if (!values.ContainsKey(column.Name))
                            continue;

                        string value = values[column.Name];

                        if (string.IsNullOrWhiteSpace(value))
                            continue;

                        string parameterName = "@p" + index;

                        updates.Add("[" + column.Name + "] = " + parameterName);

                        object parameterValue =
                            value.Trim().Equals("NULL",
                            StringComparison.OrdinalIgnoreCase)
                            ? DBNull.Value
                            : (object)value;

                        command.Parameters.AddWithValue(
                            parameterName, parameterValue);

                        index++;
                    }

                    if (updates.Count == 0)
                        return 0;

                    command.Parameters.AddWithValue("@primaryValue", primaryValue);

                    command.CommandText =
                        "UPDATE [" + tableName + "] SET " +
                        string.Join(", ", updates) +
                        " WHERE [" + primaryKey + "] = @primaryValue";

                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteRecord(
            string tableName,
            string primaryKey,
            string primaryValue)
        {
            ValidateTable(tableName);

            List<ColumnInfo> columns = GetColumns(tableName);

            if (!columns.Any(c => c.Name == primaryKey && c.IsPrimaryKey))
            {
                throw new Exception("Clave primaria no valida.");
            }

            string query =
                "DELETE FROM [" + tableName +
                "] WHERE [" + primaryKey + "] = @primaryValue";

            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@primaryValue", primaryValue);

                connection.Open();

                return command.ExecuteNonQuery();
            }
        }
    }
}
