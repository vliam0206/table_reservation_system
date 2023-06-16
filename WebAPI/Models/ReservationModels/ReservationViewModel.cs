namespace WebAPI.Models.ReservationModels;

public class ReservationViewModel : ReservationModel
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
}
