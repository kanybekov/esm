using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace esm.Models
{
    public class DatabaseMediator
    {//Посредник при работе с БД. Всё-всё хранится через него
        string basePath;

        public  DatabaseMediator(string base_path)//вызов как Models.DatabaseMediator s = new Models.DatabaseMediator(Server.MapPath("~"));
        {
            basePath = base_path;//путь вида ~/Content/... не работает. Надо так basePath + "/Content/..."
        }

        public User[] getUsersOnlineWithoutTask()
        {//получить список юзеров онлайн без текущих задач
            User[] array = new User[1];
            array[0] = new User(0);//откуда-то берутся пользователи
            return array;//и возвращаются
        }

        public User getUser(int id)
        {//сдать юзера в ответ
            return new User(0);
        }

        public User getUserByLogin(string login)
        {
            return new User(0);
        }

        public void updateUser(User u)
        {//обновить запись о юзере в базе
            int id = u.getId();//получили id
            //положили куда надо
        }

        public int getFreeTaskId()
        {//создает id для новой задачи
            return 0;
        }

        public void saveTask(Task input)
        {//сохранить задачу в базе. можно юзать /Content/task/task???.txt где ??? - id задачи

        }

        public Task loadTask(int taskId)
        {//загрузить задачу по идентификатору
            return new Task(-1, -1, -1, "", "", basePath);
        }

        public Task[] getUserTasks(int userID)
        {//все задачи поставленные данным пользователем
            return null;
        }

        public void close()
        {//закрыть подключение к базе

        }
    }
}