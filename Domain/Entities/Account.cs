using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Account : BaseEntity
{
    [MaxLength(50)]
    public string UserName { get; set; } = null!;
    [MaxLength(250)]
    public string Password { get; set; } = null!;
    [MaxLength(100)]
    public string FullName { get; set; } = null!;
    public RoleEnum Role { get; set; } = RoleEnum.Staff;
}
