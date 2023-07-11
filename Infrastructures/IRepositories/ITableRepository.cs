using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.IRepositories;

public interface ITableRepository : IGenericRepository<Table>
{
    Task<IEnumerable<Table>> GetTablesWithReservationDetailAsync();
    Task<Table?> GetTablesWithReservationDetailIdAsync(Guid id);
    Task<IEnumerable<Table>> FindSuitableTablesAsync(DateTime dateTimeBooking);
}
