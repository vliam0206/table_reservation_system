using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.ReservationModels;

public class ReservationModel
{
    public DateTime DateTimeBooking { get; set; } = default!; //Format: "M/d/yyyy 09:00:00"
    public int CustomerQuantity { get; set; } = 0;
    [MaxLength(250)]
    public string? Note { get; set; }
    public string CustomerFullName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerPhoneNumber { get; set; } = default!;
    public List<Guid> TablesId { get; set; }
}
public class ReservationUpdateModel : ReservationModel
{
    public string Status { get; set; }
}
