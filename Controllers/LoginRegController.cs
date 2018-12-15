using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using thebeltexam.Models;
using System.Linq;

namespace thebeltexam.Controllers
{
    public class LoginRegController: Controller
    {
        private MyContext dbContext;
        public LoginRegController(MyContext context)
        {
            dbContext = context;
        }



        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        


        [HttpGet]
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }



        [HttpPost]
        [Route("register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserID", user.UserId);
                HttpContext.Session.SetString("UserFirstName", user.FirstName);
                return RedirectToAction("Home", "Activity");
            }
            return View("Index"); 
        }



        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                
                if(result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Incorrect Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserID", userInDb.UserId);
                HttpContext.Session.SetString("UserFirstName", userInDb.FirstName);
                return RedirectToAction("Home", "Activity");
            }
            return View("Index");
        }
    }
}