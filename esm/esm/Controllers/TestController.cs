using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace esm.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ContentResult Test()
        {
            try
            {
                string testPath = Server.MapPath("~/App_Data/test/");
                string log = "";
                string success = " <font color=\"green\">ok</font><br>";
                string fail = " <font color=\"red\">fail</font><br>";
                string test = "Test №";

                #region TaskIO group
                {
                    log += "<h3>Test group TaskIO class</h3><br>";
                                        
                    {
                        log += test + "1";

                        string[] pp = new string[2];
                        pp[0] = "first param";
                        pp[1] = "second param";
                        //
                        Models.TaskIO.fillTaskFile(testPath + "test.txt", 1, pp);
                        //
                        string[] output = System.IO.File.ReadAllLines(testPath + "test.txt");
                        if (output.Length == 4 && output[0] == "1" && output[1] == "" && output[2] == pp[0] && output[3] == pp[1])
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "2";

                        string[] pp = null;
                        //
                        try
                        {
                            Models.TaskIO.fillTaskFile(testPath + "test.txt", 1, pp);
                            log += fail;
                        }
                        catch (Exception )
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "3";

                        string[] pp = new string[1];
                        pp[0] = "param 2";
                        double[] dd = new double[2];
                        dd[0] = 1.0;
                        dd[1] = 2.71;
                        //
                        Models.TaskIO.fillDataFile(testPath + "test.txt", dd, pp);
                        //
                        string[] output = System.IO.File.ReadAllLines(testPath + "test.txt");
                        if(output.Length == 5 && output[0] == "var data = [];" && output[1] == "var params = {};" 
                                && output[2] == "data[0] = 1;" && output[3] == "data[1] = 2.71;" && output[4] == "params.param = 2;")
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "4";

                        string[] pp = null;
                        double[] dd = null;
                        //
                        try
                        {
                            Models.TaskIO.fillDataFile(testPath + "test.txt", dd, pp);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "5";

                        System.IO.File.WriteAllText(testPath + "test.txt", "");
                        int num;
                        double[] inp;
                        string[] pp;
                        //
                        Models.TaskIO.parseInput(testPath + "test.txt", out num, out inp, out pp);
                        //
                        if (num == 0 && inp == null && pp == null)
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "6";

                        System.IO.File.WriteAllText(testPath + "test.txt", "2");
                        int num;
                        double[] inp;
                        string[] pp;
                        //
                        Models.TaskIO.parseInput(testPath + "test.txt", out num, out inp, out pp);
                        //
                        if (num == 0 && inp == null && pp == null)
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "7";

                        System.IO.File.WriteAllText(testPath + "test.txt", "1\n1");
                        int num;
                        double[] inp;
                        string[] pp;
                        //
                        Models.TaskIO.parseInput(testPath + "test.txt", out num, out inp, out pp);
                        //
                        if (num == 1 && inp.Length == 1 && inp[0] == 1 && pp.Length == 0)
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "8";

                        System.IO.File.WriteAllText(testPath + "test.txt", "1\n1");
                        int num;
                        double[] inp;
                        string[] pp;
                        //
                        try
                        {
                            Models.TaskIO.parseInput(null, out num, out inp, out pp);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }                            
                    }
                }
                #endregion

                #region Task group
                {
                    log += "<h3>Test group Task class</h3><br>";

                    {
                        log += test + "1";

                        Models.Task t = new Models.Task();
                        //
                        if (t.getOwnerId() == -1
                                && t.getTaskId() == -1
                                && t.getParentTaskId() == -1
                                && t.getDataFilePath() == ""
                                && t.getFunctionName() == ""
                                && t.getResultFilePath() == ""
                                && t.hasChilds() == false
                                && t.getNumberOfChilds() == 0
                                && t.getNumberOfSolvedChilds() == 0
                                && t.isSolved() == false
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "2";

                        Models.Task t = new Models.Task(1,2,3,"a","b",testPath);
                        //
                        if (t.getOwnerId() == 1
                                && t.getTaskId() == 2
                                && t.getParentTaskId() == 3
                                && t.getDataFilePath() == "a"
                                && t.getFunctionName() == "b"
                                && t.getResultFilePath() == testPath + "/Content/result/2.txt"
                                && t.hasChilds() == false
                                && t.getNumberOfChilds() == 0
                                && t.getNumberOfSolvedChilds() == 0
                                && t.isSolved() == false
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "3";

                        Models.Task t = new Models.Task();
                        t.setChilds(2);
                        //
                        if (t.getOwnerId() == -1
                                && t.getTaskId() == -1
                                && t.getParentTaskId() == -1
                                && t.getDataFilePath() == ""
                                && t.getFunctionName() == ""
                                && t.getResultFilePath() == ""
                                && t.hasChilds() == true
                                && t.getNumberOfChilds() == 2
                                && t.getNumberOfSolvedChilds() == 0
                                && t.isSolved() == false
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "4";

                        Models.Task t = new Models.Task();
                        t.setChilds(2,1);
                        //
                        if (t.getOwnerId() == -1
                                && t.getTaskId() == -1
                                && t.getParentTaskId() == -1
                                && t.getDataFilePath() == ""
                                && t.getFunctionName() == ""
                                && t.getResultFilePath() == ""
                                && t.hasChilds() == true
                                && t.getNumberOfChilds() == 2
                                && t.getNumberOfSolvedChilds() == 1
                                && t.isSolved() == false
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "5";

                        Models.Task t = new Models.Task(1, 2, 3, "a", "b", testPath);
                        t.setAnswer("answer");
                        //
                        if ( System.IO.File.ReadAllText(t.getResultFilePath()) == "answer" )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "6";

                        Models.Task t = new Models.Task();
                        try
                        {
                            t.setAnswer("answer");
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }                            
                    }

                    {
                        log += test + "7";

                        Models.Task t = new Models.Task(1, 2, 3, testPath+"test.txt", "b", testPath);
                        System.IO.File.WriteAllText(testPath + "test.txt", "3\n4\n\n\n");
                        bool fl = t.updateTask("6");
                        //
                        if (fl == false)
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "8";

                        Models.Task t = new Models.Task(1, 2, 3, testPath + "test.txt", "b", testPath);
                        System.IO.File.WriteAllText(testPath + "test.txt", "2\n4\n\n");
                        bool fl = t.updateTask("6");
                        //
                        string[] output = System.IO.File.ReadAllLines(testPath + "/Content/data/2.js");
                        if (fl == true && t.isSolved() && output.Length == 4 && output[0] == "var data = [];" 
                                && output[1] == "var params = {};" && output[2] == "data[0] = 4;" && output[3] == "data[1] = 6;"
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "9";

                        Models.Task t = new Models.Task(1, 2, 3, "", "b", testPath);
                        try
                        {
                            t.updateTask("6");
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }
                }
                #endregion

                #region User group
                {
                    log += "<h3>Test group User class</h3><br>";

                    {
                        log += test + "1";

                        Models.User u = new Models.User(1);

                        if (u.getId() == 1 && u.getLogin() == null && u.getTask() == null
                                && u.hasCurrentTask() == false && u.lastActivityTime == System.DateTime.MinValue
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "2";

                        Models.Task t = new Models.Task();
                        Models.User u = new Models.User(1, "ololo", true, t, DateTime.MaxValue);
                        
                        if (u.getId() == 1 && u.getLogin() == "ololo" && u.getTask() == t
                                && u.hasCurrentTask() == true && u.lastActivityTime == System.DateTime.MaxValue
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "3";

                        Models.User u = new Models.User(1, "ololo", true, null, DateTime.MaxValue);
                        Models.Task t = new Models.Task();
                        u.setTask(t);

                        if (u.getId() == 1 && u.getLogin() == "ololo" && u.getTask() == t
                                && u.hasCurrentTask() == true && u.lastActivityTime == System.DateTime.MaxValue
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "4";

                        Models.User u = new Models.User(1, "ololo", true, null, DateTime.MaxValue);
                        Models.Task t = new Models.Task();
                        u.setTask(t);
                        u.resetTask();

                        if (u.getId() == 1 && u.getLogin() == "ololo" && u.getTask() == null
                                && u.hasCurrentTask() == false && u.lastActivityTime == System.DateTime.MaxValue
                        )
                            log += success;
                        else
                            log += fail;
                    }
                }
                #endregion

                #region DatabaseMediator group
                {
                    log += "<h3>Test group DatabaseMediator class</h3><br>";

                    {
                        log += test + "1";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "\ne\nb\n\na\n");
                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        inp += "4|d|True|2|abc\n";
                        inp += "5|e|False|-1|abc\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User[] u = db.getUsersOnlineWithoutTask();

                        if (u.Length == 2 && u[0].getId() == 1 && u[0].getLogin() == "a" && u[0].hasCurrentTask() == false
                                && u[0].getTask() == null && u[0].lastActivityTime.Equals( DateTime.Parse("15.12.2016 10:33:16")) 
                                && u[1].getId() == 5 && u[1].getLogin() == "e" && u[1].hasCurrentTask() == false 
                                && u[1].getTask() == null && (DateTime.UtcNow - u[1].lastActivityTime).TotalSeconds < 1
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "2";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "");
                        System.IO.File.Delete(testPath + "/App_Data/OnlineUsers.txt");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        try
                        {
                            Models.User[] u = db.getUsersOnlineWithoutTask();
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "3";

                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        inp += "4|d|True|2|abc\n";
                        inp += "5|e|False|-1|abc\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User u = db.getUser(5);

                        if (u.getId() == 5 && u.getLogin() == "e" && u.hasCurrentTask() == false
                                && u.getTask() == null && (DateTime.UtcNow - u.lastActivityTime).TotalSeconds < 1
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "4";

                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        inp += "4|d|True|2|abc\n";
                        inp += "5|e|False|-1|abc\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User u = db.getUser(1);

                        if (u.getId() == 1 && u.getLogin() == "a" && u.hasCurrentTask() == false
                                && u.getTask() == null && u.lastActivityTime.Equals(DateTime.Parse("15.12.2016 10:33:16"))
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "5";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User u = db.getUser(1);

                        if (u.getId() == -1 && u.getLogin() == null && u.hasCurrentTask() == false
                                && u.getTask() == null && u.lastActivityTime.Equals(DateTime.MinValue)
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "6";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");
                        System.IO.File.Delete(testPath + "/App_Data/UserData.txt");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        try
                        {
                            Models.User u = db.getUser(2);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "7";

                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        inp += "4|d|True|2|abc\n";
                        inp += "5|e|False|-1|abc\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User u = db.getUserByLogin("e");

                        if (u.getId() == 5 && u.getLogin() == "e" && u.hasCurrentTask() == false
                                && u.getTask() == null && (DateTime.UtcNow - u.lastActivityTime).TotalSeconds < 1
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "8";

                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        inp += "4|d|True|2|abc\n";
                        inp += "5|e|False|-1|abc\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User u = db.getUserByLogin("a");

                        if (u.getId() == 1 && u.getLogin() == "a" && u.hasCurrentTask() == false
                                && u.getTask() == null && u.lastActivityTime.Equals(DateTime.Parse("15.12.2016 10:33:16"))
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "9";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User u = db.getUserByLogin("qwe");

                        if (u.getId() == -1 && u.getLogin() == null && u.hasCurrentTask() == false
                                && u.getTask() == null && u.lastActivityTime.Equals(DateTime.MinValue)
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "10";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");
                        System.IO.File.Delete(testPath + "/App_Data/UserData.txt");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        try
                        {
                            Models.User u = db.getUserByLogin("a");
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "11";

                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User u = db.getUserByLogin("a");
                        Models.Task t = new Models.Task();
                        u.setTask(t);
                        db.updateUser(u);

                        string[] output = System.IO.File.ReadAllLines(testPath + "/App_Data/UserData.txt");
                        if (output.Length == 2 && output[0] == "1|a|True|-1|15.12.2016 10:33:16" && output[1] == "")
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "12";

                        string inp = "\n1|a|False|-1|15.12.2016 14:33:17\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User u = db.getUserByLogin("a");
                        u.lastActivityTime = DateTime.Parse("15.12.2016 10:33:16");
                        db.updateUser(u);

                        string[] output = System.IO.File.ReadAllLines(testPath + "/App_Data/UserData.txt");
                        if (output.Length == 2 && output[0] == "1|a|False|-1|15.12.2016 10:33:16" && output[1] == "")
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "13";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User u = new Models.User(1,null,false,null, DateTime.Parse("15.12.2016 10:33:16"));
                        try
                        {
                            db.updateUser(u);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }                                          
                    }

                    {
                        log += test + "14";

                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        if (db.getUserLastActivity(1).Equals(DateTime.Parse("15.12.2016 10:33:16")) )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "15";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");
                        System.IO.File.Delete(testPath + "/App_Data/UserData.txt");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        try
                        {
                            db.getUserLastActivity(2);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "16";

                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        if (db.getUserLastActivity("a").Equals(DateTime.Parse("15.12.2016 10:33:16")))
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "17";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");
                        System.IO.File.Delete(testPath + "/App_Data/UserData.txt");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        try
                        {
                            db.getUserLastActivity("b");
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "18";

                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        db.setUserLastActivity(1, DateTime.Parse("15.12.2016 10:33:25"));

                        if (db.getUserLastActivity(1).Equals(DateTime.Parse("15.12.2016 10:33:25")))
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "19";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");
                        System.IO.File.Delete(testPath + "/App_Data/UserData.txt");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        try
                        {
                            db.setUserLastActivity(2, DateTime.Parse("15.12.2016 10:33:25"));
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "20";

                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        db.setUserLastActivity("a", DateTime.Parse("15.12.2016 10:33:25"));

                        if (db.getUserLastActivity("a").Equals(DateTime.Parse("15.12.2016 10:33:25")))
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "21";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");
                        System.IO.File.Delete(testPath + "/App_Data/UserData.txt");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        try
                        {
                            db.setUserLastActivity("b", DateTime.Parse("15.12.2016 10:33:25"));
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "22";

                        string inp = "\n1|a|True|-1|"+DateTime.UtcNow.AddMinutes(-3).ToString()+"\n";
                        inp += "\n2|aa|True|-1|" + DateTime.UtcNow.AddMinutes(-1).ToString() + "\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.User[] u = db.getUnactiveUsersWithTask().ToArray();

                        if (u.Length == 1 && u[0].getId() == 1 && u[0].getLogin() == "a" && u[0].getTask() == null
                                && u[0].hasCurrentTask() == true && (DateTime.UtcNow - u[0].lastActivityTime).TotalMinutes > 2)
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "23";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");
                        System.IO.File.Delete(testPath + "/App_Data/UserData.txt");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        try
                        {
                            db.getUnactiveUsersWithTask();
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "24";

                        string inp = "\n1|a|False|-1|" + DateTime.UtcNow.AddMinutes(-3).ToString() + "\n";
                        inp += "\n2|aa|True|1|" + DateTime.UtcNow.AddMinutes(-1).ToString() + "\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);
                        Models.Task t = new Models.Task(2, 1, -1, "", "", testPath);               
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        db.saveTask(t);

                        db.createUser("b");

                        Models.User u = db.getUserByLogin("b");
                        if (u.getId() == 3 && u.getLogin() == "b" && u.getTask() == null
                                && u.hasCurrentTask() == false && (DateTime.UtcNow - u.lastActivityTime).TotalSeconds < 1)
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "25";

                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", "");
                        System.IO.File.Delete(testPath + "/App_Data/UserData.txt");

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        try
                        {
                            db.createUser("a");
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "26";

                        System.IO.File.WriteAllText(testPath + "/App_Data/counter.txt", "10");
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        if (db.getFreeTaskId() == 10)
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "27";

                        System.IO.File.WriteAllText(testPath + "/App_Data/counter.txt", "");
                        System.IO.File.Delete(testPath + "/App_Data/counter.txt");
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        try
                        {
                            db.getFreeTaskId();
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "28";

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.Task t = new Models.Task(1,1,2,"a","n",testPath);

                        db.saveTask(t);

                        Models.Task t2 = db.loadTask(1);
                        if (t.getDataFilePath() == t2.getDataFilePath() && t.getFunctionName() == t2.getFunctionName() && t.getNumberOfChilds() == t2.getNumberOfChilds()
                                && t.getNumberOfSolvedChilds() == t2.getNumberOfSolvedChilds() && t.getOwnerId() == t2.getOwnerId() && t.getParentTaskId() == t2.getParentTaskId()
                                && t.getResultFilePath() == t2.getResultFilePath() && t.getTaskId() == t2.getTaskId() && t.hasChilds() == t2.hasChilds() && t.isSolved() == t2.isSolved()
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "29";

                        int num = System.IO.Directory.GetFiles(testPath + "/App_Data/task/").Count();
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                     
                        db.saveTask(null);

                        if (num == System.IO.Directory.GetFiles(testPath + "/App_Data/task/").Count())
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "30";

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        System.IO.Stream fStream = new System.IO.FileStream(testPath + "/App_Data/task/1.bin", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                        Models.Task t = new Models.Task(1, 1, 2, "", "", "");

                        try
                        {
                            db.saveTask(t);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                        fStream.Close();
                    }

                    {
                        log += test + "31";

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.Task t = new Models.Task(1, 1, 2, "a", "n", testPath);
                        db.saveTask(t);

                        Models.Task t2 = db.loadTask(1);

                        if (t.getDataFilePath() == t2.getDataFilePath() && t.getFunctionName() == t2.getFunctionName() && t.getNumberOfChilds() == t2.getNumberOfChilds()
                                && t.getNumberOfSolvedChilds() == t2.getNumberOfSolvedChilds() && t.getOwnerId() == t2.getOwnerId() && t.getParentTaskId() == t2.getParentTaskId()
                                && t.getResultFilePath() == t2.getResultFilePath() && t.getTaskId() == t2.getTaskId() && t.hasChilds() == t2.hasChilds() && t.isSolved() == t2.isSolved()
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "32";

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);

                        if (db.loadTask(-1) == null)
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "33";

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        System.IO.Stream fStream = new System.IO.FileStream(testPath + "/App_Data/task/1.bin", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                        Models.Task t = new Models.Task(1, 1, 2, "", "", "");

                        try
                        {
                            db.loadTask(1);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                        fStream.Close();
                    }

                    {
                        log += test + "34";

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        System.IO.File.WriteAllText(testPath + "/App_Data/user_task.txt", "");

                        db.setUserTask(1, 1);

                        string[] output = System.IO.File.ReadAllLines(testPath + "/App_Data/user_task.txt");
                        if (output.Length == 1 && output[0] == "1 1")
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "35";

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        System.IO.Stream fStream = new System.IO.FileStream(testPath + "/App_Data/user_task.txt", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

                        try
                        {
                            db.setUserTask(1, 1);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                        fStream.Close();
                    }

                    {
                        log += test + "36";

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        System.IO.File.WriteAllText(testPath + "/App_Data/user_task.txt", "\n1 -1\n2 3");

                        Models.Task[] t = db.getUserTasks(1);

                        if (t.Length == 1 && t[0] == null)
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "37";

                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        System.IO.Stream fStream = new System.IO.FileStream(testPath + "/App_Data/user_task.txt", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

                        try
                        {
                            db.getUserTasks(1);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                        fStream.Close();
                    }
                }
                #endregion

                #region Scheduler group
                {
                    log += "<h3>Test group Scheduler class</h3><br>";

                    {
                        log += test + "1";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "a\nc");
                        string inp = "\n1|a|False|-1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|-1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);
                        System.IO.File.WriteAllText(testPath + "test.txt", "2\n10\n11\nalpha 1\n");
                        System.IO.File.WriteAllText(testPath + "/App_Data/counter.txt", "2");
                        Models.Scheduler s = new Models.Scheduler(testPath);

                        bool fl = s.createTask(1, testPath + "test.txt", "tt");

                        string[] output1 = System.IO.File.ReadAllLines(testPath + "/Content/task/2.js");
                        string[] output2 = System.IO.File.ReadAllLines(testPath + "/Content/data/3.js");
                        string[] output3 = System.IO.File.ReadAllLines(testPath + "/Content/data/4.js");
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.Task t1 = db.getUser(1).getTask();
                        Models.Task t2 = db.getUser(3).getTask();
                        if (fl == true && output1.Length == 4 && output1[0] == "2" && output1[1] == ""
                                && output1[2] == "" && output1[3] == "alpha 1" && output2.Length == 4
                                && output2[0] == "var data = [];" && output2[1] == "var params = {};" && output2[2] == "data[0] = 10;" && output2[3] == "params.alpha = 1;"
                                && output3.Length == 4 && output3[0] == "var data = [];" && output3[1] == "var params = {};" && output3[2] == "data[0] = 11;" 
                                && output3[3] == "params.alpha = 1;" && t1.getTaskId() == 3 && t2.getTaskId() == 4
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "2";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "a\nc");
                        string inp = "\n1|a|True|-1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|-1|15.12.2016 10:33:16\n";
                        inp += "3|c|True|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);
                        System.IO.File.WriteAllText(testPath + "test.txt", "2\n10\n11\nalpha 1\n");
                        System.IO.File.WriteAllText(testPath + "/App_Data/counter.txt", "2");
                        Models.Scheduler s = new Models.Scheduler(testPath);
                        int num1 = System.IO.Directory.GetFiles(testPath + "/Content/task/").Count();
                        int num2 = System.IO.Directory.GetFiles(testPath + "/Content/data/").Count();

                        bool fl = s.createTask(1, testPath + "test.txt", "tt");

                        if (fl == false && num1 == System.IO.Directory.GetFiles(testPath + "/Content/task/").Count()
                                && num2 == System.IO.Directory.GetFiles(testPath + "/Content/data/").Count()
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "3";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "a\nc");
                        string inp = "\n1|a|True|-1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|-1|15.12.2016 10:33:16\n";
                        inp += "3|c|True|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);
                        System.IO.File.WriteAllText(testPath + "test.txt", "2\n10\n11\nalpha 1\n");
                        System.IO.File.WriteAllText(testPath + "/App_Data/counter.txt", "2");

                        System.IO.Stream fStream = new System.IO.FileStream(testPath + "/App_Data/counter.txt", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                        Models.Scheduler s = new Models.Scheduler(testPath);

                        try
                        {
                            s.createTask(1, testPath + "test.txt", "tt");
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                        fStream.Close();                            
                    }

                    {
                        log += test + "4";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "a\nc");
                        string inp = "\n1|a|True|1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|-1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.Task t = new Models.Task(1, 1, -1, "", "", testPath);
                        db.saveTask(t);
                        Models.Scheduler s = new Models.Scheduler(testPath);

                        bool fl = s.resetTask(1);

                        Models.User u1 = db.getUser(1);
                        Models.User u2 = db.getUser(3);
                        Models.Task t2 = u2.getTask();
                        if (fl == true && u1.getTask() == null && u1.hasCurrentTask() == false && t.getDataFilePath() == t2.getDataFilePath() 
                                && t.getFunctionName() == t2.getFunctionName() && t.getNumberOfChilds() == t2.getNumberOfChilds() && t.getNumberOfSolvedChilds() == t2.getNumberOfSolvedChilds() 
                                && t.getOwnerId() == t2.getOwnerId() && t.getParentTaskId() == t2.getParentTaskId() && t.getResultFilePath() == t2.getResultFilePath() 
                                && t.getTaskId() == t2.getTaskId() && t.hasChilds() == t2.hasChilds() && t.isSolved() == t2.isSolved() && u2.hasCurrentTask() == true
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "5";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "");
                        string inp = "\n1|a|True|1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|-1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.Task t = new Models.Task(1, 1, -1, "", "", testPath);
                        db.saveTask(t);
                        Models.Scheduler s = new Models.Scheduler(testPath);

                        bool fl = s.resetTask(1);

                        Models.User u1 = db.getUser(1);
                        Models.User u2 = db.getUser(3);
                        Models.Task t2 = u1.getTask();
                        if (fl == false && u2.getTask() == null && u2.hasCurrentTask() == false && t.getDataFilePath() == t2.getDataFilePath()
                                && t.getFunctionName() == t2.getFunctionName() && t.getNumberOfChilds() == t2.getNumberOfChilds() && t.getNumberOfSolvedChilds() == t2.getNumberOfSolvedChilds()
                                && t.getOwnerId() == t2.getOwnerId() && t.getParentTaskId() == t2.getParentTaskId() && t.getResultFilePath() == t2.getResultFilePath()
                                && t.getTaskId() == t2.getTaskId() && t.hasChilds() == t2.hasChilds() && t.isSolved() == t2.isSolved() && u1.hasCurrentTask() == true
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "6";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "a\nc");
                        System.IO.File.Delete(testPath + "/App_Data/OnlineUsers.txt");
                        string inp = "\n1|a|True|1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|-1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.Task t = new Models.Task(1, 1, -1, "", "", testPath);
                        db.saveTask(t);
                        Models.Scheduler s = new Models.Scheduler(testPath);

                        
                        try
                        {
                            s.resetTask(1);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }

                    {
                        log += test + "7";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "a\nc");
                        string inp = "\n1|a|True|-1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|-1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.Task t = new Models.Task(1, 1, -1, "", "", testPath);
                        db.saveTask(t);
                        Models.Scheduler s = new Models.Scheduler(testPath);

                        bool fl = s.setTask(t);

                        Models.User u = db.getUser(3);
                        Models.Task t2 = u.getTask();
                        if (fl == true && t.getDataFilePath() == t2.getDataFilePath() && t.getFunctionName() == t2.getFunctionName() && t.getNumberOfChilds() == t2.getNumberOfChilds() 
                                && t.getNumberOfSolvedChilds() == t2.getNumberOfSolvedChilds() && t.getOwnerId() == t2.getOwnerId() && t.getParentTaskId() == t2.getParentTaskId() 
                                && t.getResultFilePath() == t2.getResultFilePath() && t.getTaskId() == t2.getTaskId() && t.hasChilds() == t2.hasChilds() 
                                && t.isSolved() == t2.isSolved() && u.hasCurrentTask() == true
                        )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "8";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "");
                        string inp = "\n1|a|True|-1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|-1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.Task t = new Models.Task(1, 1, -1, "", "", testPath);
                        db.saveTask(t);
                        Models.Scheduler s = new Models.Scheduler(testPath);

                        bool fl = s.setTask(t);

                        Models.User u = db.getUser(3);
                        if (fl == false && u.getTask() == null && u.hasCurrentTask() == false )
                            log += success;
                        else
                            log += fail;
                    }

                    {
                        log += test + "9";

                        System.IO.File.WriteAllText(testPath + "/App_Data/OnlineUsers.txt", "a\nc");
                        System.IO.File.Delete(testPath + "/App_Data/OnlineUsers.txt");
                        string inp = "\n1|a|True|1|15.12.2016 10:33:16\n";
                        inp += "2|b|True|-1|15.12.2016 10:33:16\n";
                        inp += "3|c|False|-1|15.12.2016 10:33:16\n";
                        System.IO.File.WriteAllText(testPath + "/App_Data/UserData.txt", inp);
                        Models.DatabaseMediator db = new Models.DatabaseMediator(testPath);
                        Models.Task t = new Models.Task(1, 1, -1, "", "", testPath);
                        db.saveTask(t);
                        Models.Scheduler s = new Models.Scheduler(testPath);


                        try
                        {
                            s.setTask(t);
                            log += fail;
                        }
                        catch (Exception)
                        {
                            log += success;
                        }
                    }
                }
                #endregion

                return Content(log);
            }
            catch (Exception e)
            {
                return Content("Testing function failed<br>" + e.Message);
            }
        }
    }
}