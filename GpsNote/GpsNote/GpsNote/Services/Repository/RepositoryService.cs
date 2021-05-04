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
        private SQLiteAsyncConnection _database;


        public RepositoryService()
        {
            _database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "dbNote.db3"));
            _database.CreateTableAsync<User>();
            _database.CreateTableAsync<PinModel>();

        }

        #region -- IRepository implementation --

        public Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate) 
            where T : IEntityBase, new()
        {
            return _database.FindAsync<T>(predicate);
        }

        public async Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.InsertAsync(entity);
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await  _database.UpdateAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : IEntityBase, new()
        {
            return await _database.Table<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate) where T : IEntityBase, new()
        {
            return await _database.Table<T>().Where(predicate).ToListAsync();
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.DeleteAsync(entity);
        }

        public async Task DeleteAllAsync<T>() where T : IEntityBase, new()
        {
            await _database.DeleteAllAsync<T>();
        }

        public async Task<T> FindEntity<T>(Expression<Func<T, bool>> predicate) where T : IEntityBase, new()
        {
            return await _database.FindAsync<T>(predicate);
        }

        #endregion
    }
}