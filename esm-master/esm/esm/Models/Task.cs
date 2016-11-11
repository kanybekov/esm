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
            string[] text = System.IO.File.ReadAllLines(dataFilePath);
            int dataLength = Convert.ToInt32(text[0]);
            for (int i = 1; i <= dataLength; ++i)
            {
                if (text[i] == "\n")
                {
                    text[i] = result;
                    break;
                }
            }
            System.IO.File.WriteAllLines(dataFilePath, text);
            //обновим инфу сколько данных в файле
            //код это делающий
            int emptyLines = 0;
            for (int i = 1; i <= dataLength; ++i)
            {
                if (text[i] == "\n")
                {
                    ++emptyLines;
                }
            }

            if (emptyLines == 0)
            {
                //далее переписываем файл по стандарту: пример /Content/data/...
                int numberOfData;
                int[] data;
                string[] args;
                TaskIO.parseInput(dataFilePath, out numberOfData, out data, out args);
                TaskIO.fillDataFile(basePath + "/Content/data/" + id + ".js", data, args);

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