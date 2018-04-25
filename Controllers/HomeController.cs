using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private DashboardContext _context;

        public HomeController(DashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpGet]
        [Route("New")]
        public IActionResult New()
        {
            return View("New");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model)
        {
            User user = _context.Users.Where(e=>e.Email == model.Email).SingleOrDefault();
            if(user != null){
                ViewBag.Error ="Email already registered";
                return View("Login");
            }
            if (ModelState.IsValid)
            {
                User NewUser = new User
                {
                    Name = model.Name,
                    NickName = model.NickName,
                    Email = model.Email,
                    Password = model.Password
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);

                _context.Users.Add(NewUser);
                _context.SaveChanges();
                
                var loginUser = _context.Users.SingleOrDefault(User => User.Email == model.Email);
                HttpContext.Session.SetInt32("CurrentUserID", loginUser.UserID);
                return RedirectToAction("Dashboard", "Wall");
            }
            return View("Login");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("loggingIn")]
        public IActionResult LoggingIn(string Email, string loginpw)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();

            var loginUser = _context.Users.SingleOrDefault(User => User.Email == Email);
            if (loginUser != null)
            {
                var hashedPw = Hasher.VerifyHashedPassword(loginUser, loginUser.Password, loginpw);
                if (hashedPw == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("CurrentUserID", loginUser.UserID);
                    return RedirectToAction("Dashboard", "Wall");
                }
            }

            ViewBag.Error = "Email address or Password is not matching";
            return View("Login");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}