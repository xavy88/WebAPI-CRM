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
    public class AccountController : Controller
    {
        private readonly IClientRepository _accountRepository;

        public AccountController(IClientRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public IActionResult Index()
        {
            return View(new Account() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Account obj = new Account();
            if (id == null)
            {
                //This will be true for Insert / create
                return View(obj);
            }

            //flow come here for update
            obj = await _accountRepository.GetAsync(SD.ClientAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Account obj)
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
                    obj.Image = p1;
                }
                else
                {
                    var objFromDb = await _accountRepository.GetAsync(SD.ClientAPIPath, obj.Id, HttpContext.Session.GetString("JWToken"));
                    obj.Image = objFromDb.Image;
                }
                if (obj.Id == 0)
                {
                    await _accountRepository.CreateAsync(SD.ClientAPIPath, obj, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _accountRepository.UpdateAsync(SD.ClientAPIPath + obj.Id, obj, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }

        }

        public async Task<IActionResult> GetAllAccount()
        {
            return Json(new { data = await _accountRepository.GetAllAsync(SD.ClientAPIPath, HttpContext.Session.GetString("JWToken")) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _accountRepository.DeleteAsync(SD.ClientAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
