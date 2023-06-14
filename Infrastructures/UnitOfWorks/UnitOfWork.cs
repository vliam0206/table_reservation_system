using Infrastructures.IRepositories;
using Infrastructures.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private IAccountRepository _accountRepository;

    public UnitOfWork(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public IAccountRepository AccountRepository => _accountRepository;
}
