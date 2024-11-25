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
        public IActionResult SignUp(string name, string email,string password, DateTime dayOfBirthday)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.ErrorMessage = "Name cannot be empty.";
                return View("~/Views/HomePage/Authentication.cshtml");
            }
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
                DayOfBirthday = dayOfBirthday,
                Password = password
            };
            _context.Users.Add(user); 
            _context.SaveChanges(); 

            return RedirectToAction("MainPage", "Videos");
        }
        public IActionResult SignIn(string email, string password)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser == null)
            {
                ViewBag.ErrorMessage = "User with this email does not exist.";
                return View("~/Views/HomePage/Authentication.cshtml");
            }
            if (existingUser.Password != password)
            {
                ViewBag.ErrorMessage = "Incorrect password. Please try again.";
                return View("~/Views/HomePage/Authentication.cshtml");
            }

            return RedirectToAction("MainPage", "Videos");
        }
    }
}
