using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.FluentApis;

public class ReservationTableDetailConfiguration : IEntityTypeConfiguration<ReservationTableDetail>
{
    public void Configure(EntityTypeBuilder<ReservationTableDetail> builder)
    {
        builder.ToTable(nameof(ReservationTableDetail));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");

        builder.HasOne(x => x.Reservation)
                .WithMany(r => r.ReservationTableDetails)
                .HasForeignKey(x => x.ReservationId);

        builder.HasOne(x => x.Table)
                .WithMany(r => r.ReservationTableDetails)
                .HasForeignKey(x => x.TableId);
    }
}
