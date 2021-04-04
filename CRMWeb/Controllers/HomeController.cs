using CRMWeb.Models;
using CRMWeb.Models.ViewModel;
using CRMWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
                DepartmentList = await _dptRepo.GetAllAsync(SD.DepartmentAPIPath, HttpContext.Session.GetString("JWToken")),
                PositionList = await _positionRepo.GetAllAsync (SD.PositionAPIPath, HttpContext.Session.GetString("JWToken")),
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

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, objUser.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, objUser.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            HttpContext.Session.SetString("JWToken", objUser.Token);
            TempData["alert"] = "Welcome " + objUser.UserName;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _accRepo.RegisterAsync(SD.AccountAPIPath + "register/", obj);
            if (result == false)
            {
                return View();
            }
            TempData["alert"] = "Registration Successful";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");
           return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
