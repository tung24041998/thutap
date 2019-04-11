using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trainee.Models;

namespace Trainee.Controllers
{
    public class UsersController : Controller
    {
        db_Trainee db_Trainee = new db_Trainee();
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User user)
        {
            var test = db_Trainee.Users.FirstOrDefault(x => x.Username == user.Username && x.Passwork == user.Passwork);
            if(test != null)
            {
                Session["Username"] = test.Username;
                Session["Passwork"] = test.Passwork;
                Session["Right"] = test.right;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.err = "Tài khoản hoặc mật khẩu không chính xác!";
                return View();
            }

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.right = "User";
                var kt = db_Trainee.Users.FirstOrDefault(x => x.Username == user.Username);
                if (kt == null)
                {
                    db_Trainee.Users.Add(user);
                    db_Trainee.SaveChanges();
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    ViewBag.err = "Tài khoản đã tồn tại !";
                    return View();
                }
            }
            else
            {
                ViewBag.err = "Vui lòng điền đầy đủ ! ";
                return View();
            }

           
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}