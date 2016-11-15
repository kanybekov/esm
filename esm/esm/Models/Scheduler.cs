using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace esm.Models
{
    public class Scheduler
    {//Планировщик. Создает задачи и ставит их на выполнение
        string basePath;

        public Scheduler(string base_path)//вызов всегда как Models.Scheduler s = new Models.Scheduler(Server.MapPath("~"));
        {
            basePath = base_path;//путь вида ~/Content/... не работает. Надо так basePath + "/Content/..."
        }

        public void createTask(int userId, string filePath, string func)
        {//обрабатываем запрос на создание задачи
            DatabaseMediator db = new DatabaseMediator(basePath);//открыли базу
            int taskId = db.getFreeTaskId(); //получаем id задачи
            User[] users = db.getUsersOnlineWithoutTask();//получили список юзеров
            int numberOfData;
            double[] data;
            string[] args;
            TaskIO.parseInput(filePath, out numberOfData, out data, out args);
            int amountOfSubtasks = numberOfData / users.Count();

            //разбиваем файл задачи на подзадачи и помещаем их в /Content/data/???.js
            //например /Content/data/tmp1.js
            //функции хранятся в /Content/func/???.js
            //deprecated func = "test";//пусть для теста будет
            string masterFile = basePath + "/Content/task/" + taskId + ".js";
            TaskIO.fillTaskFile(masterFile, users.Length, args);            
            Task master = new Task(userId, taskId, -1, masterFile, func, basePath);//смотри описание класса Task

            
            for (int i = 0; i < users.Count(); ++i)
            {
                int subtaskId = db.getFreeTaskId();
                int start = i * amountOfSubtasks;
                int fin = amountOfSubtasks;
                TaskIO.fillDataFile(basePath + "/Content/data/" + subtaskId + ".js", data.Skip(start).Take(fin).ToArray(), args);

                Task slave = new Task(-1, subtaskId, taskId, basePath + "/Content/data/" + subtaskId + ".js", func, basePath);
                db.saveTask(slave);
                users[i].setTask(slave);
                db.updateUser(users[i]);
            }

            master.setChilds(users.Length);
            db.setUserTask(userId, taskId);
            db.saveTask(master);//сохраняем задачу в базу
            //users[0].setTask(master);//ставим задачу юзеру
            //db.updateUser(users[0]);//заносим в базу изменную инфу
            //работаем
            db.close();//закрыли базу
        }

        public void resetTask(int userId)
        {//короче проблемы с этим парнем, его задачу должен решить кто-то другой
            DatabaseMediator db = new DatabaseMediator(basePath);
            User tmp = db.getUser(userId);
            User[] users = db.getUsersOnlineWithoutTask();//выбираем первого попавшегося чувака, пусть он страдает
            users[0].setTask(tmp.getTask());
            db.updateUser(users[0]);
            tmp.resetTask();//а этот парень теперь не должен решать эту задачу
            db.updateUser(tmp);
            db.close();//saveTask не нужен, так задача уже сохранена
        }

        public void setTask(Task t)
        {//задача верхнего уровня готова к исполнению
            DatabaseMediator db = new DatabaseMediator(basePath);
            User[] users = db.getUsersOnlineWithoutTask();//выбираем чувака
            users[0].setTask(t);//ставим задачу
            db.updateUser(users[0]);
            db.close();
        }
    }
}