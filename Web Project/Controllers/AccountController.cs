using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Project.Database;
using Web_Project.Models;

namespace Web_Project.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account


        public ActionResult Login(Account login)
        {
            AccountDB accountDB = new AccountDB();
            Account account = accountDB.FindByUser(login);
            if (account.ID != null)
            {
                Session["Username"] = account.name;
                Session["Surname"] = account.surname;
                Session["ID"] = account.ID;
                Session["Mail"] = account.mail;
                Session["Password"] = account.password;

                return View("~/Views/Home/Index.cshtml");
            }
            ViewBag.ErrorMessage = "Incorrect Credentials!";
            return View("Login");
        }


        public ActionResult RegisterUser(Account account)
        {
            AccountDB accountDB = new AccountDB();
            if (accountDB.AddUser(account))
                return View("Login");
            return View("Register");
        }
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Profile()
        {
            if (Session["ID"] == null)
                return View("Login");
            return View("Profile");
        }

        public ActionResult Settings()
        {
            Account account = new Account();
            account.ID = Session["ID"].ToString();
            account.name = Session["Username"].ToString();
            account.surname = Session["Surname"].ToString();
            account.mail = Session["Mail"].ToString();
            account.password = Session["Password"].ToString();
            return View("Settings",account);
        }
        public ActionResult UpdateSettings(Account account)
        {
            AccountDB accountDB = new AccountDB();
            account.ID = Session["ID"].ToString();
            accountDB.update(account);
            Session["Username"] = account.name;
            Session["Surname"] = account.surname;
            Session["Mail"] = account.mail;
            Session["Password"] = account.password;
            return View("Profile");
        }

        public ActionResult Logout()
        {
            Session["Username"] = null;
            Session["Surname"] = null;
            Session["ID"] = null;
            Session["Mail"] = null;
            Session["Password"] = null;
            return View("~/Views/Home/Index.cshtml");
        }
    }
}