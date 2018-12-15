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
  public class ActivityController: Controller
    {
        private MyContext dbContext;
        public ActivityController(MyContext context)
        {
            dbContext = context;
        }



        [HttpGet]
        [Route("home")]
        public IActionResult Home()
        {
            if(!IsUserLoggedIn())
            {
                return RedirectToAction("Index", "LoginReg");
            }
            User CurrentUser = setCurrentUser();
            ViewBag.UserFirstName = CurrentUser.FirstName;
            ViewBag.UserId = CurrentUser.UserId;
            List<Activity> AllActivities = dbContext.Activities
                .Where(a => a.Date >= DateTime.Now)
                .Include(a => a.Participants)
                .Include(a => a.Coordinator)
                .OrderBy(a => a.Date)
                .ToList();
            return View(AllActivities);
        }



        [HttpGet]
        [Route("create")]
        public IActionResult NewActivity()
        {
            if(!IsUserLoggedIn())
            {
                return RedirectToAction("Index", "LoginReg");
            }
            User CurrentUser = setCurrentUser();
            ViewBag.UserFirstName = CurrentUser.FirstName;
            ViewBag.UserId = CurrentUser.UserId;
            return View();
        }



        [HttpPost]
        [Route("submit")]
        public IActionResult CreateActivity(Activity newdata)
        {
            if(!IsUserLoggedIn())
            {
                return RedirectToAction("Index", "LoginReg");
            }
            User CurrentUser = setCurrentUser();

            if (ModelState.IsValid)
            {
                Activity NewActivity = new Activity
                {
                    Title = newdata.Title,
                    Description = newdata.Description,
                    CoordinatorId = CurrentUser.UserId,
                    Duration = newdata.Duration,
                    DurationType = newdata.DurationType,
                    Date = newdata.Date.Date,
                    Time = newdata.Time,
                };
                dbContext.Activities.Add(NewActivity);
                dbContext.SaveChanges();
                return RedirectToAction("Home", "Activity");
            }
            ViewBag.UserFirstName = CurrentUser.FirstName;
            ViewBag.UserId = CurrentUser.UserId;
            return View("NewActivity"); 
        }



        [HttpGet]
        [Route("activity/{id}")]
        public IActionResult ActivityDetails(int id)
        {
            if(!IsUserLoggedIn())
            {
                return RedirectToAction("Index", "LoginReg");
            }
            User CurrentUser = setCurrentUser();
            ViewBag.UserFirstName = CurrentUser.FirstName;
            ViewBag.UserId = CurrentUser.UserId;
            Activity ActivityData =dbContext.Activities
                .Include(a => a.Coordinator)
                .Include(a => a.Participants)
                .ThenInclude(p => p.User)
                .SingleOrDefault(a => a.ActivityId == id);
            return View(ActivityData);
        }



        [HttpGet]
        [Route("join/{id}")]
        public IActionResult Join(int id)
        {
            if(!IsUserLoggedIn())
            {
                return RedirectToAction("Index", "LoginReg");
            }
            User CurrentUser = setCurrentUser();
            ViewBag.UserFirstName = CurrentUser.FirstName;
            ViewBag.UserId = CurrentUser.UserId;

            Activity ActivityData =dbContext.Activities
                .Include(a => a.Participants)
                .ThenInclude(p => p.User)
                .SingleOrDefault(a => a.ActivityId == id);
            
            Participant ThisParticipant = dbContext.Participants.SingleOrDefault(p => p.UserId == CurrentUser.UserId 
                && p.ActivityId == id);
            if (ThisParticipant != null)
            {
                ActivityData.Participants.Remove(ThisParticipant);
            }
            else
            {
                Participant NewParticipant = new Participant 
                {
                    UserId = CurrentUser.UserId,
                    ActivityId = ActivityData.ActivityId,
                };
                ActivityData.Participants.Add(NewParticipant);
            }
            dbContext.SaveChanges();
            return RedirectToAction("Home", "Activity");
        }



        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if(!IsUserLoggedIn())
            {
                return RedirectToAction("Index", "LoginReg");
            }
            User CurrentUser = setCurrentUser();
            ViewBag.UserFirstName = CurrentUser.FirstName;
            ViewBag.UserId = CurrentUser.UserId;
            Activity DeleteData =dbContext.Activities.SingleOrDefault(a => a.ActivityId == id);
            dbContext.Activities.Remove(DeleteData);
            dbContext.SaveChanges();
            return RedirectToAction("Home", "Activity");
        }

        public bool IsUserLoggedIn()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return false;
            }
            return true;
        }
        public User setCurrentUser()
        {
            User CurrentUser = dbContext.Users
                .SingleOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserID"));
            return CurrentUser;
        }


    }
}