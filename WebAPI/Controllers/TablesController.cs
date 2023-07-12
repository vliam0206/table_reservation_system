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
    [Authorize]
    public async Task<ActionResult> GetTables()
    {
        var tables = (await _unitOfWork.TableRepository.GetAllAsync())
                        .OrderBy(x => x.Code);        
        return Ok(new ApiResponse
        {
            Success = true,
            Data = _mapper.Map<IEnumerable<TableViewModel>>(tables)
        });
    }

    [HttpGet("{id}")]
    [Authorize]
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
    [Authorize]
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
    [Authorize]
    public async Task<IActionResult> UpdateTable(Guid id, TableUpdateModel updatedTable)
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

            _unitOfWork.TableRepository.Update(table);
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
    public ActionResult GetTablesStatus()
    {
        var statusList = Enum.GetNames(typeof(TableEnum)).ToList();
        return Ok(statusList);
    }

    [HttpPost("find")]    
    public async Task<ActionResult> FindTables(FindTableModel model)
    {
        try
        {
            List<TableViewModel> result = new List<TableViewModel>();
            var tableList = await _unitOfWork.TableRepository.FindSuitableTablesAsync(model.dateTimeBooking);
            var suitableTables = tableList.Where(x => x.SeatQuantity >= model.quantitySeats);
            if (suitableTables.Any())
            {
                var table = suitableTables.ToArray()[0];
                result.Add(_mapper.Map<TableViewModel>(table));
            }
            else
            {
                var tables = tableList.Reverse().ToArray();
                var totalSeats = 0;
                var index = 0;
                List<TableViewModel> tmp = new List<TableViewModel>();
                while (totalSeats < model.quantitySeats && index < tables.Length)
                {
                    totalSeats += tables[index].SeatQuantity;
                    tmp.Add(_mapper.Map<TableViewModel>(tables[index]));
                    index++;
                }
                if (totalSeats >= model.quantitySeats)
                {
                    result = tmp;
                }
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Data = result
            });
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
    [HttpPut("lock-table")]
    public async Task<IActionResult> LockTables([FromBody] List<Guid> tablesId)
    {
        try
        {
            foreach (var id in tablesId)
            {
                var table = await _unitOfWork.TableRepository.GetByIdAsync(id);
                if (table == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = $"Table with Id {id} not found."
                    });
                }
                table.Status = TableEnum.Booking;
                _unitOfWork.TableRepository.Update(table);
            }
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
    [HttpPut("unlock-table")]
    public async Task<IActionResult> UnLockTables([FromBody] List<Guid> tablesId)
    {
        try
        {
            foreach (var id in tablesId)
            {
                var table = await _unitOfWork.TableRepository.GetByIdAsync(id);
                if (table == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = $"Table with Id {id} not found."
                    });
                }
                table.Status = TableEnum.Active;
                _unitOfWork.TableRepository.Update(table);
            }
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
