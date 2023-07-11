using Domain.Entities;

namespace WebAPI.Models.ReservationModels;

public class ReservationViewModel : ReservationModel
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public ICollection<ReservationTableDetail> ReservationTableDetails { get; set; }
    public string Status { get; set; }
}
