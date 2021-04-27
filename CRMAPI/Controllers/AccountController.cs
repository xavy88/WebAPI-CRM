using AutoMapper;
using CRMAPI.Models;
using CRMAPI.Models.Dtos;
using CRMAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Controllers
{
    [Route("api/v{version:apiVersion}/accounts")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "CRMOpenAPISpecDepartments")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;

        public AccountController(IAccountRepository accountrepo, IMapper mapper)
        {
            _accountRepo = accountrepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of Account.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<AccountDto>))]
        public IActionResult GetAccounts()
        {
            var objList = _accountRepo.GetAccounts();
            var objDto = new List<AccountDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<AccountDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual account.
        /// </summary>
        /// <param name="accountId">The Id of account</param>
        /// <returns></returns>

        [HttpGet("{accountId:int}", Name ="GetAccount")]
        [ProducesResponseType(200, Type = typeof(AccountDto))]
        [ProducesResponseType(404)]
        [Authorize]
        [ProducesDefaultResponseType]
        public IActionResult GetAccount(int accountId)
        {
            var obj = _accountRepo.GetAccount(accountId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<AccountDto>(obj);
            return Ok(objDto);

        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(DepartmentDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateAccount([FromBody] AccountDto accountDto)
        {
            if (accountDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_accountRepo.AccountExists(accountDto.Name))
            {
                ModelState.AddModelError("", "Account already exists!");
                return StatusCode(404, ModelState);
            }

            var accountObj = _mapper.Map<Account>(accountDto);

            if (!_accountRepo.CreateAccount(accountObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {accountObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetAccount", new { version = HttpContext.GetRequestedApiVersion().ToString(),
                accountId=accountObj.Id},accountObj);
        }
        [HttpPatch("{accountId:int}", Name = "UpdateAccount")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateAccount(int accountId, [FromBody] AccountDto accountDto)
        {
            if (accountDto == null || accountId !=accountDto.Id )
            {
                return BadRequest(ModelState);
            }
            var accountObj = _mapper.Map<Account>(accountDto);
           
            if (!_accountRepo.UpdateAccount(accountObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {accountObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpPatch("[action]{accountId:int}", Name = "InactiveAccount")]
        public IActionResult InactiveAccount(int accountId, [FromBody] AccountDto accountDto)
        {
            if (accountDto == null || accountId != accountDto.Id)
            {
                return BadRequest(ModelState);
            }
            var accountObj = _mapper.Map<Account>(accountDto);
            accountObj.IsActive = false;

            if (!_accountRepo.InactiveAccount(accountObj))
            {
                ModelState.AddModelError("", $"Something went wrong when inactiving the record {accountObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpPatch("[action]/{accountId:int}", Name = "ActiveAccount")]
        public IActionResult ActiveAccount(int accountId, [FromBody] AccountDto accountDto)
        {
            if (accountDto == null || accountId != accountDto.Id)
            {
                return BadRequest(ModelState);
            }
            var accountObj = _mapper.Map<Account>(accountDto);
            accountObj.IsActive = true;

            if (!_accountRepo.ActiveAccount(accountObj))
            {
                ModelState.AddModelError("", $"Something went wrong when activing the record {accountObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{accountId:int}", Name = "DeleteAccount")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteAccount(int accountId)
        {
            if (!_accountRepo.AccountExists(accountId))
            {
                return NotFound();
            }
            var accountObj = _accountRepo.GetAccount(accountId);

            if (!_accountRepo.DeleteAccount(accountObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {accountObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
