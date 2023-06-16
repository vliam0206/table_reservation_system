using Domain.Entities;
using WebAPI.Models.ReservationModels;
using WebAPI.Models.TableModels;

namespace WebAPI.Models;

public class TableReservationDetailViewModel
{
    public Guid Id { get; set; }
    public TableViewModel Table { get; set; }
    public ReservationViewModel Reservation { get; set; }
}
