using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GpsNote.Models;
using SQLite;

namespace GpsNote.Services.Repository
{
    public class RepositoryService : IRepository
    {
        #region fields

        private Lazy<SQLiteAsyncConnection> database;

        #endregion

        public RepositoryService()
        {
            database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "dbNote.db3");

                SQLiteAsyncConnection _database = new SQLiteAsyncConnection(path);

                _database.CreateTableAsync<User>().Wait();
                return _database;
            });
        }

    }
}