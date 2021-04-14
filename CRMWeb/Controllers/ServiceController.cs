using CRMWeb.Models;
using CRMWeb.Models.ViewModel;
using CRMWeb.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public ServiceController(IServiceRepository serviceRepository,IDepartmentRepository departmentRepository)
        {
            _serviceRepository = serviceRepository;
            _departmentRepository = departmentRepository;
        }
        public IActionResult Index()
        {
            return View(new Service() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Department> dptList = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath, HttpContext.Session.GetString("JWToken"));
            ServiceVM objVM = new ServiceVM()
            {

                DepartmentList = dptList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Service = new Service()
            };

            if (id == null)
            {
                //This will be true for Insert / create
                return View(objVM);
            }

            //flow come here for update
            objVM.Service = await _serviceRepository.GetAsync(SD.ServiceAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (objVM.Service == null)
            {
                return NotFound();
            }

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ServiceVM obj)
        {
            if (ModelState.IsValid)
            {
                  
                if (obj.Service.Id == 0)
                {
                    await _serviceRepository.CreateAsync(SD.ServiceAPIPath, obj.Service, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _serviceRepository.UpdateAsync(SD.ServiceAPIPath+obj.Service.Id, obj.Service, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<Department> dptList = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath, HttpContext.Session.GetString("JWToken"));
                ServiceVM objVM = new ServiceVM()
                {

                    DepartmentList = dptList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Service = obj.Service
                };

                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllService()
        {
            return Json(new { data = await _serviceRepository.GetAllAsync(SD.ServiceAPIPath, HttpContext.Session.GetString("JWToken")) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _serviceRepository.DeleteAsync(SD.ServiceAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true,message="Delete Successful"});
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
