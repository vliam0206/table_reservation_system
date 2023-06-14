using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Customer : BaseEntity
{
    #region Properties
    [MaxLength(100)]
    public string FullName { get; set; } = null!;
    [MaxLength(250)]
    public string Email { get; set; } = null!;
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = null!;
    #endregion

    public ICollection<Reservation> Reservations { get; set; }
}
