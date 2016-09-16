using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace esm.Models
{
    public class User
    {//здесь хранится любая инфа связанная с конкретным пользователем
        int id;
        string login;//???
        bool hasTask;
        Task currentTask;

        public User(int i) { id = i; }

        public void setTask(Task task)
        {//юзеру ставится задача
            hasTask = true;
            currentTask = task;
        }

        public void resetTask()
        {//если есть задача, то её можно не выполнять
            hasTask = false;
            currentTask = null;
        }

        //геттеры
        public int getId()
        {
            return id;
        }

        public bool hasCurrentTask()
        {
            return hasTask;
        }

        public Task getTask()
        {
            return currentTask;
        }
    }
}