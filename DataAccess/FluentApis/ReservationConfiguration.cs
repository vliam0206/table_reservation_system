using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.FluentApis;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable(nameof(Reservation));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
        builder.Property(x => x.CreationDate).HasDefaultValueSql("getutcdate()");

        builder.HasOne(x => x.Table)
            .WithMany(t => t.Reservations)
            .HasForeignKey(x => x.TableId);

        builder.HasOne(x => x.CustomerInfo)
            .WithMany(t => t.Reservations)
            .HasForeignKey(x => x.CustomerId);
    }
}
