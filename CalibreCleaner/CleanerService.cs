using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace CalibreCleaner
{
    public class CleanerService
    {
        List<string> findPathsInDatabase(string calibrePath)
        {
            string databasePath = String.Format(@"{0}\metadata.db", calibrePath);
            if (!File.Exists(databasePath))
            {
                throw new CleanerServiceException("Could not open a \"metadata.db\" database file in the specified path");
            }
            List<string> paths = new List<string>();
            using (SQLiteConnection connection = new SQLiteConnection(String.Format(@"Data Source={0}", databasePath)))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    connection.Open();

                    command.CommandText = "SELECT path FROM books";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            paths.Add(reader.GetString(0));
                            Console.WriteLine(reader.GetString(0));
                        }
                    }

                    connection.Close();
                }
            }
            return paths;
            /*
            using (SQLiteConnection connection = new SQLiteConnection(@"Data Source=c:\Users\Steve\Desktop\test.db3"))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    connection.Open();

                    command.CommandText = @"CREATE TABLE IF NOT EXISTS [MyTable] (
                                          [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                          [Key] NVARCHAR(2048)  NULL,
                                          [Value] VARCHAR(2048)  NULL
                                          )";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO MyTable (Key, Value) VALUES ('key one', 'value one')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO MyTable (Key, Value) VALUES ('key two', 'value two')";
                    command.ExecuteNonQuery();

                    command.CommandText = "SELECT * FROM MyTable";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} - {1}", reader["key"], reader["value"]);
                        }
                    }

                    connection.Close();
                }
            }
            */
        }
    }

    public class CleanerServiceException : Exception
    {
        public CleanerServiceException(string message) : base(message)
        {
        }
    }
}
