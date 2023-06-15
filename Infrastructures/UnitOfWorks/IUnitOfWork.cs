using Infrastructures.IRepositories;
using Infrastructures.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.UnitOfWorks;

public interface IUnitOfWork
{
    public IAccountRepository AccountRepository { get; }
    public ITableRepository TableRepository { get; }
    public IReservationRepository ReservationRepository { get; }
    public ICustomerRepository CustomerRepository { get; }
    Task SaveChangesAsync();
}
