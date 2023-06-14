using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.AuthModels;

public class LoginModel
{
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }
    [Required]
    [MaxLength(250)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
