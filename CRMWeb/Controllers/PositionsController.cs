using CRMWeb.Models;
using CRMWeb.Models.ViewModel;
using CRMWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Controllers
{
    public class PositionsController : Controller
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public PositionsController(IPositionRepository positionRepository,IDepartmentRepository departmentRepository)
        {
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
        }
        public IActionResult Index()
        {
            return View(new Position() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Department> dptList = await _departmentRepository.GetAllAsync(SD.DepartmentAPIPath);
            PositionVM objVM = new PositionVM()
            {

                DepartmentList = dptList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //This will be true for Insert / create
                return View(objVM);
            }

            //flow come here for update
            objVM.Position = await _positionRepository.GetAsync(SD.PositionAPIPath, id.GetValueOrDefault());
            if (objVM.Position == null)
            {
                return NotFound();
            }

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(PositionVM obj)
        {
            if (ModelState.IsValid)
            {
                  
                if (obj.Position.Id == 0)
                {
                    await _positionRepository.CreateAsync(SD.PositionAPIPath, obj.Position);
                }
                else
                {
                    await _positionRepository.UpdateAsync(SD.PositionAPIPath+obj.Position.Id, obj.Position);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllPosition()
        {
            return Json(new { data = await _positionRepository.GetAllAsync(SD.PositionAPIPath) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _positionRepository.DeleteAsync(SD.PositionAPIPath, id);
            if (status)
            {
                return Json(new { success = true,message="Delete Successful"});
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
