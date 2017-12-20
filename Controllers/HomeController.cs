using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cSharpRetakeExam.Factory;
using cSharpRetakeExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace cSharpRetakeExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserFactory _userfactory;
        private readonly FriendFactory _friendfactory;
    
        public HomeController(UserFactory xconnect, FriendFactory fconnect)
        {
            _userfactory = xconnect;
            _friendfactory = fconnect;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return RedirectToAction("Main");
        }

        [HttpGet]
        [Route("main")]
        public IActionResult Main()
        {
            return View("Index");
        }

        [HttpGet]
        [Route("professional_profile")]
        public IActionResult Homepage()
        {
            var x = (int)HttpContext.Session.GetInt32("CurrentId");
            System.Console.WriteLine(x);
            ViewBag.LoggedUser = _userfactory.GetUserById(x);
            ViewBag.Friends = _friendfactory.ShowAllFriends(x);
            ViewBag.Invitations = _friendfactory.Invitations(x);
            return View();
        }

        [HttpGet]
        [Route("users")]
        public IActionResult AllUsers()
        {
            var x = (int)HttpContext.Session.GetInt32("CurrentId");
            ViewBag.AllUsers = _userfactory.ShowAllUsers(x);
            return View();
        }

        [HttpGet]
        [Route("users/{Name}")]
        public IActionResult ShowUser(string Name)
        {
            ViewBag.SpecUser = _userfactory.GetUserByName(Name);
            return View();
        }

        [HttpGet]
        [Route("ignore/{Name}")]
        public IActionResult IgnoreUser(string Name)
        {
            _userfactory.Ignore(Name);
            return RedirectToAction("Homepage");
        }

        [HttpGet]
        [Route("request/{Id}")]
        public IActionResult RequestInv(int Id)
        {
            int x = (int)HttpContext.Session.GetInt32("CurrentId");
            var user = _userfactory.GetUserById(Id);
            var user2 = _userfactory.GetUserById(x);
            foreach(var e in user2)
            {
                var name = e.Name;
                var des = e.Description;
                foreach (var p in user)
                {
                    _userfactory.RequestConnection(Id, x, name, des);
                }
            }
            return RedirectToAction("Homepage");
        }

        [HttpGet]
        [Route("acceptRequest/{Name}")]
        public IActionResult RequestAcpt(string Name)
        {
            int x = (int)HttpContext.Session.GetInt32("CurrentId");
            _friendfactory.AddFriend(x, Name);
            return RedirectToAction("Homepage");
        }

        [HttpPost]
        [Route("logging")]
        public IActionResult LogIn(string Email, string PasswordToCheck, User T)
        {
            var user = _userfactory.GetUserByEmail(Email);
            if (user != null && PasswordToCheck != null)
            {
                foreach(var x in user)
                {
                    var Hasher = new PasswordHasher<User>();
                    // Pass the user object, the hashed password, and the PasswordToCheck
                    if (0 != Hasher.VerifyHashedPassword(x, x.Password, PasswordToCheck))
                    {
                        HttpContext.Session.SetInt32("CurrentId", x.Id);
                        return RedirectToAction("Homepage");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("addUser")]
        public IActionResult NewUser(User T, string passConf)
        {
            if (ModelState.IsValid)
            {
                if (T.Password != passConf)
                {
                    return RedirectToAction("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                T.Password = Hasher.HashPassword(T, T.Password);
                string pass = T.Password;
                System.Console.WriteLine(pass);
                _userfactory.AddUser(T, pass);
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpGet]
        [Route("logoff")]
        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
