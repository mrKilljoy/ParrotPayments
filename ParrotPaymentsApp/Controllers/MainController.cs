using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParrotPaymentsApp.Controllers
{
    public class MainController : Controller
    {
        [Route("")]
        public ActionResult Main()
        {
            //using (Infrastructure.AuthManager mng = new Infrastructure.AuthManager())
            //{
            //    Models.AuthContext cnt = new Models.AuthContext();

            //    if (cnt.Users.Count() < 1)
            //    {
            //        mng.RegisterUser(new Models.UserModel
            //        {
            //            Email = "goga@mail.ru",
            //            Password = "Angu1ar",
            //            RepeatedPassword = "Angu1ar",
            //            Username = "Gregory"
            //        }).Wait();

            //        mng.RegisterUser(new Models.UserModel
            //        {
            //            Email = "max@mail.ru",
            //            Password = "Angu1ar",
            //            RepeatedPassword = "Angu1ar",
            //            Username = "Max"
            //        }).Wait();

            //        mng.RegisterUser(new Models.UserModel
            //        {
            //            Email = "frank@mail.ru",
            //            Password = "Angu1ar",
            //            RepeatedPassword = "Angu1ar",
            //            Username = "Frank"
            //        }).Wait();
            //    }
            //    cnt.Dispose();
            //}

            return View();
        }

        [Route("~/Login")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("~/Register")]
        public ActionResult Register()
        {
            return View();
        }
    }
}