using Microsoft.AspNetCore.Mvc;

namespace ApplicationFilmsAndSerials.Controllers
{
    public class MainPageController : Controller
    {
        public IActionResult MainPage()
        {
            return View("~/Views/HomePage/MainPage.cshtml");
        }
        
    } 
}
