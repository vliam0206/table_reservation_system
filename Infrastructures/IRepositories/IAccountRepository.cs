﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.IRepositories;

public interface IAccountRepository : IGenericRepository<Account>
{
    Account? GetAccount(string username);
}
