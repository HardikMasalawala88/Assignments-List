using lab3.Models;
using lab3.Models.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab3.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Registration()
        {
            UserCredential user = new UserCredential();
            return View(user);
        }

        [HttpPost]
        public IActionResult Registration(UserCredential userRegistration)
        {
            if (ModelState.IsValid)
            {
                UserCredentialsStores.addUser(userRegistration);
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            UserCredential userLogin = new UserCredential();
            return View(userLogin);
        }

        [HttpPost]
        public IActionResult Login(UserCredential user)
        {
            if (user.Username == null && user.Password != null)
            {
                TempData["errorMsg"] = "UserName Or Password Is Not Matched..!";
                return View();
            }
            else
            {
                //var loggedInUser = _context.Users.Where(x => x.UserName == user.Username && x.Password == user.Password)
                //                                 .FirstOrDefault();
                if (user.Username != null)
                {
                    HttpContext.Session.SetString("UserName", user.Username);
                }
                else
                {
                    TempData["errorMsg"] = "UserName Or Password Is Not Matched..!";
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            ViewBag.Message = "User logged out successfully!";
            return RedirectToAction("Login", "Account");
        }
    }
}
