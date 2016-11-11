using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace esm.Models
{
    public class User
    {//здесь хранится любая инфа связанная с конкретным пользователем
        int id;
        string login;//???
        bool hasTask;
        Task currentTask;

        public User(int i) { id = i; }

        public User(int i, string log, bool hastask, Task curTask) 
        { 
            id = i; 
            login = log; 
            hasTask = hastask;
            currentTask = curTask;
        }

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

        public string getLogin()
        {
            return login;
        }
    }

    public class RegMe
    {
        [Required]
        [Display(Name="Имя пользователя")]
        public string login { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [Display(Name = "Повторите пароль")]
        [DataType(DataType.Password)]
        public string password1 { get; set; }
    }
}