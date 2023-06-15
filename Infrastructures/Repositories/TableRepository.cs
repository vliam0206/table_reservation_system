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
public class TableRepository : ITableRepository
{
    private readonly AppDBContext _dbContext;

    public TableRepository(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Table>> GetTablesAsync()
    {
        return await _dbContext.Tables.ToListAsync();
    }

    public async Task<Table?> GetTableByIdAsync(Guid id)
    {
        return await _dbContext.Tables.FirstOrDefaultAsync(t => t.Id == id);
    }

    public void AddTable(Table table)
    {
        _dbContext.Tables.Add(table);
    }

    public void RemoveTable(Table table)
    {
        _dbContext.Tables.Remove(table);
    }
}

