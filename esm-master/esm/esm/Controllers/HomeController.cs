using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using esm.Models;
using System.IO;

namespace esm.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        #region Инфа о приложении
        [AllowAnonymous]
        public ActionResult About()
        {//Форма херни
            int i=0;
            ViewBag.Message = "MatanBar application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {//Форма херни намба ту
            ViewBag.Message = "MatanBar contact page.";

            return View();
        }

        #endregion

        #region Для вычислений и прочая ерунда

        public ActionResult Master()
        {//Форма главной страницы. Здесь пользователь решает, что он хочет делать

            return View();
        }

        public ActionResult Send()
        {//Форма отправки исходнных данных на сервер
            return View();
        }

        public ActionResult Calculation(string task="-1", string func="-1")
        {//Форма вычислений
            //пока можно забить

            return View();
        }

        public ActionResult Results()
        {//Форма статуса вычислений. Если вычисление закончено, то показан результат
            Models.DatabaseMediator db = new Models.DatabaseMediator(Server.MapPath("~"));//обращаемся к базе
            Models.Task[] array = db.getUserTasks((int)Session["user_id"]);//выцыганиваем id юзера из сессии
            db.close();
            //ну и как то это всё обработали и  вывели
            return View();
        }

        public ActionResult TransferIn()
        {//Форма загрузки данных с сервера на клиент
            //пока не знаю зачем, пусть будет

            return View();
        }

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
        public ContentResult BackgroundCheck(int request=0)
        {//Форма работы с js клиента. Здесь используется всё черная магия
            string result = "";
            Models.DatabaseMediator db = new Models.DatabaseMediator(Server.MapPath("~"));//обращаемся к базе
            Models.User u;
            Models.Task t;
            switch (request)
            {
                default:
                case 0://пустое поле или ошибка
                    result = "bad request";
                    break;
                case 1://юзер говорит нам, что жив
                    u = db.getUser((int) Session["user_id"]);
                    t = u.getTask();
                    if (t == null)
                        result = "ok";
                    else
                        result = "task";
                    break;
                case 2://возвращаем юзеру id поставленной ему задачи
                    u = db.getUser((int)Session["user_id"]);
                    t = u.getTask();
                    if (t != null)
                        result = t.getTaskId().ToString();
                    else
                        result = "-1";
                    break;
                case 3://возвращаем юзеру название функции для его задачи
                    u = db.getUser((int)Session["user_id"]);
                    t = u.getTask();
                    if (t != null)
                        result = t.getFunctionName();
                    else
                        result = "-1";
                    break;
            }
            db.close();
            return Content(result);
        }

        #endregion

        #region Авторизация и регистрация, главная

        [AllowAnonymous]
        public ActionResult Login(string username, string password)
        {
            SHA256Managed hash = new SHA256Managed();
            byte[] hashBytes=hash.ComputeHash(Encoding.UTF8.GetBytes(username + password));
            string hashStr = BitConverter.ToString(hashBytes).Replace("-","");
            System.IO.StreamReader file = new System.IO.StreamReader(Server.MapPath("~/App_Data/Users.txt"));
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
                    System.IO.File.AppendAllText(Server.MapPath("~/App_Data/OnlineUsers.txt"), username + " " + Request.UserHostAddress + "\n");
                    return View("Master");
                }
            }
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult Index()
        {//Форма входа
            return View();
        }
        
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(RegMe regMe)
        {
            List<string> logins = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(Server.MapPath("~/App_Data/Users.txt"));
            
            if (regMe.password != regMe.password1)
            {
                ModelState.AddModelError("", "Пароли не совпадают!");
            }
            
            if(!string.IsNullOrWhiteSpace(regMe.login))
            {               
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] str = line.Split(' ');
                    logins.Add(str[0]);
                }
                file.Close();
                if (logins.Contains(regMe.login))
                    ModelState.AddModelError("", "Такой пользователь уже существует");
            }
            if(ModelState.IsValid)
            {
                SHA256Managed hash = new SHA256Managed();
                byte[] hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(regMe.login + regMe.password));
                string hashStr = BitConverter.ToString(hashBytes).Replace("-", "");
                System.IO.File.AppendAllText(Server.MapPath("~/App_Data/Users.txt"), "\n" + regMe.login + " " + hashStr);
                DatabaseMediator db = new DatabaseMediator(Server.MapPath("~"));
                db.createUser(regMe.login);
                return View("Index");
            }
            return View("Register");
        }
        #endregion


        public ActionResult GetUserData(string userData)
        {
            string s = User.Identity.Name;
            string str = "I am here and i get Data!!!";
            string str1 = userData;
            //тут я должен вернуть данные с расчетами, но пока их нет
            return View("Calculation");
        }

        //не трогать, мое!!!
        [HttpPost]
        public JsonResult Upload()
        {
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                     Models.DatabaseMediator db = new Models.DatabaseMediator(Server.MapPath("~"));//обращаемся к базе
                     Models.User user = db.getUser((int)Session["user_id"]);
                     int id = user.getId();
                    db.close();
                   
                    upload.SaveAs(Server.MapPath("~/App_Data/usertask/" + id.ToString()));
                }
            }

            return Json("файл загружен");
        }
    }
}
