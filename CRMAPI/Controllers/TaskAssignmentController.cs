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
    [Route("api/v{version:apiVersion}/taskassignments")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "CRMOpenAPISpecPositions")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TaskAssignmentController : ControllerBase
    {
        private readonly ITaskAssignmentRepository _taRepo;
        private readonly IMapper _mapper;

        public TaskAssignmentController(ITaskAssignmentRepository taRepo, IMapper mapper)
        {
            _taRepo = taRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of positions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<TaskAssignmentDto>))]
        public IActionResult GetTaskAssignments()
        {
            var objList = _taRepo.GetTaskAssignments();
            var objDto = new List<TaskAssignmentDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TaskAssignmentDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual position.
        /// </summary>
        /// <param name="taskAssignmentId">The Id of Task Assignment</param>
        /// <returns></returns>

        [HttpGet("{taskAssignmentId:int}", Name ="GetTaskAssignment")]
        [ProducesResponseType(200, Type = typeof(TaskAssignmentDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles ="Admin")]
        public IActionResult GetTaskAssignment(int taskAssignmentId)
        {
            var obj = _taRepo.GetTaskAssignment(taskAssignmentId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<TaskAssignmentDto>(obj);
            return Ok(objDto);

        }

        [HttpGet("[action]{userId:int}")]
        [ProducesResponseType(200, Type = typeof(TaskAssignmentDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTaskAssignmentInUser(int userId)
        {
            var objList = _taRepo.GetTaskAssignmentInUser(userId);
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<TaskAssignmentDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TaskAssignmentDto>(obj));
            }

           
            return Ok(objDto);

        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TaskAssignmentUpsertDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTaskAssignment([FromBody] TaskAssignmentUpsertDto taskAssignmentDto)
        {
            if (taskAssignmentDto == null)
            {
                return BadRequest(ModelState);
            }

            //if (_taRepo.TaskAssignmentExists(taskAssignmentDto.AccountId))
            //{
            //    ModelState.AddModelError("", "Employee already exists!");
            //    return StatusCode(404, ModelState);
            //}

            var taskAssignmentObj = _mapper.Map<TaskAssignment>(taskAssignmentDto);

            if (!_taRepo.CreateTaskAssignment(taskAssignmentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {taskAssignmentObj.Account.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTaskAssignment", new { taskAssignmentId = taskAssignmentObj.Id}, taskAssignmentObj);
        }
        [HttpPatch("{taskAssignmentId:int}", Name = "UpdateTaskAssignment")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTaskAssignment(int taskAssignmentId, [FromBody] TaskAssignmentUpsertDto taskAssignmentDto)
        {
            if (taskAssignmentDto == null || taskAssignmentId != taskAssignmentDto.Id )
            {
                return BadRequest(ModelState);
            }
            var taskAssignmentObj = _mapper.Map<TaskAssignment>(taskAssignmentDto);
           
            if (!_taRepo.UpdateTaskAssignment(taskAssignmentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {taskAssignmentObj.Account.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{taskAssignmentId:int}", Name = "DeleteTaskAssignment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTaskAssignment(int taskAssignmentId)
        {
            if (!_taRepo.TaskAssignmentExists(taskAssignmentId))
            {
                return NotFound();
            }
            var taskAssignmentObj = _taRepo.GetTaskAssignment(taskAssignmentId);

            if (!_taRepo.DeleteTaskAssignment(taskAssignmentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {taskAssignmentObj.Account.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
