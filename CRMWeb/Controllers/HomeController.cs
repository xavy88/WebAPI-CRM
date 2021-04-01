using CRMWeb.Models;
using CRMWeb.Models.ViewModel;
using CRMWeb.Repository.IRepository;
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
        private readonly IPositionRepository _positionRepo;

        public HomeController(ILogger<HomeController> logger, IDepartmentRepository dptRepo, IPositionRepository positionRepo)
        {
            _logger = logger;
            _dptRepo = dptRepo;
            _positionRepo = positionRepo;

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
    }
}
