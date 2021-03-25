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
    public class DepartmentController : ControllerBase
    {
        private IDepartmentRepository _departmentRepo;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentrepo, IMapper mapper)
        {
            _departmentRepo = departmentrepo;
            _mapper = mapper;
        }

        [HttpGet]
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

        [HttpGet("{departmentId:int}", Name ="GetDepartment")]
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
