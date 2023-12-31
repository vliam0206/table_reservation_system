﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Reservation : BaseEntity
{
    #region Properties
    public DateTime DateTimeBooking { get; set; } = default!; //Format: "M/d/yyyy 09:00:00"
    public int CustomerQuantity { get; set; } = 0;
    public Guid CustomerId { get; set; }
    [MaxLength(250)]
    public string? Note { get; set; }
    public ReservationEnum Status { get; set; } = ReservationEnum.Waiting;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    #endregion

    #region Relationship
    public ICollection<ReservationTableDetail> ReservationTableDetails { get; set; } = default!;
    public Customer CustomerInfo { get; set; } = default!;
    #endregion

}
