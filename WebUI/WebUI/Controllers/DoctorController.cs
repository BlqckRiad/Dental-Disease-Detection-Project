using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
} 