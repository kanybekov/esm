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
        {//Форма входа
            //MembershipUser user= Membership.CreateUser("Pavel", "matanbar");
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {//Форма херни
            ViewBag.Message = "MatanBar application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {//Форма херни намба ту
            ViewBag.Message = "MatanBar contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Master()
        {//Форма главной страницы. Здесь пользователь решает, что он хочет делать

            return View();
        }

        [Authorize]
        public ActionResult Send()
        {//Форма отправки исходнных данных на сервер
            return View();
        }

        [Authorize]
        public ActionResult Calculation()
        {//Форма вычислений
            //пока можно забить

            return View();
        }

        [Authorize]
        public ActionResult Results()
        {//Форма статуса вычислений. Если вычисление закончено, то показан результат
            Models.DatabaseMediator db = new Models.DatabaseMediator(Server.MapPath("~"));//обращаемся к базе
            Models.Task[] array = db.getUserTasks((int)Session["user_id"]);//выцыганиваем id юзера из сессии
            db.close();
            //ну и как то это всё обработали и  вывели
            return View();
        }

        [Authorize]
        public ActionResult TransferIn()
        {//Форма загрузки данных с сервера на клиент
            //пока не знаю зачем, пусть будет

            return View();
        }

        [Authorize]
        public ActionResult TransferOut()
        {//Форма выгрузки результата с клиента на сервер
            int taskId = 0;//каким-то образом получили id решеной задачи
            string res = "42e+1000";//и результат

            Models.DatabaseMediator db = new Models.DatabaseMediator(Server.MapPath("~"));
            Models.Task t = db.loadTask(taskId);//нашли нужную задачу
            t.setAnswer(res);//записали ответ
            db.saveTask(t);//сохранили в базу
            int parent = t.getParentTaskId();
            if (parent != -1)//если есть родитель пишем результат в родителя
            {
                Models.Task t2 = db.loadTask(parent);//нашли родительскую задачу
                bool fin = t2.updateTask(res);//обновили её
                db.saveTask(t2);
                db.close();
                if (fin)
                {//если все данные родительской задачи получены, то ставим на выполнение
                    Models.Scheduler s = new Models.Scheduler(Server.MapPath("~"));
                    s.setTask(t2);
                }
            }
                        
            return View();
        }

        [AllowAnonymous]
        public ActionResult BackgroundCheck()
        {//Форма работы с js клиента. Здесь используется всё черная магия

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
                    Models.DatabaseMediator db = new Models.DatabaseMediator(Server.MapPath("~"));//обращаемся к базе
                    Models.User u = db.getUserByLogin(logins[0]);
                    Session["user_id"] = u.getId();//выцыганиваем id из базы
                    db.close();//закрыли базу
                    FormsAuthentication.SetAuthCookie(username, false);
                    System.IO.File.AppendAllText(Server.MapPath("~/Content/OnlineUsers.txt"), username + " " + Request.UserHostAddress);
                    return View("Master");
                }
            }
            return View("Index");
        }

        

        [Authorize]
        public ActionResult GetUserData(string userData)
        {
            string s = User.Identity.Name;
            string str = "I am here and i get Data!!!";
            string str1 = userData;
            //тут я должен вернуть данные с расчетами, но пока их нет
            return View("Calculation");
        }
    }
}