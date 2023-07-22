using DashiNovel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DashiNovel.Controllers
{
    public class HomeController : Controller
    {
        private DashiNovelContext context = new DashiNovelContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Novel> novels = context.Novels.ToList();
            return View(novels);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.emailErr = TempData["emailErr"];
            ViewBag.passwordErr = TempData["passErr"];
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            User user = context.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                TempData["emailErr"] = "Email doesn't exist";
            }
            else
            {
                if (user.Password.Equals(password))
                {
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["passErr"] = "The password is not correct";
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.emailErr = TempData["emailErr"];
            ViewBag.passErr = TempData["passErr"];
            ViewBag.confPassErr = TempData["confPassErr"];
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user, string confpass)
        {
            bool isUnique = !context.Users.Any(u => u.Email == user.Email);
            if (isUnique == false)
            {
                TempData["emailErr"] = "Email already exists.";
            }
            string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()\-_=+{}[\]:;""'<>,.?/])(?=.*[a-z]).{8,}$";
            bool isValid = Regex.IsMatch(user.Password, pattern);
            if (isValid == false)
            {
                TempData["passErr"] = "Password must contain an uppercase letter, a digit, a special character, and be at least 8 characters long.";
            }
            bool isSame = user.Password.Equals(confpass);
            if (isSame == false)
            {
                TempData["confPassErr"] = "Passwords do not match.";
            }

            if (isUnique == true && isValid == true && isSame == true)
            {
                context.Users.Add(user);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            else
                return RedirectToAction("Register", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index");
        }
    }
}