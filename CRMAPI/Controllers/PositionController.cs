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
    [ApiExplorerSettings(GroupName = "CRMOpenAPISpecPositions")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class PositionController : ControllerBase
    {
        private readonly IPositionRepository _positionRepo;
        private readonly IMapper _mapper;

        public PositionController(IPositionRepository positionRepo, IMapper mapper)
        {
            _positionRepo = positionRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of positions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<PositionDto>))]
        public IActionResult GetPositions()
        {
            var objList = _positionRepo.GetPositions();
            var objDto = new List<PositionDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<PositionDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual position.
        /// </summary>
        /// <param name="positionId">The Id of position</param>
        /// <returns></returns>

        [HttpGet("{positionId:int}", Name ="GetPosition")]
        [ProducesResponseType(200, Type = typeof(PositionDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetPosition(int positionId)
        {
            var obj = _positionRepo.GetPosition(positionId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<PositionDto>(obj);
            return Ok(objDto);

        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PositionDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreatePosition([FromBody] PositionCreateDto positionDto)
        {
            if (positionDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_positionRepo.PositionExists(positionDto.Name))
            {
                ModelState.AddModelError("", "Position already exists!");
                return StatusCode(404, ModelState);
            }

            var positionObj = _mapper.Map<Position>(positionDto);

            if (!_positionRepo.CreatePosition(positionObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {positionObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPosition", new { positionId=positionObj.Id},positionObj);
        }
        [HttpPatch("{positionId:int}", Name = "UpdatePosition")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePosition(int positionId, [FromBody] PositionUpdateDto positionDto)
        {
            if (positionDto == null || positionId !=positionDto.Id )
            {
                return BadRequest(ModelState);
            }
            var positionObj = _mapper.Map<Position>(positionDto);
           
            if (!_positionRepo.UpdatePosition(positionObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {positionObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{positionId:int}", Name = "DeletePosition")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletePosition(int positionId)
        {
            if (!_positionRepo.PositionExists(positionId))
            {
                return NotFound();
            }
            var positionObj = _positionRepo.GetPosition(positionId);

            if (!_positionRepo.DeletePosition(positionObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {positionObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
