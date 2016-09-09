using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace esm.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            //MembershipUser user= Membership.CreateUser("Pavel", "matanbar");
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "MatanBar application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "MatanBar contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string username, string password)
        {
            SHA256Managed hash = new SHA256Managed();
            byte[] hashBytes=hash.ComputeHash(Encoding.UTF8.GetBytes(username + password));
            string hashStr = BitConverter.ToString(hashBytes).Replace("-","");
            System.IO.StreamReader file = new System.IO.StreamReader(Server.MapPath("~/Content/Users.txt"));
            string line;
            while((line=file.ReadLine())!=null)
            {
                string[] logins = line.Split(' ');
                if ( hashStr== logins[1])
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    System.IO.File.AppendAllText(Server.MapPath("~/Content/OnlineUsers.txt"), username + " " + Request.UserHostAddress);
                    return View("SendPage");
                }
            }
            return View("Index");
        }

        [Authorize]
        public ActionResult SendPage()
        {
            return View();
        }

        [Authorize]
        public ActionResult GetUserData(string userData)
        {
            string s = User.Identity.Name;
            string str = "I am here and i get Data!!!";
            string str1 = userData;
            //тут я должен вернуть данные с расчетами, но пока их нет
            return View("SendPage");
        }
    }
}