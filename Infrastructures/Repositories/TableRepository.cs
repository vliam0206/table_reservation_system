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
public class TableRepository : GenericRepository<Table>, ITableRepository
{
    private readonly AppDBContext _dbContext;

    public TableRepository(AppDBContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Table>> GetTablesWithReservationDetailAsync()
    {
        return await _dbContext.Tables
            .Include(x => x.ReservationTableDetails)
            .ToListAsync();
    }

    public async Task<Table?> GetTablesWithReservationDetailIdAsync(Guid id)
    {
        return await _dbContext.Tables
            .Include(x => x.ReservationTableDetails)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}

