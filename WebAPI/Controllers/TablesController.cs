using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructures.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Models.TableModels;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TablesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TablesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TableViewModel>>> GetTables()
    {
        var tables = await _unitOfWork.TableRepository.GetAllAsync();
        return Ok(new ApiResponse
        {
            Success = true,
            Data = tables//_mapper.Map<IEnumerable<TableViewModel>>(tables)
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TableViewModel>> GetTable(Guid id)
    {
        var table = await _unitOfWork.TableRepository.GetByIdAsync(id);

        if (table == null)
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                ErrorMessage = "Table not found"
            });
        }

        return Ok(new ApiResponse
        {
            Success = true,
            Data = _mapper.Map<TableViewModel>(table)
        });
    }

    [HttpPost]
    public async Task<ActionResult<Table>> CreateTable(TableModel model)
    {
        try
        {
            var table = _mapper.Map<Table>(model);
            _unitOfWork.TableRepository.Add(table);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTable), new { id = table.Id }, new ApiResponse
            {
                Success = true,
                Data = table
            });
        } catch (Exception ex)
        {
            return BadRequest(new ApiResponse()
            {
                Success = false,
                ErrorMessage = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTable(Guid id, TableModel updatedTable)
    {
        try
        {
            var table = await _unitOfWork.TableRepository.GetByIdAsync(id);

            if (table == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "Table not found"
                });
            }


            table.Code = updatedTable.Code;
            table.SeatQuantity = updatedTable.SeatQuantity;
            table.Status = Enum.Parse<TableEnum>(updatedTable.Status);

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
    public async Task<IActionResult> DeleteTable(Guid id)
    {
        try
        {
            var table = await _unitOfWork.TableRepository.GetByIdAsync(id);

            if (table == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "Table not found"
                });
            }
            _unitOfWork.TableRepository.Remove(table);
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

    [HttpGet("status-list")]
    public async Task<ActionResult<IEnumerable<string>>> GetTablesStatus()
    {
        var tableEnums = (TableEnum[])Enum.GetValues(typeof(TableEnum));
        var statusList = new List<string>();
        //var count = 0;

        foreach (TableEnum tableEnum in tableEnums)
        {
            //statusList.Add($"{tableEnum}={count++}");
            statusList.Add($"{tableEnum}");
        }

        return Ok(new ApiResponse
        {
            Success = true,
            Data = statusList
        });
    }
}
