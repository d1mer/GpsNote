using System;
using System.Collections.Generic;
using System.Text;
using GpsNote.Models;
using System.Threading.Tasks;

namespace GpsNote.Services.Repository
{
    public interface IRepository
    {
        Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new();
        Task<T> GetEntityAsync<T>(T entity) where T : IEntityBase, new();
    }
}