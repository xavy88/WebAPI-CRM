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
    public class TaskAssignmentController : Controller
    {
        private readonly IEmployeeRepository _empRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IClientRepository _accountRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskAssignmentRepository _taRepository;

        public TaskAssignmentController(IEmployeeRepository empRepository,IDepartmentRepository departmentRepository,IClientRepository accountRepository, ITaskRepository taskRepository, ITaskAssignmentRepository taRepository)
        {
            _empRepository = empRepository;
            _departmentRepository = departmentRepository;
            _accountRepository = accountRepository;
            _taskRepository = taskRepository;
            _taRepository = taRepository;
        }
        public IActionResult Index()
        {
            return View(new TaskAssignment() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Department> dptList = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath, HttpContext.Session.GetString("JWToken"));
            IEnumerable<Models.Task> taskList = await _taskRepository.GetAllAsync(SD.TaskAPIPath, HttpContext.Session.GetString("JWToken"));
            IEnumerable<Employee> empList = await _empRepository.GetAllAsync(SD.EmployeeAPIPath, HttpContext.Session.GetString("JWToken"));
            IEnumerable<Account> accountList = await _accountRepository.GetAllAsync(SD.ClientAPIPath, HttpContext.Session.GetString("JWToken"));
            TaskAssignmentVM objVM = new TaskAssignmentVM()
            {

                DepartmentList = dptList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                TaskList = taskList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                EmployeeList = empList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                AccountList = accountList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                TaskAssignment = new TaskAssignment()
            };
                     

            if (id == null)
            {
                //This will be true for Insert / create
                return View(objVM);
            }

            //flow come here for update
            objVM.TaskAssignment = await _taRepository.GetAsync(SD.TaskAssignmentAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (objVM.TaskAssignment == null)
            {
                return NotFound();
            }

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TaskAssignmentVM obj)
        {
            if (ModelState.IsValid)
            {
                        
                if (obj.TaskAssignment.Id == 0)
                {
                    await _taRepository.CreateAsync(SD.TaskAssignmentAPIPath, obj.TaskAssignment, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _taRepository.UpdateAsync(SD.TaskAssignmentAPIPath+obj.TaskAssignment.Id, obj.TaskAssignment, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<Department> dptList = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath, HttpContext.Session.GetString("JWToken"));
                IEnumerable<Employee> empList = await _empRepository.GetAllAsync(SD.EmployeeAPIPath, HttpContext.Session.GetString("JWToken"));
                IEnumerable<Account> accountList = await _accountRepository.GetAllAsync(SD.ClientAPIPath, HttpContext.Session.GetString("JWToken"));
                IEnumerable<Models.Task> tasktList = await _taskRepository.GetAllAsync(SD.TaskAPIPath, HttpContext.Session.GetString("JWToken"));
                TaskAssignmentVM objVM = new TaskAssignmentVM()
                {

                    DepartmentList = dptList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),

                    EmployeeList = empList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),

                    TaskAssignment = new TaskAssignment()
                };

                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllTaskAssignment()
        {
            return Json(new { data = await _taRepository.GetAllAsync(SD.TaskAssignmentAPIPath, HttpContext.Session.GetString("JWToken")) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _taRepository.DeleteAsync(SD.TaskAssignmentAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true,message="Delete Successful"});
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
