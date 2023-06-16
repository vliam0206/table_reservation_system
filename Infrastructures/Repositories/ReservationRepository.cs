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

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    private AppDBContext _dbContext;
    public ReservationRepository(AppDBContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Reservation?> GetReservationWithCustomer(Guid id)
    {
        return await _dbContext.Reservations
            .Include(r => r.CustomerInfo)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reservation?>> GetReservationWithCustomer()
    {
        return await _dbContext.Reservations
            .Include(r => r.CustomerInfo).ToListAsync();
    }
}
