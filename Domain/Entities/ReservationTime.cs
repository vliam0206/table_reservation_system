using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class ReservationTime : BaseEntity
{
    [MaxLength(50)]
    public string Time { get; set; } = default!;
}
