using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.TableModels;

public class TableModel
{
    [MaxLength(50)]
    public string Code { get; set; } = null!;
    public int SeatQuantity { get; set; } = 0;
}

public class TableUpdateModel : TableModel
{
    public string Status { get;set; }
}

public class TableStatus
{
    public string Status { get; set; }
}
