using ApplicationFilmsAndSerials.Data;
using ApplicationFilmsAndSerials.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationFilmsAndSerials.Controllers
{
    public class SignInUpController : Controller
    {
        private readonly FilmsAndSerialsContext _context;

        public SignInUpController(FilmsAndSerialsContext context)
        {
            _context = context;
        }
        public IActionResult Authentication()
        {
            return View("~/Views/HomePage/Authentication.cshtml");
        }
        public IActionResult SignUp(string name, string email, DateTime dayOfBirthday)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "User with this email already exists.";
                return View("~/Views/HomePage/Authentication.cshtml");
            }
            var user = new User
            {
                Name = name,
                Email = email,
                DayOfBirthday = dayOfBirthday
            };
            _context.Users.Add(user); 
            _context.SaveChanges(); 

            return RedirectToAction("MainPage", "MainPage");
        }
    }
}
