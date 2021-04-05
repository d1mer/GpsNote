using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GpsNote.Models;
using SQLite;
using Xamarin.Forms.GoogleMaps;

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
                database.CreateTableAsync<PinModel>().Wait();
                return database;
            });
        }


        #region -- IRepository implementation --

        public Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate) 
            where T : IEntityBase, new() =>
            _database.Value.FindAsync<T>(predicate);

        public Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new() =>
            _database.Value.InsertAsync(entity);


        public Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new() => _database.Value.Table<T>().ToListAsync();


        public async Task<List<Pin>> GetPinsAsync(int owner) 
        {
            List<PinModel> res = await _database.Value.Table<PinModel>().Where(p => p.Owner == owner).ToListAsync();
            List<Pin> list = new List<Pin>();
            res.ForEach(p => list.Add(p.Pin));
            return list;
        }

        #endregion
    }
}