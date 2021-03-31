using CRMWeb.Models;
using CRMWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Controllers
{
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

        public async Task<IActionResult> Upsert(int? id)
        {
            Department obj = new Department();
            if (id == null)
            {
                //This will be true for Insert / create
                return View(obj);
            }

            //flow come here for update
            obj = await _departmentRepository.GetAsync(SD.DepartmentAPIPath, id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        public async Task<IActionResult> GetAllDepartment()
        {
            return Json(new { data = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath) });
        }
    }
}
