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
    [Route("api/v{version:apiVersion}/departments")]
    [ApiVersion("2.0")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "CRMOpenAPISpecDepartments")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class DepartmentV2Controller : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IMapper _mapper;

        public DepartmentV2Controller(IDepartmentRepository departmentrepo, IMapper mapper)
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
            var obj = _departmentRepo.GetDepartments().FirstOrDefault();
            
            return Ok(_mapper.Map<DepartmentDto>(obj));
        }

    }
}
