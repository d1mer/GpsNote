using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GpsNote.Models;
using SQLite;

namespace GpsNote.Services.Repository
{
    public class RepositoryService : IRepository
    {
        #region -- Private fields --

        private Lazy<SQLiteAsyncConnection> _database;

        #endregion

        public RepositoryService()
        {
            _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "dbNote.db3");

                SQLiteAsyncConnection database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<User>().Wait();
                return database;
            });
        }

        #region -- IRepository implementation --

        public Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate) 
            where T : IEntityBase, new() =>
            _database.Value.FindAsync<T>(predicate);

        public Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new() =>
            _database.Value.InsertAsync(entity);

        #endregion
    }
}