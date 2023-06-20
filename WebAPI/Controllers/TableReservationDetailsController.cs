using AutoMapper;
using Infrastructures.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.TableModels;
using WebAPI.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TableReservationDetailsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TableReservationDetailsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllTableReservationDetails()
    {
        try
        {
            var tableReservationDetails = await _unitOfWork.ReservationTableRepository.GetTableReservationDetailAsync();
            return Ok(new ApiResponse
            {
                Success = true,
                Data = _mapper.Map<IEnumerable<TableReservationDetailViewModel>>(tableReservationDetails)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Success = false, ErrorMessage = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetTableReservationDetail(Guid id)
    {
        try
        {
            var tableReservationDetail = await _unitOfWork.ReservationTableRepository.GetTableReservationDetailAsync(id);
            if (tableReservationDetail == null)
                return NotFound();

            return Ok(new ApiResponse { 
                Success = true, 
                Data = _mapper.Map<TableReservationDetailViewModel>(tableReservationDetail)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Success = false, ErrorMessage = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTableReservationDetail(ReservationTableDetail tableReservationDetail)
    {
        try
        {
            _unitOfWork.ReservationTableRepository.Add(tableReservationDetail);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTableReservationDetail),
                new ApiResponse { 
                    Success = true, 
                    Data = _mapper.Map<TableReservationDetailViewModel>(tableReservationDetail)
                });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Success = false, ErrorMessage = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateTableReservationDetail(Guid id, ReservationTableDetail updatedTableReservationDetail)
    {
        try
        {
            var tableReservationDetail = await _unitOfWork.ReservationTableRepository.GetByIdAsync(id);
            if (tableReservationDetail == null)
                return NotFound();

            tableReservationDetail.TableId = updatedTableReservationDetail.TableId;
            tableReservationDetail.ReservationId = updatedTableReservationDetail.ReservationId;

            _unitOfWork.ReservationTableRepository.Update(tableReservationDetail);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Success = false, ErrorMessage = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteTableReservationDetail(Guid id)
    {
        try
        {
            var tableReservationDetail = await _unitOfWork.ReservationTableRepository.GetByIdAsync(id);
            if (tableReservationDetail == null)
                return NotFound();

            _unitOfWork.ReservationTableRepository.Remove(tableReservationDetail);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Success = false, ErrorMessage = ex.Message });
        }
    }
}

