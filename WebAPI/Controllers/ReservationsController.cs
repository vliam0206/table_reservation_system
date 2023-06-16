using AutoMapper;
using Domain.Entities;
using Infrastructures.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Models.ReservationModels;
using WebAPI.Models.TableModels;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReservationsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    //[HttpGet]
    //public async Task<IActionResult<IEnumerable<TableViewModel>>> GetSuitableTables()
    //{

    //}

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ReservationViewModel>>> GetReservations()
    {
        var reservations = await _unitOfWork.ReservationRepository.GetReservationWithCustomer();
        var reservationDtos = _mapper.Map<IEnumerable<ReservationViewModel>>(reservations);

        return Ok(new ApiResponse()
        {
            Success = true,
            Data = reservationDtos
        });
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ReservationViewModel>> GetReservation(Guid id)
    {
        var reservation = await _unitOfWork.ReservationRepository.GetReservationWithCustomer(id);
        if (reservation == null)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                ErrorMessage = "Reservation not found"
            });
        }

        var reservationDto = _mapper.Map<ReservationViewModel>(reservation);

        return Ok(new ApiResponse()
        {
            Success = true,
            Data = reservationDto
        });
    }

    [HttpPost]
    public async Task<ActionResult<ReservationViewModel>> CreateReservation(ReservationModel reservationCreateDto)
    {
        try
        {
            var customer = new Customer
            {
                FullName = reservationCreateDto.CustomerFullName,
                Email = reservationCreateDto.CustomerEmail,
                PhoneNumber = reservationCreateDto.CustomerPhoneNumber
            };

            var reservation = _mapper.Map<Reservation>(reservationCreateDto);
            reservation.CustomerInfo = customer;

            _unitOfWork.CustomerRepository.Add(customer);
            _unitOfWork.ReservationRepository.Add(reservation);
            await _unitOfWork.SaveChangesAsync();

            var createdReservationDto = _mapper.Map<ReservationViewModel>(reservation);

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, new ApiResponse(){
                Success = true,
                Data= createdReservationDto
            });
        } catch (Exception ex)
        {
            return BadRequest(new ApiResponse()
            {
                Success= false,
                ErrorMessage = ex.Message
            });
        }
    }


    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateReservation(Guid id, ReservationModel reservationUpdateDto)
    {
        try
        {
            var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "Reservation not found"
                });
            }

            _mapper.Map(reservationUpdateDto, reservation);

            //// Set the CustomerId based on the provided Customer's ID
            //reservation.CustomerId = reservationUpdateDto.CustomerId;

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        } catch (Exception ex)
        {
            return BadRequest(new ApiResponse()
            {
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteReservation(Guid id)
    {
        try
        {
            var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "Reservation not found"
                });
            }

            _unitOfWork.ReservationRepository.Remove(reservation);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        } catch(Exception ex)
        {
            return BadRequest(new ApiResponse()
            {
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }
}

