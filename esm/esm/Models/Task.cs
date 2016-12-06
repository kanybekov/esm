using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace esm.Models
{
    [Serializable]
    public class Task
    {
        /*
        Класс хранит всё что касается поставленной задачи.
        */
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
        string resultFilePath;//здесь хранится результат задачи

        /*
        Конструктор по умолчанию, заполняющий все поля неверными значениями.
        */
        public Task()
        {
            ownerId = -1;
            id = -1;
            parentId = -1;
            dataFilePath = "";
            func = "";
            resultFilePath = "";
            hasChildTask = false;//есть ли вложенные задачи
            numberOfChilds = 0;//их количество
            numberOfSolvedChilds = 0;//количество решенных из них
            solved = false;//задача ещё не решена
        }

        /*
        Конструктор.
        Входные данные:
        1) целое число хранящее идентификатор юзера которому нужно сообщить о результате (неотрицательное число);
        2) целое число хранящее идентификатор задачи (неотрицательное число);
        3) целое число хранящее идентификатор родительской задачи (неотрицательное число);
        4) строка указывающая на файл с данными;
        5) строка хранящая имя функции для решения;
        6) строка указывающая корневую папку для корректной внутренней адрессации.
        Выходные данные:
        новый объект класса.
        */
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
            basePath = base_path;
            resultFilePath = base_path + "/Content/result/" + id.ToString() + ".txt";
            hasChildTask = false;//есть ли вложенные задачи
            numberOfChilds = 0;//их количество
            numberOfSolvedChilds = 0;//количество решенных из них
            solved = false;//задача ещё не решена
        }

        /*
        Обновить информацию о количестве вложенных задач.
        Входные параметры:
        1) целое число хранящее количествов вложенных задач (неотрицательное число);
        2) целое число хранящее количество уже решенных вложенных задач (по умолчанию 0, не должно превосходить числа вложенных задач).
        */
        public void setChilds(int number_of_childs, int number_of_solved_childs = 0)
        {
            hasChildTask = true;
            numberOfChilds = number_of_childs;
            numberOfSolvedChilds = number_of_solved_childs;
        }

        /*
        Метод сохраняющий решение вложенной задачи и обновляющий данные текущей задачи.
        Входные параметры:
        строка с результатом работы очеродной вложенной задачи.
        Выходные параметры:
        булева переменная сигнализирующая пора ли поставить задачу на выполнение.
        Побочные эффекты:
        чтение и запись в файл данных задачи;
        может быть создан файл задачи %taskID%.js в папке /Content/data/.
        */
        public bool updateTask(string result)
        {
            try
            {
                //добавим его в нужный файл
                string[] text = System.IO.File.ReadAllLines(dataFilePath);
                int dataLength = Convert.ToInt32(text[0]);
                for (int i = 1; i <= dataLength; ++i)
                {
                    if (text[i] == "")
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
                    if (text[i] == "")
                    {
                        ++emptyLines;
                    }
                }

                if (emptyLines == 0)
                {
                    //далее переписываем файл по стандарту: пример /Content/data/...
                    int numberOfData;
                    double[] data;
                    string[] args;
                    TaskIO.parseInput(dataFilePath, out numberOfData, out data, out args);
                    dataFilePath = basePath + "/Content/data/" + id.ToString() + ".js";
                    TaskIO.fillDataFile(dataFilePath, data, args);

                    hasChildTask = false;
                    solved = true;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
        Метод сохраняющий решение задачи.
        Входные данные:
        строка с результатом работы задачи.
        Побочные эффекты:
        запись в файл результата.
        */
        public void setAnswer(string result)
        {
            try
            {
                System.IO.File.WriteAllText(resultFilePath, result);
                solved = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*
        Получение идентификатора пользователя поставившего задачу.
        Выходные параметры:
        целое число хранящее идентификатор (неотрицательное число).
        */
        public int getOwnerId()
        {
            return ownerId;
        }

        /*
        Получение идентификатора задачи.
        Выходные параметры:
        целое число хранящее идентификатор (неотрицательное число).
        */
        public int getTaskId()
        {
            return id;
        }

        /*
        Получение идентификатора родительской задачи.
        Выходные параметры:
        целое число хранящее идентификатор (неотрицательное число).
        */
        public int getParentTaskId()
        {
            return parentId;
        }

        /*
        Получение пути файла данных этой задачи.
        Выходные параметры:
        строка содержащяя путь к файлу данных.
        */
        public string getDataFilePath()
        {
            return dataFilePath;
        }

        /*
        Получение пути файла результатов этой задачи.
        Выходные параметры:
        строка содержащяя путь к файлу результатов.
        */
        public string getResultFilePath()
        {
            return resultFilePath;
        }

        /*
        Получение имени функции.
        Выходные параметры:
        строка содержащяя имя функции.
        */
        public string getFunctionName()
        {
            return func;
        }

        /*
        Получение сведений о наличии вложенных задач.
        Выходные параметры:
        булева переменная сигнализирующая о существовании вложенных задач.
        */
        public bool hasChilds()
        {
            return hasChildTask;
        }

        /*
        Получение количества вложенных задач
        Выходные параметры:
        целое число хранящее количество вложенных задач (неотрицательное).
        */
        public int getNumberOfChilds()
        {
            return numberOfChilds;
        }

        /*
        Получение количества вложенных решенных задач
        Выходные параметры:
        целое число хранящее количество вложенных решенных задач (неотрицательное).
        */
        public int getNumberOfSolvedChilds()
        {
            return numberOfSolvedChilds;
        }

        /*
        Получение сведений о решенности задачи.
        Выходные параметры:
        булева переменная сигнализирующая о решении задачи.
        */
        public bool isSolved()
        {
            return solved;
        }
    }
}