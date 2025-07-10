using System;
using System.Data.SQLite;
using System.Data.Common;

namespace StorageExample
{
    public interface Storage
    {
        public void save(String data);
        public String retrieve(int id);
    };

    public class DbStorage : Storage
    {
        private Dictionary<int, String> storage = new Dictionary<int, string>();
        private DbConnection connection;
        private String tableName;
        private String dbProviderName;
        DbProviderFactory factory;
        private String connectionString;
        private int counter;
        private String replaceSpecialChars(String str)
        {
            str.Replace("\"", "");
            str.Replace("\'", "");
            return str;
        }

        private void saveToDb()
        {
            return;
        }

        private void createTableIfNeedTo()
        {
            using (DbCommand command = factory.CreateCommand())
            {
                command.Connection = this.connection;
                command.CommandText = "CREATE TABLE IF NOT EXISTS " + this.tableName + " (Id INTEGER PRIMARY KEY, Value TEXT)";
                command.ExecuteNonQuery();
            }
        }

        private void readAllDataFromTable()
        {
            using (DbCommand command = this.factory.CreateCommand())
            {
                command.Connection = this.connection;
                command.CommandText = "SELECT * FROM " + this.tableName;
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int Id = Convert.ToInt32(reader["Id"]);
                    String Value = Convert.ToString(reader["Value"]);
                    this.storage[Id] = Value;
                    this.counter++;
                }
            }

        }

        private void loadFromDb()
        {
            this.createTableIfNeedTo();
            this.readAllDataFromTable();
            return;
        }


        private void saveToDb(int Id, String Value)
        {
            using (DbCommand command = this.factory.CreateCommand())
            {
                command.Connection = this.connection;
                command.CommandText = "INSERT INTO " + this.tableName + " (Value, Id) " + "VALUES ('" + Value  + "', " + Id + ")";
                command.ExecuteNonQuery();
            }
            return; 
        }

        public DbStorage(String dbProviderName, String connectionString, String tableName)
        {
            this.dbProviderName = dbProviderName;
            this.tableName = this.replaceSpecialChars(tableName);

            this.connectionString = connectionString;
            this.counter = 0;
            this.factory = DbProviderFactories.GetFactory(this.dbProviderName);

            this.connection = this.factory.CreateConnection();
            this.connection.ConnectionString = connectionString;
            this.connection.Open();

            this.loadFromDb();
        }

        ~DbStorage()
        {
            this.connection.Close();
        }

        public void save(String data)
        {
            this.counter++;
            this.storage.Add(this.counter, data);
            this.saveToDb(this.counter, data);
        }
        public String retrieve(int id)
        {
            return this.storage[id];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DbProviderFactories.RegisterFactory("Microsoft.Data.Sqlite", SQLiteFactory.Instance);

            string connectionString = "Data Source=test.db";
            string providerName = "Microsoft.Data.Sqlite";
            string tableName = "DATA_STORAGE";


            DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                DbStorage storage = new DbStorage(providerName, connectionString, tableName);
                storage.save("test1");
                storage.save("test2");
                storage.save("test3");
                Console.WriteLine(storage.retrieve(1));
                Console.WriteLine(storage.retrieve(2));
                Console.WriteLine(storage.retrieve(3));
            }

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                DbStorage storage = new DbStorage(providerName, connectionString, tableName);
                Console.WriteLine(storage.retrieve(1));
                Console.WriteLine(storage.retrieve(2));
                Console.WriteLine(storage.retrieve(3));
            }
        }
    }
}
