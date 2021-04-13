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
    [Route("api/v{version:apiVersion}/services")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "CRMOpenAPISpecPositions")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly IMapper _mapper;

        public ServiceController(IServiceRepository serviceRepo, IMapper mapper)
        {
            _serviceRepo = serviceRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of servicess.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<ServiceDto>))]
        public IActionResult GetService()
        {
            var objList = _serviceRepo.GetServices();
            var objDto = new List<ServiceDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<ServiceDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual service.
        /// </summary>
        /// <param name="serviceId">The Id of service</param>
        /// <returns></returns>

        [HttpGet("{serviceId:int}", Name ="GetService")]
        [ProducesResponseType(200, Type = typeof(ServiceDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles ="Admin")]
        public IActionResult GetService(int serviceId)
        {
            var obj = _serviceRepo.GetService(serviceId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<ServiceDto>(obj);
            return Ok(objDto);

        }

        [HttpGet("[action]{serviceId:int}")]
        [ProducesResponseType(200, Type = typeof(ServiceDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetServiceInDepartment(int serviceId)
        {
            var objList = _serviceRepo.GetServicesInDepartment(serviceId);
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<ServiceDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<ServiceDto>(obj));
            }

           
            return Ok(objDto);

        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PositionDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateService([FromBody] ServiceCreateDto serviceDto)
        {
            if (serviceDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_serviceRepo.ServiceExists(serviceDto.Name))
            {
                ModelState.AddModelError("", "Service already exists!");
                return StatusCode(404, ModelState);
            }

            var serviceObj = _mapper.Map<Service>(serviceDto);

            if (!_serviceRepo.CreateService(serviceObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {serviceObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetService", new { serviceId=serviceObj.Id},serviceObj);
        }
        [HttpPatch("{serviceId:int}", Name = "UpdateService")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateService(int serviceId, [FromBody] ServiceUpdateDto serviceDto)
        {
            if (serviceDto == null || serviceId !=serviceDto.Id )
            {
                return BadRequest(ModelState);
            }
            var serviceObj = _mapper.Map<Service>(serviceDto);
           
            if (!_serviceRepo.UpdateService(serviceObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {serviceObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{serviceId:int}", Name = "Deleteservice")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteService(int serviceId)
        {
            if (!_serviceRepo.ServiceExists(serviceId))
            {
                return NotFound();
            }
            var serviceObj = _serviceRepo.GetService(serviceId);

            if (!_serviceRepo.DeleteService(serviceObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {serviceObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
