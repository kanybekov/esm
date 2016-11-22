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
using System.Text.RegularExpressions;

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
            if (task != "-1" && func != "-1")
            {
                string taskFile = "/Content/data/" + task + ".js";
                string funcFile = "/Content/func/" + func + ".js";
                string html = "<script src=\"" + taskFile + "\"> </script>";
                html += "<script src=\"" + funcFile + "\"> </script>";
                html += "<script src=\"/Scripts/client_calc.js\"> </script>";
                html += "<script> makeCalculation(\"" + task + "\"); </script>";
                return Content(html);
            }

            return View("Master");
        }

        public ActionResult Results()
        {//Форма статуса вычислений. Если вычисление закончено, то показан результат
            Models.DatabaseMediator db = new Models.DatabaseMediator(Server.MapPath("~"));//обращаемся к базе
            Models.Task[] array = db.getUserTasks((int)Session["user_id"]);//выцыганиваем id юзера из сессии
            db.close();
            //ну и как то это всё обработали и  вывели

            List<String> list = new List<String>();
            string line;
            foreach (Models.Task t in array)
            {
                if(t.isSolved())
                {
                    // выводим
                    using (StreamReader sr = new StreamReader(t.getResultFilePath()))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            list.Add(t.getTaskId()+") "+line);
                        }
                    }
                }
            }
            if (list.Count == 0)
            {
                ViewBag.Out = null;
            }
            else
            {
                ViewBag.Out = list;
            }
            return View();           
        }

        public ActionResult Status()
        {
            List<String[]> userDataList = new List<String[]>();
            string line;
            using (StreamReader sr = new StreamReader(Server.MapPath("~/App_Data/UserData.txt")))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if(line != "")
                        userDataList.Add(line.Split('|'));
                }
            }

            List<String[]> userOnline = new List<String[]>();
            using (StreamReader sr = new StreamReader(Server.MapPath("~/App_Data/OnlineUsers.txt")))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "")
                        userOnline.Add(line.Split(' '));
                }
            }
            foreach (var i in userOnline)
            {
                for(int j=0; j<userDataList.Count; j++)
                {
                    if(userDataList[j][1] == i[0])
                    {
                        userDataList[j][0] = "1";
                    }
                    else
                    {
                        userDataList[j][0] = "0";
                    }
                }
            }
            ViewData["UserStat"] = userDataList;
            return View();
        }

        public ActionResult TransferIn()
        {//Форма загрузки данных с сервера на клиент
            //пока не знаю зачем, пусть будет

            return View();
        }

        public ActionResult TransferOut(string task, string result)
        {//Форма выгрузки результата с клиента на сервер
            int taskId = Convert.ToInt32(task);//каким-то образом получили id решеной задачи

            Models.DatabaseMediator db = new Models.DatabaseMediator(Server.MapPath("~"));
            Models.Task t = db.loadTask(taskId);//нашли нужную задачу
            t.setAnswer(result);//записали ответ
            db.saveTask(t);//сохранили в базу

            int userId = (int)Session["user_id"];
            User u = db.getUser(userId);
            u.resetTask();
            db.updateUser(u);

            int parent = t.getParentTaskId();
            if (parent != -1)//если есть родитель пишем результат в родителя
            {
                Models.Task t2 = db.loadTask(parent);//нашли родительскую задачу
                bool fin = t2.updateTask(result);//обновили её
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
            System.IO.StreamReader file = new System.IO.StreamReader(new FileStream(Server.MapPath("~/App_Data/Users.txt"), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            string line;
            while((line=file.ReadLine())!=null)
            {
                if (line != "")
                {
                    string[] logins = line.Split(' ');
                    if (hashStr == logins[1])
                    {
                        Models.DatabaseMediator db = new Models.DatabaseMediator(Server.MapPath("~"));//обращаемся к базе
                        Models.User u = db.getUserByLogin(logins[0]);
                        Session["user_id"] = u.getId();//выцыганиваем id из базы
                        db.close();//закрыли базу
                        FormsAuthentication.SetAuthCookie(username, false);
                        HttpContext.Response.Cookies["login"].Value = username;
                        //System.IO.File.AppendAllText(Server.MapPath("~/App_Data/OnlineUsers.txt"), username + " " + Request.UserHostAddress + "\n");

                        System.IO.StreamReader file1 = new System.IO.StreamReader(new FileStream(Server.MapPath("~/App_Data/OnlineUsers.txt"), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
                        List<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
                        string line1;
                        while ((line1 = file1.ReadLine()) != null)
                        {
                            if (line1 != "")
                            {
                                string[] str = line1.Split(' ');
                                users.Add(new KeyValuePair<string, string>(str[0], str[1]));
                            }
                        }
                        file1.Close();
                        System.IO.StreamWriter file2 = new System.IO.StreamWriter(new FileStream(Server.MapPath("~/App_Data/OnlineUsers.txt"), FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite));
                        users.Add(new KeyValuePair<string, string>(username, Request.UserHostAddress));
                        string result = "";
                        foreach (var str in users)
                        {
                            result += str.Key + " " + str.Value + "\n";
                        }
                        file2.Write(result);
                        file2.Close();

                        file.Close();
                        return View("Master");
                    }
                }
            }
            file.Close();
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
            System.IO.StreamReader file = new System.IO.StreamReader(new FileStream(Server.MapPath("~/App_Data/Users.txt"), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            
            if (regMe.password != regMe.password1)
            {
                ModelState.AddModelError("", "Пароли не совпадают!");
            }
            List<KeyValuePair<string, string>> logs = new List<KeyValuePair<string, string>>();
            if(!string.IsNullOrWhiteSpace(regMe.login))
            {               
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        string[] str = line.Split(' ');
                        logs.Add(new KeyValuePair<string, string>(str[0], str[1]));
                        logins.Add(str[0]);
                    }
                }
                file.Close();
                if (logins.Count()>0 && logins.Contains(regMe.login))
                    ModelState.AddModelError("", "Такой пользователь уже существует");
            }
            if(ModelState.IsValid)
            {
                SHA256Managed hash = new SHA256Managed();
                byte[] hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(regMe.login + regMe.password));
                string hashStr = BitConverter.ToString(hashBytes).Replace("-", "");
                logs.Add(new KeyValuePair<string, string>(regMe.login, hashStr));
                System.IO.StreamWriter file1 = new System.IO.StreamWriter(new FileStream(Server.MapPath("~/App_Data/Users.txt"), FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite));
                string resStr = "";
                foreach(var tmp in logs)
                {
                    resStr += tmp.Key + " " + tmp.Value + "\n";
                }
                file1.Write(resStr);
                file1.Close();
                DatabaseMediator db = new DatabaseMediator(Server.MapPath("~"));
                db.createUser(regMe.login);
                return View("Index");
            }
            return View("Register");
        }

        public ActionResult Logout(string loginUser)
        {
            FormsAuthentication.SignOut();
            System.IO.StreamReader file = new System.IO.StreamReader(new FileStream(Server.MapPath("~/App_Data/OnlineUsers.txt"), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            List<KeyValuePair<string, string>> users = new List<KeyValuePair<string, string>>();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (line != "")
                {
                    string[] logins = line.Split(' ');
                    users.Add(new KeyValuePair<string, string>(logins[0], logins[1]));
                }
            }
            file.Close();
            System.IO.StreamWriter file1 = new System.IO.StreamWriter(new FileStream(Server.MapPath("~/App_Data/OnlineUsers.txt"), FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite));
            users=users.Where(c => c.Key != loginUser).ToList();
            string result = "";
            foreach(var str in users)
            {
                result += str.Key + " " + str.Value + "\n";
            }
            file1.Write(result);
            file1.Close();
            return View("Index");
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
        public ActionResult Upload(string method)
        {
            string result = "Файл загружен";
            string filePath = "";
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    int id = (int)Session["user_id"];
                    filePath = Server.MapPath("~/App_Data/usertask/" + id.ToString());
                    upload.SaveAs(filePath);
                }
            }

            //-----------------
            string resultCheckFile = checkFormatFile(filePath);
            if (resultCheckFile.Length > 0)
            {
                result = "Файл не загружен, ошибка: " + resultCheckFile ;
                //удаляем файл из репозитория
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            else
            {
                //успешная загрузка ставим задачу на выполнение
                Models.Scheduler s = new Models.Scheduler(Server.MapPath("~"));
                bool fl = s.createTask((int)Session["user_id"], filePath, method);
                if (!fl)
                    result = "В данный момент задача не может быть решена. Попробуйте позже.";
            }
            //----------------------
            ViewBag.MessagerFromControl = result;
            return View("Send");
        }


        public string checkFormatFile(string nameFile)
        {
            int iterator_by_str = -1;
            int saveN = 0;
            string result = "";
            using (StreamReader fs = new StreamReader(nameFile))
            {
                while (true)
                {
                    // Читаем строку из файла во временную переменную.
                    string str = fs.ReadLine();

                    // Если достигнут конец файла, прерываем считывание.
                    if (str == null)
                    {
                        //проверка на четность
                        if (iterator_by_str < saveN)
                        {
                            result = result + "Целое число в 0 строке, обозначающее количество вводимых данных, не соответсвует количеству данных, вводимых ниже 0 строки.";
                        }
                        break;
                    }

                    //проверяем строку
                    if (iterator_by_str == -1)
                    {
                        bool noNum = Regex.IsMatch(str, @"^((\D+))$");
                        bool NoInt = Regex.IsMatch(str, @"^((\d+)(\.+)(\d*))$");
                        bool noCorectFormat = Regex.IsMatch(str, @"^((\d+\,\d+))$");
                        if (noNum || NoInt || noCorectFormat)
                        {
                            return result = result + " В 0 строке должно быть целое число, обозначающее количество вводимых данных. ";
                        }
                        /*if (!Regex.IsMatch(str, @"^[a-zA-Z0-9.]+(?:\s[a-zA-Z0-9.]+)?$"))
                        {
                            result = result + " Не верный формат данных в " + (iterator_by_str + 1) + " строке, В 0 строке должно быть целое число без пробельных символов до и после (" + str + ").";
                        }*/

                        saveN = int.Parse(str);
                    }
                    else
                    {
                        if (Regex.IsMatch(str, @"^((\d+\,\d+))$"))
                        {
                            result = result + " Не верный формат данных в " + (iterator_by_str+1) + " строке, для обозначения вещественного числа должна использоваться точка ("+str+ "). ";
                        }

                        if (!Regex.IsMatch(str, @"^[a-zA-Z0-9.]+(?:\s[a-zA-Z0-9.]+)?$") && Regex.IsMatch(str, @"[a-zA-Z]"))
                        {
                            result = result + " Не верный формат данных в " + (iterator_by_str+1) + " строке, один пробельный символ может быть только между двумя словами ("+str+ ").";
                        }
                    }

                    iterator_by_str++;

                }
            }

            // Выводим на экран.
            return result;
        }

    }
    
}
