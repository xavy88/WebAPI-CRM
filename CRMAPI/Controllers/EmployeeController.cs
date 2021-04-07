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
    [Route("api/v{version:apiVersion}/employees")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "CRMOpenAPISpecPositions")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _empRepo;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository empRepo, IMapper mapper)
        {
            _empRepo = empRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of positions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<EmployeeDto>))]
        public IActionResult GetEmployees()
        {
            var objList = _empRepo.GetEmployees();
            var objDto = new List<EmployeeDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<EmployeeDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual position.
        /// </summary>
        /// <param name="employeeId">The Id of position</param>
        /// <returns></returns>

        [HttpGet("{employeeId:int}", Name ="GetEmployee")]
        [ProducesResponseType(200, Type = typeof(EmployeeDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles ="Admin")]
        public IActionResult GetEmployee(int employeeId)
        {
            var obj = _empRepo.GetEmployee(employeeId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<EmployeeDto>(obj);
            return Ok(objDto);

        }

        [HttpGet("[action]{departmentId:int}")]
        [ProducesResponseType(200, Type = typeof(EmployeeDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetEmployeesInDepartment(int departmentId)
        {
            var objList = _empRepo.GetEmployeesInDepartment(departmentId);
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<EmployeeDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<EmployeeDto>(obj));
            }

           
            return Ok(objDto);

        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(EmployeeDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateEmployee([FromBody] EmployeeCreateDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_empRepo.EmployeeExists(employeeDto.Name))
            {
                ModelState.AddModelError("", "Employee already exists!");
                return StatusCode(404, ModelState);
            }

            var employeeObj = _mapper.Map<Employee>(employeeDto);

            if (!_empRepo.CreateEmployee(employeeObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {employeeObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetEmployee", new { employeeId=employeeObj.Id},employeeObj);
        }
        [HttpPatch("{employeeId:int}", Name = "UpdateEmployee")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateEmployee(int employeeId, [FromBody] EmployeeUpdateDto employeeDto)
        {
            if (employeeDto == null || employeeId !=employeeDto.Id )
            {
                return BadRequest(ModelState);
            }
            var employeeObj = _mapper.Map<Employee>(employeeDto);
           
            if (!_empRepo.UpdateEmployee(employeeObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {employeeObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{employeeId:int}", Name = "DeleteEmployee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteEmployee(int employeeId)
        {
            if (!_empRepo.EmployeeExists(employeeId))
            {
                return NotFound();
            }
            var employeeObj = _empRepo.GetEmployee(employeeId);

            if (!_empRepo.DeleteEmployee(employeeObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {employeeObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
