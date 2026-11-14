
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RegistroEquiposDeportivos
{
    class Program
    {
        static DatabaseManager database = new DatabaseManager();

        static void Main(string[] args)
        {
            Console.Title = "Sistema de Gestion de Equipos Deportivos";

            if (!database.TestConnection())
            {
                Console.WriteLine("No se pudo conectar con la base de datos.");
                Console.WriteLine("Presione una tecla para salir.");
                Console.ReadKey();
                return;
            }

            MainMenu();
        }

        static void MainMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();

                Console.WriteLine("==========================================");
                Console.WriteLine("       SISTEMA DE GESTION DEPORTIVA");
                Console.WriteLine("==========================================");
                Console.WriteLine();

                List<string> tables = database.GetTables();

                for (int i = 0; i < tables.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + tables[i]);
                }

                Console.WriteLine();
                Console.WriteLine("0. Salir del sistema");
                Console.WriteLine("==========================================");
                Console.Write("Seleccione una tabla: ");

                string input = Console.ReadLine();

                int option;

                if (!int.TryParse(input, out option))
                    continue;

                if (option == 0)
                {
                    exit = true;
                }
                else if (option >= 1 && option <= tables.Count)
                {
                    ManageTable(tables[option - 1]);
                }
            }

            Console.Clear();
            Console.WriteLine("Gracias por utilizar el sistema.");
        }

        static void ManageTable(string tableName)
        {
            bool back = false;

            while (!back)
            {
                Console.Clear();

                Console.WriteLine("==========================================");
                Console.WriteLine("          TABLA: " + tableName.ToUpper());
                Console.WriteLine("==========================================");
                Console.WriteLine();
                Console.WriteLine("1. Agregar registro");
                Console.WriteLine("2. Buscar registro");
                Console.WriteLine("3. Modificar registro");
                Console.WriteLine("4. Eliminar registro");
                Console.WriteLine("5. Listar registros");
                Console.WriteLine("0. Volver al menu principal");
                Console.WriteLine();
                Console.Write("Seleccione una opcion: ");

                string input = Console.ReadLine();
                int option;

                if (!int.TryParse(input, out option))
                    continue;

                try
                {
                    switch (option)
                    {
                        case 1:
                            AddRecord(tableName);
                            break;

                        case 2:
                            SearchRecords(tableName);
                            break;

                        case 3:
                            UpdateRecord(tableName);
                            break;

                        case 4:
                            DeleteRecord(tableName);
                            break;

                        case 5:
                            ListRecords(tableName);
                            break;

                        case 0:
                            back = true;
                            Console.Clear();
                            break;

                        default:
                            Console.WriteLine("Opcion no valida.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                    Pause();
                }
            }

            Console.Clear();
        }

        static void AddRecord(string tableName)
        {
            Console.Clear();

            Console.WriteLine("========== AGREGAR REGISTRO ==========");
            Console.WriteLine("Tabla: " + tableName);
            Console.WriteLine();

            List<ColumnInfo> columns = database.GetColumns(tableName);

            Dictionary<string, string> values =
                new Dictionary<string, string>();

            foreach (ColumnInfo column in columns)
            {
                if (column.IsIdentity)
                    continue;

                Console.Write(column.Name + " (" + column.DataType + ")");

                if (column.HasDefault)
                    Console.Write(" [ENTER para valor automatico]");

                Console.Write(": ");

                values[column.Name] = Console.ReadLine();
            }

            int result = database.AddRecord(tableName, values);

            Console.WriteLine();
            Console.WriteLine(result > 0
                ? "Registro agregado correctamente."
                : "No se pudo agregar el registro.");

            Pause();
        }

        static void SearchRecords(string tableName)
        {
            Console.Clear();

            Console.WriteLine("========== BUSCAR REGISTRO ==========");
            Console.WriteLine("Tabla: " + tableName);
            Console.WriteLine();

            List<ColumnInfo> columns = database.GetColumns(tableName);

            Console.WriteLine("Seleccione el criterio de busqueda:");

            for (int i = 0; i < columns.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + columns[i].Name);
            }

            Console.Write("\nSeleccione una columna: ");

            int option;

            if (!int.TryParse(Console.ReadLine(), out option) ||
                option < 1 || option > columns.Count)
            {
                Console.WriteLine("Seleccion no valida.");
                Pause();
                return;
            }

            string selectedColumn = columns[option - 1].Name;

            Console.Write("Escriba el valor que desea buscar: ");
            string value = Console.ReadLine();

            DataTable data =
                database.SearchRecords(tableName, selectedColumn, value);

            ShowData(data);

            Pause();
        }

        static void UpdateRecord(string tableName)
        {
            Console.Clear();

            Console.WriteLine("========== MODIFICAR REGISTRO ==========");
            Console.WriteLine("Tabla: " + tableName);
            Console.WriteLine();

            List<ColumnInfo> columns = database.GetColumns(tableName);

            ColumnInfo primaryKey = columns.FirstOrDefault(
                c => c.IsPrimaryKey);

            if (primaryKey == null)
            {
                Console.WriteLine("No se encontro la clave primaria.");
                Pause();
                return;
            }

            Console.Write("Ingrese " + primaryKey.Name + ": ");
            string primaryValue = Console.ReadLine();

            Dictionary<string, string> values =
                new Dictionary<string, string>();

            Console.WriteLine();
            Console.WriteLine("Escriba los nuevos valores.");
            Console.WriteLine("Presione ENTER para conservar el valor actual.");
            Console.WriteLine("Escriba NULL para establecer un campo como nulo.");
            Console.WriteLine();

            foreach (ColumnInfo column in columns)
            {
                if (column.IsIdentity || column.IsPrimaryKey)
                    continue;

                Console.Write(column.Name + ": ");
                values[column.Name] = Console.ReadLine();
            }

            int result = database.UpdateRecord(
                tableName, primaryKey.Name, primaryValue, values);

            Console.WriteLine();

            Console.WriteLine(result > 0
                ? "Registro modificado correctamente."
                : "No se encontro el registro o no se realizaron cambios.");

            Pause();
        }

        static void DeleteRecord(string tableName)
        {
            Console.Clear();

            Console.WriteLine("========== ELIMINAR REGISTRO ==========");
            Console.WriteLine("Tabla: " + tableName);
            Console.WriteLine();

            List<ColumnInfo> columns = database.GetColumns(tableName);

            ColumnInfo primaryKey = columns.FirstOrDefault(
                c => c.IsPrimaryKey);

            if (primaryKey == null)
            {
                Console.WriteLine("No se encontro la clave primaria.");
                Pause();
                return;
            }

            Console.Write("Ingrese " + primaryKey.Name + ": ");
            string primaryValue = Console.ReadLine();

            Console.Write("\n¿Esta seguro de eliminar este registro? (S/N): ");

            string confirmation = Console.ReadLine();

            if (confirmation.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                int result = database.DeleteRecord(
                    tableName, primaryKey.Name, primaryValue);

                Console.WriteLine(result > 0
                    ? "Registro eliminado correctamente."
                    : "No se encontro el registro.");
            }
            else
            {
                Console.WriteLine("Operacion cancelada.");
            }

            Pause();
        }

        static void ListRecords(string tableName)
        {
            Console.Clear();

            Console.WriteLine("========== LISTA DE REGISTROS ==========");
            Console.WriteLine("Tabla: " + tableName);
            Console.WriteLine();

            DataTable data = database.ListRecords(tableName);

            ShowData(data);

            Pause();
        }

        static void ShowData(DataTable data)
        {
            if (data.Rows.Count == 0)
            {
                Console.WriteLine("\nNo se encontraron registros.");
                return;
            }

            foreach (DataColumn column in data.Columns)
            {
                Console.Write(column.ColumnName + "\t");
            }

            Console.WriteLine();
            Console.WriteLine(new string('-', 100));

            foreach (DataRow row in data.Rows)
            {
                foreach (object value in row.ItemArray)
                {
                    string text = value == DBNull.Value
                        ? "NULL"
                        : value.ToString();

                    Console.Write(text + "\t");
                }

                Console.WriteLine();
            }

            Console.WriteLine("\nTotal de registros: " + data.Rows.Count);
        }

        static void Pause()
        {
            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }
    }
}
