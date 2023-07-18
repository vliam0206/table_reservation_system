using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructures.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    private readonly IEmailService _emailService;

    public ReservationsController(IUnitOfWork unitOfWork, IMapper mapper,
                                        IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ReservationViewModel>>> GetReservations()
    {
        var reservations = (await _unitOfWork.ReservationRepository.GetReservationWithCustomer())
                                .OrderBy(x => x.CreationDate);
        var reservationDtos = _mapper.Map<IEnumerable<ReservationViewModel>>(reservations);

        return Ok(new ApiResponse()
        {
            Success = true,
            Data = reservationDtos
        });
    }

    [HttpGet("status/{status}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ReservationViewModel>>> GetReservationsByStatus(string status)
    {
        var reservations = (await _unitOfWork.ReservationRepository.GetReservationWithCustomer())
                                .Where(x => x.Status.ToString().Equals(status))
                                .OrderBy(x => x.CreationDate);
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
            reservation.CustomerId = customer.Id;

            var detailList = new List<ReservationTableDetail>();
            foreach(var tableId in reservationCreateDto.TablesId)
            {
                detailList.Add(new ReservationTableDetail
                {
                    ReservationId = reservation.Id,
                    TableId = tableId
                });
            }

            _unitOfWork.CustomerRepository.Add(customer);
            _unitOfWork.ReservationRepository.Add(reservation);
            await _unitOfWork.ReservationTableRepository.AddRangeAsync(detailList);

            await _unitOfWork.SaveChangesAsync();

            
            //Get project's directory and fetch DefaultTemplate content from EmailTemplates
            string exePath = Environment.CurrentDirectory.ToString();
            if (exePath.Contains(@"\bin\Debug\net7.0"))
                exePath = exePath.Remove(exePath.Length - (@"\bin\Debug\net7.0").Length);
            string FilePath = exePath + @"\EmailTemplates\DefaultTemplate.html";
            StreamReader streamreader = new StreamReader(FilePath);
            string mailText = streamreader.ReadToEnd();
            streamreader.Close();
            //Replace email informations
            mailText = mailText.Replace("[CustomerFullName]", customer.FullName);
            mailText = mailText.Replace("[DateTimeBooking]", reservation.DateTimeBooking.ToString("f"));
            mailText = mailText.Replace("[CustomerQuantity]", reservation.CustomerQuantity.ToString());
            mailText = mailText.Replace("[Note]", reservation.Note);
            // Send email to customer (send reservation information)
            await _emailService.SendMailAsync(new List<string> { customer.Email }, "Reservation confirmation", mailText);
            
            // return ok response to FE
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
    public async Task<IActionResult> UpdateReservation(Guid id, ReservationUpdateModel reservationUpdateDto)
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
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(reservation.CustomerId);
            //reservation =  _mapper.Map<ReservationModel, Reservation>(reservationUpdateDto);

            // update reservation
            reservation.DateTimeBooking = reservationUpdateDto.DateTimeBooking;
            reservation.CustomerQuantity = reservationUpdateDto.CustomerQuantity;
            reservation.Note = reservationUpdateDto.Note;
            reservation.Status = Enum.Parse<ReservationEnum>(reservationUpdateDto.Status);

            // Update customer
            customer.FullName = reservationUpdateDto.CustomerFullName;
            customer.PhoneNumber = reservationUpdateDto.CustomerPhoneNumber;
            customer.Email = reservationUpdateDto.CustomerEmail;

            _unitOfWork.ReservationRepository.Update(reservation);
            _unitOfWork.CustomerRepository.Update(customer);
            
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

    [HttpGet("status-list")]
    public async Task<IActionResult> GetAllReservationStatus()
    {
        var statusList = Enum.GetNames(typeof(ReservationEnum)).ToList();
        return Ok(statusList);
    }

    [HttpPut("update-status/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateReservationStatus(Guid id, TableStatus model)
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
            
            reservation.Status = Enum.Parse<ReservationEnum>(model.Status);

            _unitOfWork.ReservationRepository.Update(reservation);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse()
            {
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }
}

