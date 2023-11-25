using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using SQLite;

namespace Announcement_Platform
{
    public class DataBase
    {

        readonly SQLiteAsyncConnection _database;

        public DataBase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Announcement>().Wait();
            _database.CreateTableAsync<Company>().Wait();
            _database.CreateTableAsync<Courses>().Wait();
            _database.CreateTableAsync<Education>().Wait();
            _database.CreateTableAsync<Links>().Wait();
            _database.CreateTableAsync<Occupation>().Wait();
            _database.CreateTableAsync<Skills>().Wait();
            _database.CreateTableAsync<Skills>().Wait();
            _database.CreateTableAsync<WorkingExperience>().Wait();
            _database.CreateTableAsync<WorkingSummary>().Wait();
            _database.CreateTableAsync<Account>().Wait();
        }
        public Task<List<T>> GetItemsAsync<T>() where T : new()
        {
            return _database.Table<T>().ToListAsync();
        }

        public Task<int> SaveItemAsync<T>(T item)
        {
            return _database.InsertAsync(item);
        }

    }
}
