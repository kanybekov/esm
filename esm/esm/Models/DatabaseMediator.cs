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
        string timeFormat = "dd.MM.yyyy HH:mm:ss";
        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;

        public DatabaseMediator(string base_path)//вызов как Models.DatabaseMediator s = new Models.DatabaseMediator(Server.MapPath("~"));
        {
            //basePath = base_path;//путь вида ~/Content/... не работает. Надо так basePath + "/Content/..."
            basePath = base_path + "/App_Data/";
        }

        /*
         * Получить пользователей без задачи.
         * Входные данные отсутствуют.
         * Выходные данные: Массив, состоящий из объектов класса User. Содержит пользователей без задачи.
         * Побочные эффекты:
         * Отсутствуют.
         */
        public User[] getUsersOnlineWithoutTask()
        {
            System.IO.StreamReader file = new System.IO.StreamReader(new FileStream(basePath + "OnlineUsers.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            try
            {                
                string line;
                List<string> onlineUser = new List<string>();
                while ((line = file.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        string[] logins = line.Split(' ');
                        onlineUser.Add(logins[0]);
                    }
                }
                file.Close();
                file = new System.IO.StreamReader(new FileStream(basePath + "UserData.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
                line = null;
                List<User> users = new List<User>();
                while ((line = file.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        JavaScriptSerializer jser = new JavaScriptSerializer();
                        string[] datas = line.Split('|');
                        DateTime data;
                        if (!DateTime.TryParseExact(datas[4],timeFormat,provider, System.Globalization.DateTimeStyles.AdjustToUniversal, out data))
                            data = DateTime.UtcNow;
                        if (onlineUser.Contains(datas[1]) && !Convert.ToBoolean(datas[2]))
                        {
                            users.Add(new User(Convert.ToInt32(datas[0]),
                                datas[1],
                                Convert.ToBoolean(datas[2]),
                                loadTask(jser.Deserialize<Int32>(datas[3])),
                                data
                                ));
                        }
                    }
                }
                file.Close();
                return users.ToArray();
            }
            catch(Exception e)
            {
                file.Close();
                throw e;
            }
        }

        /*
         * Получить пользователя по его идентификатору.
         * Входные данные: идентификатор пользователя.
         * Выходные данные: объект класса User. Содержит информацию о пользователе.
         * Если пользователь не найден, возвращается пользователь с отрицательным идентификатором.
         * Побочные эффекты:
         * Отсутствуют.
         */
        public User getUser(int id)
        {
            System.IO.StreamReader file1 = new System.IO.StreamReader(new FileStream(basePath + "UserData.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            try
            {
                User result = new User(-1);                 
                string line;
                while ((line = file1.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        JavaScriptSerializer jser = new JavaScriptSerializer();
                        string[] datas = line.Split('|');
                        DateTime data;
                        if (!DateTime.TryParseExact(datas[4], timeFormat, provider, System.Globalization.DateTimeStyles.AdjustToUniversal, out data))
                            data = DateTime.UtcNow;
                        if (Convert.ToInt32(datas[0]) == id)
                        {
                            result = new User(Convert.ToInt32(datas[0]),
                                datas[1],
                                Convert.ToBoolean(datas[2]),
                                loadTask(jser.Deserialize<Int32>(datas[3])),
                                data
                                );
                        }
                    }
                }
                file1.Close();
                return result;
            }
            catch(Exception e)
            {
                file1.Close();
                throw e;
            }
        }

        /*
         * Получить пользователя по его логину.
         * Входные данные: логин пользователя.
         * Выходные данные: объект класса User. Содержит информацию о пользователе.
         * Если пользователь не найден, возвращается пользователь с отрицательным идентификатором.
         * Побочные эффекты:
         * Отсутствуют.
         */
        public User getUserByLogin(string login)
        {
            System.IO.StreamReader file1 = new System.IO.StreamReader(new FileStream(basePath + "UserData.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            try
            {
                User result = new User(-1);                
                string line;
                while ((line = file1.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        JavaScriptSerializer jser = new JavaScriptSerializer();
                        string[] datas = line.Split('|');
                        DateTime data;
                        if (!DateTime.TryParseExact(datas[4], timeFormat, provider, System.Globalization.DateTimeStyles.AdjustToUniversal, out data))
                            data = DateTime.UtcNow;
                        if (datas[1] == login)
                        {
                            result = new User(Convert.ToInt32(datas[0]),
                                datas[1],
                                Convert.ToBoolean(datas[2]),
                                loadTask(jser.Deserialize<Int32>(datas[3])),
                                data
                                );
                        }
                    }
                }
                file1.Close();
                return result;
            }
            catch(Exception e)
            {
                file1.Close();
                throw e;
            }
        }

        /*
         * Обновить информацию о пользователе.
         * Входные данные: экземпляр класса User, содержащий информацию о пользователе.
         * Выходные данные: отсутствуют.
         * Побочные эффекты:
         * 1. Модифицируется файл /App_Data/UserData.txt
         */
        public void updateUser(User u)
        {
            System.IO.StreamReader file1 = new System.IO.StreamReader(new FileStream(basePath + "UserData.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            try
            {
                List<User> result = new List<User>();                
                string line;
                while ((line = file1.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        JavaScriptSerializer jser = new JavaScriptSerializer();
                        string[] datas = line.Split('|');
                        DateTime data;
                        if (!DateTime.TryParseExact(datas[4], timeFormat, provider, System.Globalization.DateTimeStyles.AdjustToUniversal, out data))
                            data = DateTime.UtcNow;
                        result.Add(new User(Convert.ToInt32(datas[0]),
                            datas[1],
                            Convert.ToBoolean(datas[2]),
                            loadTask(jser.Deserialize<Int32>(datas[3])),
                            data
                            ));
                    }
                }
                file1.Close();
                int index = result.IndexOf(result.Where(c => c.getId() == u.getId()).FirstOrDefault());
                result[index] = u;
                string resstring = "";
                foreach (var item in result)
                {
                    JavaScriptSerializer jser = new JavaScriptSerializer();
                    saveTask(item.getTask());
                    int taskId = -1;
                    if (item.getTask() != null)
                        taskId = item.getTask().getTaskId();
                    resstring += item.getId() + "|" + item.getLogin() + "|" + item.hasCurrentTask() + "|" + jser.Serialize(taskId) + "|" + item.lastActivityTime + "\n";
                }
                System.IO.File.WriteAllText(basePath + "UserData.txt", resstring);
            }
            catch (Exception e)
            {
                file1.Close();
                throw e;
            }
        }

        /*
         * Получить дату последней активности пользователя по его логину.
         * Входные данные: логин пользователя.
         * Выходные данные: дата последней активности пользователя.
         * Побочные эффекты:
         * Отсутствуют.
         */
        public DateTime getUserLastActivity(string userlogin)
        {
            try
            {
                User cur_user = getUserByLogin(userlogin);
                return cur_user.lastActivityTime;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /*
         * Получить дату последней активности пользователя по его идентификатору.
         * Входные данные: идентификатору пользователя.
         * Выходные данные: дата последней активности пользователя.
         * Побочные эффекты:
         * Отсутствуют.
         */
        public DateTime getUserLastActivity(int userId)
        {
            try
            {
                User cur_user = getUser(userId);
                return cur_user.lastActivityTime;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /*
         * Установить дату последней активности пользователя по его логину.
         * Входные данные: логин пользователя, дата последней активности.
         * Выходные данные: Отсутствуют.
         * Побочные эффекты:
         * 1. Модифицируется файл /App_Data/UserData.txt
         */
        public void setUserLastActivity(string userLogin, DateTime activityTime)
        {
            try
            {
                User cur_user = getUserByLogin(userLogin);
                cur_user.lastActivityTime = activityTime;
                updateUser(cur_user);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /*
         * Установить дату последней активности пользователя по его идентификатору.
         * Входные данные: идентификатору пользователя, дата последней активности.
         * Выходные данные: Отсутствуют.
         * Побочные эффекты:
         * 1. Модифицируется файл /App_Data/UserData.txt
         */
        public void setUserLastActivity(int id, DateTime activityTime)
        {
            try
            {
                User cur_user = getUser(id);
                cur_user.lastActivityTime = activityTime;
                updateUser(cur_user);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /*
         * Получить не активных пользователей, у которых есть задача.
         * Входные данные отсутствуют.
         * Выходные данные: Массив, состоящий из объектов класса User. Содержит не активных пользователей, у которых есть задача.
         * Побочные эффекты:
         * Отсутствуют.
         */
        public List<User> getUnactiveUsersWithTask()
        {
            System.IO.StreamReader file1 = new System.IO.StreamReader(new FileStream(basePath + "UserData.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            try
            {
                List<User> result = new List<User>();                
                string line;
                while ((line = file1.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        JavaScriptSerializer jser = new JavaScriptSerializer();
                        string[] datas = line.Split('|');

                        TimeSpan time = DateTime.UtcNow - Convert.ToDateTime(datas[4]);
                        if (Convert.ToBoolean(datas[2]) && time.TotalMinutes >= 2)
                        {
                            result.Add(new User(Convert.ToInt32(datas[0]),
                                datas[1],
                                Convert.ToBoolean(datas[2]),
                                loadTask(jser.Deserialize<Int32>(datas[3])),
                                Convert.ToDateTime(datas[4])
                                ));
                        }
                    }
                }
                file1.Close();
                return result;
            }
            catch (Exception e)
            {
                file1.Close();
                throw e;
            }
        }

        /*
         * Создать пользователя.
         * Входные данные логин пользователя.
         * Выходные данные: Отсутствуют.
         * Побочные эффекты:
         * 1. Модифицируется файл /App_Data/UserData.txt
         */
        public void createUser(string login)
        {
            StreamReader file1 = new StreamReader(new FileStream(basePath + "UserData.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            try
            {
                List<User> result = new List<User>();                
                string line;
                while ((line = file1.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        JavaScriptSerializer jser = new JavaScriptSerializer();
                        string[] datas = line.Split('|');
                        DateTime data;
                        if (!DateTime.TryParseExact(datas[4], timeFormat, provider, System.Globalization.DateTimeStyles.AdjustToUniversal, out data))
                            data = DateTime.UtcNow;
                        result.Add(new User(Convert.ToInt32(datas[0]),
                            datas[1],
                            Convert.ToBoolean(datas[2]),
                            loadTask(jser.Deserialize<Int32>(datas[3])),
                            data
                            ));
                    }
                }
                file1.Close();
                int i = 0;
                if (result.Count() > 0)
                    i = result.Select(c => c.getId()).Max();
                User user = new User(i + 1, login, false, null, DateTime.Now);
                result.Add(user);
                string resstring = "";
                foreach (var item in result)
                {
                    JavaScriptSerializer jser = new JavaScriptSerializer();
                    saveTask(item.getTask());
                    int taskId = -1;
                    if (item.getTask() != null)
                        taskId = item.getTask().getTaskId();
                    resstring += item.getId() + "|" + item.getLogin() + "|" + item.hasCurrentTask() + "|" + jser.Serialize(taskId) + "|" + item.lastActivityTime + "\n";
                }
                System.IO.File.WriteAllText(basePath + "UserData.txt", resstring);
            }
            catch(Exception e)
            {
                file1.Close();
                throw e;
            }
        }

        /*
         * Создает id для новой задачи
         * Входные данные: Отсутствуют.
         * Выходные данные: целое число(id для новой задачи)
         * Побочные эффекты:
         * 1. Модифицируется файл /App_Data/counter.txt
         */
        public int getFreeTaskId()
        {
            try
            {
                int count = Convert.ToInt32(System.IO.File.ReadAllText(basePath + "counter.txt"));
                ++count;
                System.IO.File.WriteAllText(basePath + "counter.txt", count.ToString());
                return count - 1;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /*
         * Сохранить задачу в базе.
         * Входные данные экземпляр класса Task.
         * Выходные данные: Отсутствуют.
         * Побочные эффекты:
         * 1. Создается файл /App_Data/task/i.bin, где i - id задачи
         */
        public void saveTask(Task input)
        {
            try
            {
                if (input == null)
                    return;
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Stream fStream = new FileStream(basePath + "/task/" + input.getTaskId().ToString() + ".bin", FileMode.Create);
                binFormat.Serialize(fStream, input);
                fStream.Close();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /*
         * загрузить задачу по идентификатору
         * Входные данные: целое число(id задачи).
         * Выходные данные: экземпляр класса Task
         * Побочные эффекты:
         * Отсутствуют
         */
        public Task loadTask(int taskId)
        {
            try
            {
                if (taskId == -1)
                    return null;
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binFormat = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Stream fStream = new FileStream(basePath + "/task/" + taskId.ToString() + ".bin", FileMode.Open);
                Task tmp = (Task)binFormat.Deserialize(fStream);
                fStream.Close();
                return tmp;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /*
         * Прикрепить пользователю с идентификатором id задачу с идентификатором TaskId
         * Входные данные: целое число(идентификатор id пользователя), целое число(идентификатор id задачи).
         * Выходные данные: Отсутствуют.
         * Побочные эффекты:
         * 1. Модифицируется файл /App_Data/user_task.txt
         */
        public void setUserTask(int userID, int TaskID)
        {
            try
            {
                System.IO.File.AppendAllText(basePath + "user_task.txt", userID.ToString() + " " + TaskID.ToString() + "\n");
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /*
         * Получить все задачи пользователя с идентификатором id
         * Входные данные: целое число(идентификатор id пользователя).
         * Выходные данные: Массив, состоящий из объектов класса Task.
         * Побочные эффекты:
         * Отсутствуют
         */
        public Task[] getUserTasks(int userID)
        {
            StreamReader file1 = new StreamReader(new FileStream(basePath + "user_task.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite));
            try
            {
                List<Task> res = new List<Task>();                
                string line;
                while ((line = file1.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        string[] datas = line.Split(' ');
                        int tmpId = Convert.ToInt32(datas[0]);
                        if (tmpId == userID)
                        {
                            res.Add(loadTask(Convert.ToInt32(datas[1])));
                        }
                    }
                }
                file1.Close();
                return res.ToArray();
            }
            catch(Exception e)
            {
                file1.Close();
                throw e;
            }
        }

        public void close()
        {//закрыть подключение к базе

        }
    }
}