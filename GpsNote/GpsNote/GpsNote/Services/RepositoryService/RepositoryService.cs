using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GpsNote.Models;
using SQLite;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.RepositoryService
{
    public class RepositoryService : IRepositoryService
    {
        #region -- Private fields --

        private Lazy<SQLiteAsyncConnection> _database;

        #endregion



        #region -- Constructor --

        public RepositoryService()
        {
            _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "dbNote.db3");

                SQLiteAsyncConnection database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<User>().Wait();
                database.CreateTableAsync<PinModel>().Wait();
                return database;
            });
        }

        #endregion


        #region -- IRepository implementation --

        public Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate) 
            where T : IEntityBase, new() => _database.Value.FindAsync<T>(predicate);

        public Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new() =>
            _database.Value.InsertAsync(entity);


        public Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new() => _database.Value.Table<T>().ToListAsync();

        public Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate) where T : IEntityBase, new() =>
            _database.Value.Table<T>().Where(predicate).ToListAsync();

        public Task DeleteAllAsync<T>() where T : IEntityBase, new() => _database.Value.DeleteAllAsync<T>();

        #endregion
    }
}