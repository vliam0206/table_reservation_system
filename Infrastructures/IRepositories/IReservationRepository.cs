using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.IRepositories;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task<IEnumerable<Reservation?>> GetReservationWithCustomer();
    Task<IEnumerable<Reservation?>> GetReservationWithCustomer(Guid id);
}
