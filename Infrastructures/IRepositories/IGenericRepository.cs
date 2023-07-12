using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.IRepositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    void Add(T entity);
    Task AddRangeAsync(List<T> entities);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> ExistsAsync(Guid id);
    Task<int> CountAsync();
    Task SaveChangesAsync();
}