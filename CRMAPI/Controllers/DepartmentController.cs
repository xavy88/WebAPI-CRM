using AutoMapper;
using CRMAPI.Models;
using CRMAPI.Models.Dtos;
using CRMAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "CRMOpenAPISpecDepartments")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentrepo, IMapper mapper)
        {
            _departmentRepo = departmentrepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of departments.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<DepartmentDto>))]
        public IActionResult GetDepartments()
        {
            var objList = _departmentRepo.GetDepartments();
            var objDto = new List<DepartmentDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<DepartmentDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual department.
        /// </summary>
        /// <param name="departmentId">The Id of department</param>
        /// <returns></returns>

        [HttpGet("{departmentId:int}", Name ="GetDepartment")]
        [ProducesResponseType(200, Type = typeof(DepartmentDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetDepartment(int departmentId)
        {
            var obj = _departmentRepo.GetDepartment(departmentId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<DepartmentDto>(obj);
            return Ok(objDto);

        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(DepartmentDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateDepartment([FromBody] DepartmentDto departmentDto)
        {
            if (departmentDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_departmentRepo.DepartmentExists(departmentDto.Name))
            {
                ModelState.AddModelError("", "Deparment already exists!");
                return StatusCode(404, ModelState);
            }

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            var departmentObj = _mapper.Map<Department>(departmentDto);

            if (!_departmentRepo.CreateDepartment(departmentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {departmentObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetDepartment", new { departmentId=departmentObj.Id},departmentObj);
        }
        [HttpPatch("{departmentId:int}", Name = "UpdateDepartment")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateDepartment(int departmentId, [FromBody] DepartmentDto departmentDto)
        {
            if (departmentDto == null || departmentId !=departmentDto.Id )
            {
                return BadRequest(ModelState);
            }
            var departmentObj = _mapper.Map<Department>(departmentDto);
           
            if (!_departmentRepo.UpdateDepartment(departmentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {departmentObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        //[HttpPatch("{departmentId:int}", Name = "InactiveDepartment")]
        //public IActionResult InactiveDepartment(int departmentId, [FromBody] DepartmentDto departmentDto)
        //{
        //    if (departmentDto == null || departmentId != departmentDto.Id)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var departmentObj = _mapper.Map<Department>(departmentDto);
        //    departmentObj.IsActive = false;

        //    if (!_departmentRepo.InactiveDepartment(departmentObj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong when inactiving the record {departmentObj.Name}");
        //        return StatusCode(500, ModelState);
        //    }

        //    return NoContent();

        //}


        //[HttpPatch("{departmentId:int}", Name = "ActiveDepartment")]
        //public IActionResult ActiveDepartment(int departmentId, [FromBody] DepartmentDto departmentDto)
        //{
        //    if (departmentDto == null || departmentId != departmentDto.Id)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var departmentObj = _mapper.Map<Department>(departmentDto);
        //    departmentObj.IsActive = false;

        //    if (!_departmentRepo.ActiveDepartment(departmentObj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong when activing the record {departmentObj.Name}");
        //        return StatusCode(500, ModelState);
        //    }

        //    return NoContent();

        //}

        [HttpDelete("{departmentId:int}", Name = "DeleteDepartment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteDepartment(int departmentId)
        {
            if (!_departmentRepo.DepartmentExists(departmentId))
            {
                return NotFound();
            }
            var departmentObj = _departmentRepo.GetDepartment(departmentId);

            if (!_departmentRepo.DeleteDepartment(departmentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {departmentObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
