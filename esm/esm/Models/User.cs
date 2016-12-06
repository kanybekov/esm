using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace esm.Models
{
    /*
     * Класс для пользователей системы.
     */
    public class User
    {
        int id; //Идентификатор пользователя.
        string login;//Логин пользователя.
        bool hasTask;//Наличие задачи.
        Task currentTask;//Текущая задача
        public DateTime lastActivityTime { get; set; }//Время последней активности. Автоматически реализуемое свойство
        
        /*
         * Конструктор.
         * Входные параметры: идентификатор пользователя(целое число).
         */
        public User(int i) { id = i; }

        /*
         * Конструктор.
         * Входные параметры: идентификатор пользователя (целое число), логин(строка),
         * наличие задачи (истина или ложь), задача, время последней активности(Дата)
         */
        public User(int i, string log, bool hastask, Task curTask, DateTime lastActivTime) 
        { 
            id = i; 
            login = log; 
            hasTask = hastask;
            currentTask = curTask;
            lastActivityTime = lastActivTime;
        }

        //Поставить пользователю задачу
        public void setTask(Task task)
        {
            hasTask = true;
            currentTask = task;
        }

        //Сбросить задачу у пользователя
        public void resetTask()
        {
            hasTask = false;
            currentTask = null;
        }

        //Получить Id пользователя
        public int getId()
        {
            return id;
        }

        //Узнать имеет ли пользователь задачу
        public bool hasCurrentTask()
        {
            return hasTask;
        }

        //Получить задачу
        public Task getTask()
        {
            return currentTask;
        }

        //Получить логин
        public string getLogin()
        {
            return login;
        }
    }

    /*
     *Класс специально для регистрации пользователя. 
     */
    public class RegMe
    {
        //Логин пользователя. Автоматически реализуемое свойство
        [Required]
        [Display(Name="Имя пользователя")]
        public string login { get; set; }

        //Пароль пользователя. Автоматически реализуемое свойство
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        //Повтор пароля. Автоматически реализуемое свойство
        [Required]
        [Display(Name = "Повторите пароль")]
        [DataType(DataType.Password)]
        public string password1 { get; set; }
    }
}