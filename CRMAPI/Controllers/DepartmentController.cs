﻿using AutoMapper;
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
    }
}
