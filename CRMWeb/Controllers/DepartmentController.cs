using CRMWeb.Models;
using CRMWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Controllers
{
    //[Authorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public IActionResult Index()
        {
            return View(new Department() { });
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            Department obj = new Department();
            if (id == null)
            {
                //This will be true for Insert / create
                return View(obj);
            }

            //flow come here for update
            obj = await _departmentRepository.GetAsync(SD.DepartmentAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Department obj)
        {
            if (ModelState.IsValid)
            {
                  
                if (obj.Id == 0)
                {
                    await _departmentRepository.CreateAsync(SD.DepartmentAPIPath, obj, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _departmentRepository.UpdateAsync(SD.DepartmentAPIPath+obj.Id, obj, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllDepartment()
        {
            return Json(new { data = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath, HttpContext.Session.GetString("JWToken")) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _departmentRepository.DeleteAsync(SD.DepartmentAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true,message="Delete Successful"});
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
