using DataAccess;
using Domain.Entities;
using Infrastructures.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    private readonly AppDBContext _dbContext;
    public AccountRepository(AppDBContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Account? GetAccount(string username)
    {
        return _dbContext.Accounts.SingleOrDefault(x => x.UserName.Equals(username));
    }
}
