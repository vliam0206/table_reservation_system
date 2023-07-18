using AutoMapper;
using Infrastructures.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Models.AccountModels;
using Domain.Entities;
using Application.Utils;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccountsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET api/Accounts
        [HttpGet]        
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = (await _unitOfWork.AccountRepository.GetAllAsync());
            return Ok(new ApiResponse
            {
                Success = true,
                Data = _mapper.Map<List<AccountViewModel>>(accounts)
            });
        }

        // GET api/Accounts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAccount(Guid id)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);

            if (account == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "Account not found"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Data = _mapper.Map<AccountViewModel>(account)
            });
        }

        // POST api/Accounts
        [HttpPost]
        public async Task<ActionResult> CreateAccount(AccountModel model)
        {
            try
            {
                var acc = (await _unitOfWork.AccountRepository.GetAllAsync())
                            .Where(x => x.UserName.Equals(model.UserName)).ToList();
                if (!acc.IsNullOrEmpty())
                {
                    return BadRequest(new ApiResponse()
                    {
                        Success = false,
                        ErrorMessage = "Username is dupplicated!"
                    });
                }
                var account = _mapper.Map<Account>(model);
                account.Password = model.Password.Hash();

                _unitOfWork.AccountRepository.Add(account);
                await _unitOfWork.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, new ApiResponse
                {
                    Success = true,
                    Data = account
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

        // DELETE api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            try
            {
                var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);

                if (account == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Account not found"
                    });
                }
                _unitOfWork.AccountRepository.Remove(account);
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
}
