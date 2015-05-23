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
            string databasePath = Path.Combine(calibrePath, "metadata.db");
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
        }

        List<string> findPathsOnFilesystem(string calibrePath)
        {
            List<string> paths = new List<string>();
            foreach (string authorDirectory in Directory.GetDirectories(calibrePath))
            {
                foreach (string bookDirectory in Directory.GetDirectories(Path.Combine(calibrePath, authorDirectory)))
                {
                    paths.Add(Path.Combine(authorDirectory, bookDirectory));
                }
            }
            return paths;
        }
    }

    public class CleanerServiceException : Exception
    {
        public CleanerServiceException(string message) : base(message)
        {
        }
    }
}
