using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace esm.Models
{
    public class Task
    {//здесь хранится любая инфа связанная с задачей
        int ownerId;//идентификатор юзера которому нужно сообщить о результате
        int id;//идентификатор задачи
        int parentId;//идентификатор родительской задачи
        string dataFilePath;//что решать
        string func;//чем решать
        bool hasChildTask;//есть ли вложенные задачи
        int numberOfChilds;//их количество
        int numberOfSolvedChilds;//количество решенных из них
        string basePath;//путь вида ~/Content/... не работает. Надо так basePath + "/Content/..."
        bool solved;//решена ли задача

        public Task(int owner_id, //идентификатор юзера которому нужно сообщить о результате
            int task_id, //идентификатор задачи
            int parent_task_id, //идентификатор родительской задачи
            string data_file_path, //что решать
            string function_name,//чем решать
            string base_path)//корневая папка
        {
            ownerId = owner_id;
            id = task_id;
            parentId = parent_task_id;
            dataFilePath = data_file_path;
            func = function_name;
            hasChildTask = false;//есть ли вложенные задачи
            numberOfChilds = 0;//их количество
            numberOfSolvedChilds = 0;//количество решенных из них
            solved = false;//задача ещё не решена
        }

        public void setChilds(int number_of_childs, int number_of_solved_childs = 0)
        {//объявить информацию о вложенности
            hasChildTask = true;
            numberOfChilds = number_of_childs;
            numberOfSolvedChilds = number_of_solved_childs;
        }

        public bool updateTask(string result)
        {//посчиталось какое-то данное, если вернулось true значит пора поставить задачу на выполнение, иначе игнорируем
            //добавим его в нужный файл
            System.IO.File.AppendAllText(dataFilePath, result);
            //обновим инфу сколько данных в файле
            //код это делающий
            //далее если набралось необходимое количество ответов
            //что-то типа считать первую строку из файла, вторую строку и если первая равна второй
            System.IO.StreamReader file = new System.IO.StreamReader(dataFilePath);
            string line1, line2;
            line1 = file.ReadLine();//что-то типа количества текущих ответов
            line2 = file.ReadLine();//сколько надо
            if (line1 == line2)
            {
                //далее переписываем файл по стандарту: пример /Content/data/...
                //мне это делать лень
                hasChildTask = false;
                solved = true;
                return true;
            }
            return false;
        }

        public void setAnswer(string result)
        {
            System.IO.File.WriteAllText(dataFilePath, result);
            solved = true;
        }

        //геттеры
        public int getOwnerId()
        {
            return ownerId;
        }

        public int getTaskId()
        {
            return id;
        }

        public int getParentTaskId()
        {
            return parentId;
        }

        public string getDataFilePath()
        {
            return dataFilePath;
        }

        public string getFunctionName()
        {
            return func;
        }

        public bool hasChilds()
        {
            return hasChildTask;
        }

        public int getNumberOfChilds()
        {
            return numberOfChilds;
        }

        public int getNumberOfSolvedChilds()
        {
            return numberOfSolvedChilds;
        }

        public bool isSolved()
        {
            return solved;
        }
    }
}