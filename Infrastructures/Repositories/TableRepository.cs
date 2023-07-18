using DataAccess;
using Domain.Entities;
using Domain.Enums;
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

    public async Task<IEnumerable<Table>> FindSuitableTablesAsync(DateTime dateTimeBooking)
    {
        var tableList = (from table in _dbContext.Tables
                         join detail in _dbContext.ReservationTableDetails on table.Id equals detail.TableId
                         join reservation in _dbContext.Reservations on detail.ReservationId equals reservation.Id
                         where detail.Reservation.DateTimeBooking != dateTimeBooking
                               && table.Status == TableEnum.Active
                         select table).Distinct();
        return await tableList
                        .OrderBy(x => x.SeatQuantity)
                        .ToListAsync();
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

