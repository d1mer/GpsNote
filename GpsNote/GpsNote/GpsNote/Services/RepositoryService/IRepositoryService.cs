using System;
using GpsNote.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.RepositoryService
{
    public interface IRepositoryService
    {
        Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new();

        Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate) where T : IEntityBase, new();

        Task<List<T>> GetAllAsync<T>() where T : IEntityBase, new();

        Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate) where T : IEntityBase, new();

        Task DeleteAllAsync<T>() where T : IEntityBase, new();
    }
}