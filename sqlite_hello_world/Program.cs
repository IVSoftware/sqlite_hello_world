using System;
using System.IO;
using SQLite;

namespace sqlite_hello_world
{
    class Program
    {
        static void Main(string[] args)
        {
            var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "sqlite_hello_world");
            Directory.CreateDirectory(appData);
            var connectionString = Path.Combine(appData, "MyDatabase.db");

            using (var cnx = new SQLiteConnection(connectionString))
            {
                cnx.CreateTable<Record>();

                var newRecord = new Record { Description = "Hello" };

                var guidToFind = newRecord.guid;

                cnx.Insert(newRecord);

                var recordset = cnx.Query<Record>($"SELECT * FROM items WHERE guid = '{guidToFind}'");

                Console.WriteLine($"Found: {recordset[0].Description}");

                // Change a value and update the database
                newRecord.Description = $"{recordset[0].Description} [Edited]";
                cnx.Update(newRecord);

                // Retrieve the record a second time from the database
                recordset = cnx.Query<Record>($"SELECT * FROM items WHERE guid = '{guidToFind}'");

                Console.WriteLine($"Found: {recordset[0].Description}");
            }

            Console.ReadKey();
        }

        [Table("items")]
        class Record
        {
            [PrimaryKey]
            public string guid { get; set; } = Guid.NewGuid().ToString().Trim().TrimStart('{').TrimEnd('}');
            public string Description { get; set; } = string.Empty;
        }
    }
}
