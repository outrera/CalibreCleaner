using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace CalibreCleaner
{
    public class BookMetadata
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
    }

    public class CleanerService
    {
        readonly Func<string, BookMetadata> stringToBookMetadata = path => 
        {
            int backslashIndex = path.IndexOf('\\') >= 0 ? path.IndexOf('\\') : 0;
            int openParensIndex = path.LastIndexOf('(');
            int closeParensIndex = path.LastIndexOf(')');
            return new BookMetadata
            {
                Author = path.Substring(0, backslashIndex).Trim(),
                Title = (openParensIndex > backslashIndex + 1) ? path.Substring(backslashIndex + 1, openParensIndex - backslashIndex - 1).ToString() : "",
                Id = (closeParensIndex > openParensIndex + 1) ? path.Substring(openParensIndex + 1, closeParensIndex - openParensIndex - 1).ToString() : ""
            };
        };

        public void findMissingBooks(string calibrePath, out List<BookMetadata> booksMissingInDatabase, out List<BookMetadata> booksMissingOnFilesystem)
        {
            List<string> pathsInDatabase = findPathsInDatabase(calibrePath);
            List<string> pathsOnFilesystem = findPathsOnFilesystem(calibrePath);
            booksMissingOnFilesystem = pathsInDatabase.Where(pathInDatabase => !pathsOnFilesystem.Contains(pathInDatabase))
                .OrderBy(path => path).Select(stringToBookMetadata).ToList();
            booksMissingInDatabase = pathsOnFilesystem.Where(pathOnFilesystem => !pathsInDatabase.Contains(pathOnFilesystem))
                .OrderBy(path => path).Select(stringToBookMetadata).ToList();
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
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
                            paths.Add(reader.GetString(0).Replace('/', '\\'));
                        }
                    }

                    connection.Close();
                }
            }
            return paths;
        }

        List<string> findPathsOnFilesystem(string calibrePath)
        {
            return Directory.GetDirectories(calibrePath).SelectMany(authorDirectory =>
                Directory.GetDirectories(Path.Combine(calibrePath, authorDirectory)).Select(bookDirectory =>
                    Path.Combine(authorDirectory, bookDirectory).Remove(0, calibrePath.Length + 1)
                )
            ).ToList();
        }
    }

    [Serializable]
    public class CleanerServiceException : Exception
    {
        public CleanerServiceException(string message) : base(message)
        {
        }
    }
}
