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
            var users = UserCredentialsStores.getUserList();
            foreach(var userItem in users)
            {
                if(userItem.Username == user.Username && userItem.Password == user.Password)
                {
                    return RedirectToAction("AddMovie", "Home");
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            ViewBag.Message = "User logged out successfully!";
            return RedirectToAction("Login", "Account");
        }
    }
}
