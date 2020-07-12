using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStorageAPI.Infrastructure
{
    public interface IRepository<T>
    {
        Task<IEnumerable<Guid>> GetAllIDs();

        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(Guid guid);

        Task Add(T model);

        Task Update(Guid guid, T model);

        Task Remove(Guid guid);
    }
}
