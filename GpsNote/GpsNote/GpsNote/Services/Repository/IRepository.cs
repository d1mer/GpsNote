using System;
using GpsNote.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace GpsNote.Services.Repository
{
    public interface IRepository
    {
        Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new();
        Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate) where T : IEntityBase, new();
    }
}