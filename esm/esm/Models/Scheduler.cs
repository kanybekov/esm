using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace esm.Models
{
    public class Scheduler
    {
        /*
        Класс планировщика задач. Создает задачи и назначает их пользователям.
        */
        string basePath;//путь вида ~/Content/... не работает. Надо так basePath + "/Content/..."

        /*
        Конструктор.
        Входные данные:
        строка указывающая корневую папку для корректной внутренней адрессации.
        Выходные данные:
        новый объект класса.
        */
        public Scheduler(string base_path)
        {
            basePath = base_path;//путь вида ~/Content/... не работает. Надо так basePath + "/Content/..."
        }

        /*
        Метод создает требуемую пользователю задачу и необходимые подзадачи. Все подзадачи распределяются между активными пользователями.
        Входные параметры:
        1) целое число хранящее идентификатор пользователя поставившего задачу (неотрицательное число);
        2) строка хранящая путь к файлу с введенными данными;
        3) строка с именем метода для решения задачи.
        Выходные параметры:
        булева переменная сигнализирующая о том, что задача может быть решена в текущий момент.
        Побочные эффекты:
        создание файлов данных задачи (частично незаполненого) и подзадач (полных) в папке /Content/data.
        */
        public bool createTask(int userId, string filePath, string func)
        {
            try
            {
                DatabaseMediator db = new DatabaseMediator(basePath);//открыли базу
                int taskId = db.getFreeTaskId(); //получаем id задачи
                User[] users = db.getUsersOnlineWithoutTask();//получили список юзеров
                if (users.Length == 0)
                    return false;
                int numberOfData;
                double[] data;
                string[] args;
                TaskIO.parseInput(filePath, out numberOfData, out data, out args);
                int amountOfSubtasks = numberOfData / users.Count();

                //разбиваем файл задачи на подзадачи и помещаем их в /Content/data/???.js
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
                db.close();//закрыли базу
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
        Метод переназначающий задачу неактивного пользователя.
        Входные параметры:
        целое число хранящее идентификатор неактивного пользователя (неотрицательное число).
        Выходные параметры:
        булева переменная сигнализирующая о том, что задача может быть решена в текущий момент.
        */
        public bool resetTask(int userId)
        {
            try
            {
                DatabaseMediator db = new DatabaseMediator(basePath);
                User tmp = db.getUser(userId);
                User[] users = db.getUsersOnlineWithoutTask();//выбираем первого попавшегося чувака, пусть он страдает
                if (users.Length == 0)
                    return false;
                users[0].setTask(tmp.getTask());
                db.updateUser(users[0]);
                tmp.resetTask();//а этот парень теперь не должен решать эту задачу
                db.updateUser(tmp);
                db.close();//saveTask не нужен, так задача уже сохранена
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
        Метод ставящий на выполнение задачу верхнего уровня.
        Входные параметры:
        объект задачи необходимой для решения.
        Выходные параметры:
        булева переменная сигнализирующая о том, что задача может быть решена в текущий момент.
        */
        public bool setTask(Task t)
        {
            try
            {
                DatabaseMediator db = new DatabaseMediator(basePath);
                User[] users = db.getUsersOnlineWithoutTask();//выбираем чувака
                if (users.Length == 0)
                    return false;
                users[0].setTask(t);//ставим задачу
                db.updateUser(users[0]);
                db.close();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}