using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class ReservationTableDetail : BaseEntity
{
    public Guid TableId { get; set; }
    public Guid ReservationId { get; set; }

    public Table Table { get; set; }
    public Reservation Reservation { get; set; }
}
