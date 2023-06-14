using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions options) : base(options)
    {
    }
    #region DBSets
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationTableDetail> ReservationTableDetails { get; set; }
    public DbSet<Table> Tables { get; set; }
    #endregion

    #region Connect DB
    protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
    {
        if (!optionsbuilder.IsConfigured)
        {
            optionsbuilder.UseSqlServer(GetConnectionStrings());
        }
    }
    private string GetConnectionStrings()
    {
        IConfiguration config = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .Build();
        return config["ConnectionStrings:DefaultDB"];
    }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

}
