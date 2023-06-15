using DataAccess;
using Domain.Entities;
using Infrastructures.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDBContext dbContext) : base(dbContext)
    {
    }
}
