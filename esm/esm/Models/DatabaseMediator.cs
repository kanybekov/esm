using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;

namespace esm.Models
{
    public class DatabaseMediator
    {//Посредник при работе с БД. Всё-всё хранится через него
        string basePath;
        static int count;

        public DatabaseMediator(string base_path)//вызов как Models.DatabaseMediator s = new Models.DatabaseMediator(Server.MapPath("~"));
        {
            //basePath = base_path;//путь вида ~/Content/... не работает. Надо так basePath + "/Content/..."
            basePath = base_path + "/App_Data/";
            count = 0;
        }

        public User[] getUsersOnlineWithoutTask()
        {//получить список юзеров онлайн без текущих задач
            //User[] array = new User[1];
            //array[0] = new User(0);//откуда-то берутся пользователи
            //return array;//и возвращаются
            System.IO.StreamReader file = new System.IO.StreamReader(basePath + "OnlineUsers.txt");
            string line;
            List<string> onlineUser = new List<string>();
            while ((line = file.ReadLine()) != null)
            {
                string[] logins = line.Split(' ');
                onlineUser.Add(logins[0]);
            }
            file.Close();
            System.IO.StreamReader file1 = new System.IO.StreamReader(basePath + "UserData.txt");
            line = null;
            List<User> users = new List<User>();
            while ((line = file1.ReadLine()) != null)
            {
                if (line != "")
                {
                    JavaScriptSerializer jser = new JavaScriptSerializer();
                    string[] datas = line.Split('|');
                    if (onlineUser.Contains(datas[1]) && !Convert.ToBoolean(datas[2]))
                    {
                        users.Add(new User(Convert.ToInt32(datas[0]),
                            datas[1], Convert.ToBoolean(datas[2]), loadTask(jser.Deserialize<Int32>(datas[3]))
                            ));
                    }
                }
            }
            file1.Close();
            return users.ToArray();
        }

        public User getUser(int id)
        {//сдать юзера в ответ
            User result = new User(-1); ;
            System.IO.StreamReader file1 = new System.IO.StreamReader(basePath + "UserData.txt");
            string line;
            while ((line = file1.ReadLine()) != null)
            {
                if (line != "")
                {
                    JavaScriptSerializer jser = new JavaScriptSerializer();
                    string[] datas = line.Split('|');
                    if (Convert.ToInt32(datas[0]) == id)
                    {
                        result = new User(Convert.ToInt32(datas[0]),
                            datas[1], Convert.ToBoolean(datas[2]), loadTask(jser.Deserialize<Int32>(datas[3]))
                            );
                    }
                }
            }
            file1.Close();
            return result;
        }

        public User getUserByLogin(string login)
        {
            User result = new User(-1); ;
            System.IO.StreamReader file1 = new System.IO.StreamReader(basePath + "UserData.txt");
            string line;
            while ((line = file1.ReadLine()) != null)
            {
                if (line != "")
                {
                    JavaScriptSerializer jser = new JavaScriptSerializer();
                    string[] datas = line.Split('|');
                    if (datas[1] == login)
                    {
                        result = new User(Convert.ToInt32(datas[0]),
                            datas[1], Convert.ToBoolean(datas[2]), loadTask(jser.Deserialize<Int32>(datas[3]))
                            );
                    }
                }
            }
            file1.Close();
            return result;
        }

        public void updateUser(User u)
        {//обновить запись о юзере в базе
            //int id = u.getId();//получили id
            //положили куда надо
            List<User> result = new List<User>(); ;
            System.IO.StreamReader file1 = new System.IO.StreamReader(new FileStream(basePath + "UserData.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            string line;
            while ((line = file1.ReadLine()) != null)
            {
                if (line != "")
                {
                    JavaScriptSerializer jser = new JavaScriptSerializer();
                    string[] datas = line.Split('|');

                    result.Add(new User(Convert.ToInt32(datas[0]),
                        datas[1], Convert.ToBoolean(datas[2]), loadTask( jser.Deserialize<Int32>(datas[3]) )
                        ));
                }
            }
            file1.Close();
            //System.IO.FileStream fil = new System.IO.FileStream(basePath + "UserData.txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite,System.IO.FileShare.ReadWrite);
            //fil.
            //var lines = System.IO.File.ReadAllLines(basePath + "UserData.txt");
            //foreach(var lin in lines)
            //{
            //    JavaScriptSerializer jser = new JavaScriptSerializer();
            //    string[] datas = lin.Split('|');

            //    result.Add(new User(Convert.ToInt32(datas[0]),
            //        datas[1], Convert.ToBoolean(datas[2]), jser.Deserialize<Task>(datas[3])
            //        ));
            //}
            int index = result.IndexOf(result.Where(c => c.getId() == u.getId()).FirstOrDefault());
            result[index] = u;
            //System.IO.File.Delete(basePath + "UserData.txt");
            System.IO.StreamWriter file = new System.IO.StreamWriter(new FileStream(basePath + "UserData.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            string resstring = "";
            foreach (var item in result)
            {
                JavaScriptSerializer jser = new JavaScriptSerializer();
                saveTask(item.getTask());
                int taskId = -1;
                if (item.getTask() != null)
                    taskId = item.getTask().getTaskId();
                resstring += item.getId() + "|" + item.getLogin() + "|" + item.hasCurrentTask() + "|" + jser.Serialize(taskId) + "\n";
                //System.IO.File.AppendAllText(basePath + "UserData.txt", item.getId() + "|" + item.getLogin() + "|" + item.hasCurrentTask() + "|" + jser.Serialize(item.getTask()) + "\n");
            }
            file.WriteLine(resstring);
            file.Close();
        }

        public void createUser(string login)
        {
            List<User> result = new List<User>(); ;
            StreamReader file1 = new StreamReader(new FileStream(basePath + "UserData.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            string line;
            while ((line = file1.ReadLine()) != null)
            {
                if (line != "")
                {
                    JavaScriptSerializer jser = new JavaScriptSerializer();
                    string[] datas = line.Split('|');

                    result.Add(new User(Convert.ToInt32(datas[0]),
                        datas[1], Convert.ToBoolean(datas[2]), loadTask(jser.Deserialize<Int32>(datas[3]))
                        ));
                }
            }
            file1.Close();
            int i = result.Select(c => c.getId()).Max();
            User user = new User(i+1, login, false, null);
            result.Add(user);
            StreamWriter file = new StreamWriter(new FileStream(basePath + "UserData.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            string resstring = "";
            foreach (var item in result)
            {
                JavaScriptSerializer jser = new JavaScriptSerializer();
                saveTask(item.getTask());
                int taskId = -1;
                if (item.getTask() != null)
                    taskId = item.getTask().getTaskId();
                resstring += item.getId() + "|" + item.getLogin() + "|" + item.hasCurrentTask() + "|" + jser.Serialize(taskId) + "\n";
            }
            file.WriteLine(resstring);
            file.Close();
        }
        public int getFreeTaskId()
        {//создает id для новой задачи
            return ++count;
        }

        public void saveTask(Task input)
        {//сохранить задачу в базе. можно юзать /Content/task/task???.txt где ??? - id задачи
            if (input == null)
                return;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream fStream = new FileStream(basePath + "/task/" + input.getTaskId().ToString() + ".bin", FileMode.Create);
            binFormat.Serialize(fStream, input);
            fStream.Close();
        }

        public Task loadTask(int taskId)
        {//загрузить задачу по идентификатору
            if (taskId == -1)
                return null;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream fStream = new FileStream(basePath + "/task/" + taskId.ToString() + ".bin", FileMode.Open);
            Task tmp = (Task) binFormat.Deserialize(fStream);
            fStream.Close();
            return tmp;
        }

        public void setUserTask(int userID, int TaskID)
        {
            System.IO.File.AppendAllText(basePath + "user_task.txt", userID.ToString() + " " + TaskID.ToString() + "\n");
        }

        public Task[] getUserTasks(int userID)
        {//все задачи поставленные данным пользователем
            List<Task> res = new List<Task>();
            StreamReader file1 = new StreamReader(new FileStream(basePath + "user_task.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            string line;
            while ((line = file1.ReadLine()) != null)
            {
                if (line != "")
                {
                    string[] datas = line.Split(' ');
                    int tmpId = Convert.ToInt32(datas[0]);
                    if (tmpId == userID)
                    {
                        res.Add( loadTask( Convert.ToInt32(datas[1]) ) );
                    }
                }
            }
            file1.Close();
            return res.ToArray();
        }

        public void close()
        {//закрыть подключение к базе

        }
    }
}