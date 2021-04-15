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
    [Route("api/v{version:apiVersion}/tasks")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "CRMOpenAPISpecPositions")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IMapper _mapper;

        public TaskController(ITaskRepository taskRepo, IMapper mapper)
        {
            _taskRepo = taskRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of tasks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<TaskDto>))]
        public IActionResult GetTasks()
        {
            var objList = _taskRepo.GetTasks();
            var objDto = new List<TaskDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TaskDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual task.
        /// </summary>
        /// <param name="taskId">The Id of task</param>
        /// <returns></returns>

        [HttpGet("{taskId:int}", Name ="GetTask")]
        [ProducesResponseType(200, Type = typeof(TaskDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles ="Admin")]
        public IActionResult GetTask(int taskId)
        {
            var obj = _taskRepo.GetTask(taskId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<TaskDto>(obj);
            return Ok(objDto);

        }

        [HttpGet("[action]{departmentId:int}")]
        [ProducesResponseType(200, Type = typeof(TaskDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTasksInDepartment(int departmentId)
        {
            var objList = _taskRepo.GetTaskInDepartment(departmentId);
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<TaskDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TaskDto>(obj));
            }

           
            return Ok(objDto);

        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TaskDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTask([FromBody] TaskCreateDto taskDto)
        {
            if (taskDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_taskRepo.TaskExists(taskDto.Name))
            {
                ModelState.AddModelError("", "Task already exists!");
                return StatusCode(404, ModelState);
            }

            var taskObj = _mapper.Map<Models.Task>(taskDto);

            if (!_taskRepo.CreateTask(taskObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {taskObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTask", new { taskId=taskObj.Id},taskObj);
        }
        [HttpPatch("{taskId:int}", Name = "UpdateTask")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTask(int taskId, [FromBody] TaskUpdateDto taskDto)
        {
            if (taskDto == null || taskId !=taskDto.Id )
            {
                return BadRequest(ModelState);
            }
            var taskObj = _mapper.Map<Models.Task>(taskDto);
           
            if (!_taskRepo.UpdateTask(taskObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {taskObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{taskId:int}", Name = "DeleteTask")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTask(int taskId)
        {
            if (!_taskRepo.TaskExists(taskId))
            {
                return NotFound();
            }
            var taskObj = _taskRepo.GetTask(taskId);

            if (!_taskRepo.DeleteTask(taskObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {taskObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
