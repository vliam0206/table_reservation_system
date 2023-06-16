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

public class ReservationTableRepository : GenericRepository<ReservationTableDetail>, IReservationTableRepository
{
    private AppDBContext _dbContext;
    public ReservationTableRepository(AppDBContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ReservationTableDetail?>> GetTableReservationDetailAsync()
    {
        return await _dbContext.ReservationTableDetails
            .Include(x => x.Reservation)            
            .Include(x => x.Table)            
            .ToListAsync();
    }

    public async Task<ReservationTableDetail?> GetTableReservationDetailAsync(Guid id)
    {
        return await _dbContext.ReservationTableDetails
            .Include(x => x.Reservation)
            .Include(x => x.Table)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
