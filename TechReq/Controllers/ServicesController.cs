using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeachRed.Domain.Enum;
using TeachRed.Service.Interfaces;

namespace TechReq.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> ListOfServices()
        {
            var response = await _serviceService.GetAllServices();

            // ИСПРАВЛЕНО: .Ok вместо .OK
            if (response.StatusCode == TeachRed.Domain.Enum.StatusCode.Ok)
            {
                return View(response.Data);
            }

            return RedirectToAction("Error", "Home");
        }
    }
}