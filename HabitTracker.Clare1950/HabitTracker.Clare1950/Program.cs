using System;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace habitTracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

            GetUserInput();
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp ==false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("\nType 1 to View All Records");
                Console.WriteLine("\nType 2 to Insert Record");
                Console.WriteLine("\nType 3 to Delete Record");
                Console.WriteLine("\nType 4 to Update Record");
                Console.WriteLine("\n------------------------------------------------------------------");


                string menuInput = Console.ReadLine();


                switch (menuInput)
                {
                    case "0":
                        Console.WriteLine("The App is Closing");
                        closeApp = true;
                        break;
                    case "1":
                        ViewAllRecords();
                        break;

                    case "2":
                        InsertRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    //case "4":
                    //    UpdateRecord();
                    default:
                        break;
                }
            }
        }

        private static void ViewAllRecords()
        {

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM drinking_water";
                List<DrinkingWater> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-GB")),
                                Quantity = reader.GetInt32(2)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();

                Console.WriteLine("--------------------------------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}");
                }
            }
        }

        private static void InsertRecord()
        {
            string date = GetDateInput();
            int quantity = GetNumberInput("\n\nPlease insert the number of glasses as a whole number");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', '{quantity}')";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void DeleteRecord()
        {
            Console.Clear();
            ViewAllRecords();
            Console.WriteLine("\n\nPlease type the id of the record you would like to delete.  Type 0 to return to the main menu.\n\n");
            string idInput = Console.ReadLine();

            if (idInput =="0") GetUserInput();

         
         
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id='{idInput}'";
               int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"A record with Id {idInput} doesn't exist\n\n");
                    DeleteRecord();
                }
                connection.Close();
            }

        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy).  Type 0 to return to the main menu.\n\n");
            string dateInput = Console.ReadLine();

            if (dateInput =="0") GetUserInput();

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();

            if (numberInput =="0") GetUserInput();
            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;

        }
    }
}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }

}


