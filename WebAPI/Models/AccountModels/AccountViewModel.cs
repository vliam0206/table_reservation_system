using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.AccountModels;

public class AccountViewModel
{
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string UserName { get; set; } = null!;
    [MaxLength(100)]
    public string FullName { get; set; } = null!;
    public string Role { get; set; } = null!;
}
