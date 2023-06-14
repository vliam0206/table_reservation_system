using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess;

public class DBInitializer
{
    private static AppDBContext context;
    public static void InitializeAccount()
    {
        if (context.Accounts.Any())
        {
            return;
        }
        var accounts = new Account[]
        {
            new Account() {UserName="lamlam", Password="123".Hash(), FullName="Lam Vo"}
        };
        context.Accounts.AddRange(accounts);
        context.SaveChanges();
    }
    public static void InitializeCustomer()
    {
        if (context.Customers.Any())
        {
            return;
        }

        var customers = new Customer[]
        {
            new Customer { FullName = "John Doe", Email = "john@example.com", PhoneNumber = "1234567890" },
            new Customer { FullName = "Jane Smith", Email = "jane@example.com", PhoneNumber = "9876543210" },
            new Customer { FullName = "Lam Vo", Email = "v.trclam@gmail.com", PhoneNumber = "0876543210" }
        };

        context.Customers.AddRange(customers);
        context.SaveChanges();
    }

    public static void InitializeTable()
    {
        if (context.Tables.Any())
        {
            return;
        }

        var tables = new Table[]
        {
            new Table { Code = "T1", SeatQuantity = 4},
            new Table { Code = "T2", SeatQuantity = 6}
        };

        context.Tables.AddRange(tables);
        context.SaveChanges();
    }

    public static void InitializeReservation()
    {
        if (context.Reservations.Any())
        {
            return;
        }

        var customers = context.Customers.ToList();
        var timeList = ReservationTime.TimeList;
        var reservations = new Reservation[]
        {
            new Reservation
            {
                DateTimeBooking = DateTime.Parse(DateTime.Now.AddDays(1).ToString("M/d/yyyy") + " " + timeList[12]),
                CustomerQuantity = 4,
                CustomerId = customers[0].Id,
                Note = "Table near the window",
                Status = ReservationEnum.Confirmed,
                CreationDate = DateTime.Now                
            },
            new Reservation
            {
                DateTimeBooking = DateTime.Parse(DateTime.Now.AddDays(2).ToString("M/d/yyyy") + " " + timeList[5]),
                CustomerQuantity = 6,
                CustomerId = customers[1].Id,
                Note = "Special occasion",
                Status = ReservationEnum.Waiting,
                CreationDate = DateTime.Now
            }
        };

        context.Reservations.AddRange(reservations);
        context.SaveChanges();
    }

    public static void InitializeReservationTableDetail()
    {
        if (context.ReservationTableDetails.Any())
        {
            return;
        }

        var reservations = context.Reservations.ToList();
        var tables = context.Tables.ToList();

        var reservationTableDetails = new ReservationTableDetail[]
        {
            new ReservationTableDetail { ReservationId = reservations[0].Id, TableId = tables[0].Id },
            new ReservationTableDetail { ReservationId = reservations[0].Id, TableId = tables[1].Id },
            new ReservationTableDetail { ReservationId = reservations[1].Id, TableId = tables[1].Id }
        };

        context.ReservationTableDetails.AddRange(reservationTableDetails);
        context.SaveChanges();
    }

    public static void InitializeData(AppDBContext _context)
    {
        context = _context;
        InitializeAccount();
        InitializeCustomer();
        InitializeTable();
        InitializeReservation();
        InitializeReservationTableDetail();
    }
}
