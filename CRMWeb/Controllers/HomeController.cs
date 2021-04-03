using CRMWeb.Models;
using CRMWeb.Models.ViewModel;
using CRMWeb.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDepartmentRepository _dptRepo;
        private readonly IAccountRepository _accRepo;
        private readonly IPositionRepository _positionRepo;

        public HomeController(ILogger<HomeController> logger, IDepartmentRepository dptRepo, IPositionRepository positionRepo, IAccountRepository accRepo)
        {
            _logger = logger;
            _dptRepo = dptRepo;
            _positionRepo = positionRepo;
            _accRepo = accRepo;

        }

        public async Task<IActionResult> Index()
        {
            IndexVM listOfDepartmentAndPosition = new IndexVM()
            {
                DepartmentList = await _dptRepo.GetAllAsync(SD.DepartmentAPIPath),
                PositionList = await _positionRepo.GetAllAsync (SD.PositionAPIPath),
            };
            return View(listOfDepartmentAndPosition);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            User objUser = await _accRepo.LoginAsync(SD.AccountAPIPath+"authenticate/", obj);
            if (objUser== null)
            {
                return View();
            }

            HttpContext.Session.SetString("JWToken", objUser.Token);
            return RedirectToAction("~/Home/Index");
        }

    }
}
