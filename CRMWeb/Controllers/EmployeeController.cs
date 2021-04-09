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
    public class EmployeeController : Controller
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IPositionRepository positionRepository,IDepartmentRepository departmentRepository,IEmployeeRepository employeeRepository)
        {
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            return View(new Employee() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Department> dptList = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath, HttpContext.Session.GetString("JWToken"));
            IEnumerable<Position> positionList = await _positionRepository.GetAllAsync(SD.PositionAPIPath, HttpContext.Session.GetString("JWToken"));
            EmployeeVM objVM = new EmployeeVM()
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

                Employee = new Employee()
            };
                     

            if (id == null)
            {
                //This will be true for Insert / create
                return View(objVM);
            }

            //flow come here for update
            objVM.Employee = await _employeeRepository.GetAsync(SD.EmployeeAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (objVM.Employee == null)
            {
                return NotFound();
            }

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(EmployeeVM obj)
        {
            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    obj.Employee.Image = p1;
                }
                else
                {
                    var objFromDb = await _employeeRepository.GetAsync(SD.EmployeeAPIPath, obj.Employee.Id, HttpContext.Session.GetString("JWToken"));
                    obj.Employee.Image = objFromDb.Image;
                }

                if (obj.Employee.Id == 0)
                {
                    await _employeeRepository.CreateAsync(SD.EmployeeAPIPath, obj.Employee, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _employeeRepository.UpdateAsync(SD.EmployeeAPIPath+obj.Employee.Id, obj.Employee, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<Department> dptList = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath, HttpContext.Session.GetString("JWToken"));
                IEnumerable<Position> positionList = await _positionRepository.GetAllAsync(SD.PositionAPIPath, HttpContext.Session.GetString("JWToken"));
                EmployeeVM objVM = new EmployeeVM()
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

                    Employee = new Employee()
                };

                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllEmployee()
        {
            return Json(new { data = await _employeeRepository.GetAllAsync(SD.EmployeeAPIPath, HttpContext.Session.GetString("JWToken")) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _employeeRepository.DeleteAsync(SD.EmployeeAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true,message="Delete Successful"});
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
