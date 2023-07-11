using DataAccess;
using Domain.Entities;
using Infrastructures.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly AppDBContext _dbContext;

    public GenericRepository(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public void Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _dbContext.Entry<T>(entity).State = EntityState.Modified;
    }

    public void Remove(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbContext.Set<T>().AnyAsync(e => e.Id == id);
    }

    public async Task<int> CountAsync()
    {
        return await _dbContext.Set<T>().CountAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}

