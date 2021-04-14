using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GpsNote.Models;
using SQLite;

namespace GpsNote.Services.RepositoryService
{
    public class RepositoryService : IRepositoryService
    {
        private Lazy<SQLiteAsyncConnection> _database;


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


        #region -- IRepository implementation --

        public Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate) 
            where T : IEntityBase, new()
        {
            return _database.Value.FindAsync<T>(predicate);
        }

        public Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.UpdateAsync(entity);
        }

        public Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new()
        {
            return _database.Value.Table<T>().ToListAsync();
        }

        public Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate) where T : IEntityBase, new()
        {
            return _database.Value.Table<T>().Where(predicate).ToListAsync();
        }

        public Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new()
        {
            return _database.Value.DeleteAsync(entity);
        }

        public Task DeleteAllAsync<T>() where T : IEntityBase, new()
        {
            return _database.Value.DeleteAllAsync<T>();
        }

        public async Task<T> FindEntity<T>(Expression<Func<T, bool>> predicate) where T : IEntityBase, new()
        {
            return await _database.Value.FindAsync<T>(predicate);
        }

        #endregion
    }
}