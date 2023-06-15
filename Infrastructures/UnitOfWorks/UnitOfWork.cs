﻿using DataAccess;
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
    private ITableRepository _tableRepository;
    private IReservationRepository _reservationRepository;
    private ICustomerRepository _customerRepository;
    private AppDBContext _dbContext;

    public UnitOfWork(IAccountRepository accountRepository, 
                        ITableRepository tableRepository,
                        IReservationRepository reservationRepository, 
                        ICustomerRepository customerRepository,
                        AppDBContext dBContext)
    {
        _accountRepository = accountRepository;
        _tableRepository = tableRepository;
        _reservationRepository = reservationRepository;
        _customerRepository = customerRepository;
        _dbContext = dBContext;
    }

    public IAccountRepository AccountRepository => _accountRepository;

    public ITableRepository TableRepository => _tableRepository;

    public IReservationRepository ReservationRepository => _reservationRepository;

    public ICustomerRepository CustomerRepository => _customerRepository;

    public async Task SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
}
