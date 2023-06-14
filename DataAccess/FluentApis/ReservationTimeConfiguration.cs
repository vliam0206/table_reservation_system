using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.FluentApis;

public class ReservationTimeConfiguration : IEntityTypeConfiguration<ReservationTime>
{
    public void Configure(EntityTypeBuilder<ReservationTime> builder)
    {
        builder.ToTable(nameof(ReservationTime));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
        builder.Property(x => x.Time).IsRequired();
    }
}
