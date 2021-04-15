using CRMWeb.Models;
using CRMWeb.Models.ViewModel;
using CRMWeb.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Controllers
{
    public class TaskController : Controller
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ITaskRepository _taskRepository;

        public TaskController(IPositionRepository positionRepository,IDepartmentRepository departmentRepository,ITaskRepository taskRepository)
        {
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
            _taskRepository = taskRepository;
        }
        public IActionResult Index()
        {
            return View(new Models.Task() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Department> dptList = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath, HttpContext.Session.GetString("JWToken"));
            IEnumerable<Position> positionList = await _positionRepository.GetAllAsync(SD.PositionAPIPath, HttpContext.Session.GetString("JWToken"));
            TaskVM objVM = new TaskVM()
            {

                DepartmentList = dptList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                PositionList = positionList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                Task = new Models.Task()
            };
                     

            if (id == null)
            {
                //This will be true for Insert / create
                return View(objVM);
            }

            //flow come here for update
            objVM.Task = await _taskRepository.GetAsync(SD.TaskAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (objVM.Task == null)
            {
                return NotFound();
            }

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TaskVM obj)
        {
            if (ModelState.IsValid)
            {

                if (obj.Task.Id == 0)
                {
                    await _taskRepository.CreateAsync(SD.TaskAPIPath, obj.Task, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _taskRepository.UpdateAsync(SD.TaskAPIPath+obj.Task.Id, obj.Task, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<Department> dptList = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath, HttpContext.Session.GetString("JWToken"));
                IEnumerable<Position> positionList = await _positionRepository.GetAllAsync(SD.PositionAPIPath, HttpContext.Session.GetString("JWToken"));
                TaskVM objVM = new TaskVM()
                {

                    DepartmentList = dptList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),

                    PositionList = positionList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),

                    Task = new Models.Task()
                };

                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllTask()
        {
            return Json(new { data = await _taskRepository.GetAllAsync(SD.TaskAPIPath, HttpContext.Session.GetString("JWToken")) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _taskRepository.DeleteAsync(SD.TaskAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true,message="Delete Successful"});
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
