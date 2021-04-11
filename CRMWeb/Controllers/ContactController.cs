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
    public class ContactController : Controller
    {
        private readonly IClientRepository _accountRepository;
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository,IClientRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _contactRepository = contactRepository;
            
        }
        public IActionResult Index()
        {
            return View(new Contact() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Account> accList = await _accountRepository.GetAllAsync(SD.ClientAPIPath, HttpContext.Session.GetString("JWToken"));
            ContactVM objVM = new ContactVM()
            {

                AccountList = accList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                Contact = new Contact()
            };
                     

            if (id == null)
            {
                //This will be true for Insert / create
                return View(objVM);
            }

            //flow come here for update
            objVM.Contact = await _contactRepository.GetAsync(SD.ContactAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (objVM.Contact == null)
            {
                return NotFound();
            }

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ContactVM obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Contact.Id == 0)
                {
                    await _contactRepository.CreateAsync(SD.ContactAPIPath, obj.Contact, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _contactRepository.UpdateAsync(SD.ContactAPIPath+obj.Contact.Id, obj.Contact, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<Account> accList = await _accountRepository.GetAllAsync(SD.ClientAPIPath, HttpContext.Session.GetString("JWToken"));
                ContactVM objVM = new ContactVM()
                {

                    AccountList = accList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),

                    Contact = new Contact()
                };

                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllContact()
        {
            return Json(new { data = await _contactRepository.GetAllAsync(SD.ContactAPIPath, HttpContext.Session.GetString("JWToken")) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _contactRepository.DeleteAsync(SD.ContactAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true,message="Delete Successful"});
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
