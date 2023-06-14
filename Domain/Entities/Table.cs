using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities;

public class Table :BaseEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = null!;
    public int SeatQuantity { get; set; } = 0;
    public TableEnum Status { get; set; } = TableEnum.Empty;

    public ICollection<ReservationTableDetail> ReservationTableDetails { get; set; }
}
