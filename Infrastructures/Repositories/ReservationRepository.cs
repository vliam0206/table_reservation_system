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
            .Include(r => r.ReservationTableDetails)
            .ThenInclude(d => d.Table)
            .Include(r => r.CustomerInfo)
            .Select(r => new Reservation
            {
                Id = r.Id,
                CreationDate = r.CreationDate,
                ReservationTableDetails = r.ReservationTableDetails.Select(d => new ReservationTableDetail
                {
                    TableId = d.TableId,
                    ReservationId = d.ReservationId,
                    Table = d.Table,
                    Id = d.Id
                }).ToList(),
                DateTimeBooking = r.DateTimeBooking,
                CustomerQuantity = r.CustomerQuantity,
                Note = r.Note,
                CustomerInfo = new Customer
                {
                    FullName = r.CustomerInfo.FullName,
                    Email = r.CustomerInfo.Email,
                    PhoneNumber = r.CustomerInfo.PhoneNumber
                },
                Status = r.Status
            })
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reservation?>> GetReservationWithCustomer()
    {
        return await _dbContext.Reservations
            .Include(r => r.ReservationTableDetails)
            .ThenInclude(d => d.Table)
            .Include(r => r.CustomerInfo)
            .Select(r => new Reservation
            {
                Id = r.Id,
                CreationDate = r.CreationDate,
                ReservationTableDetails = r.ReservationTableDetails.Select(d => new ReservationTableDetail
                {
                    TableId = d.TableId,
                    ReservationId = d.ReservationId,
                    Table = d.Table,
                    Id = d.Id
                }).ToList(),
                DateTimeBooking = r.DateTimeBooking,
                CustomerQuantity = r.CustomerQuantity,
                Note = r.Note,
                CustomerInfo = new Customer
                {
                    FullName = r.CustomerInfo.FullName,
                    Email = r.CustomerInfo.Email,
                    PhoneNumber = r.CustomerInfo.PhoneNumber
                },
                Status = r.Status
            }).ToListAsync();
    }
}
