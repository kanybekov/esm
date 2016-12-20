using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace esm
{
    public class BackgroundHub : Hub
    {
        /*
        Метод возвращающий идентификатор задачи поставленной данному пользователю.
        Выходные данные:
        строка с идентификатором задачи. Если задача не поставлена, строка равна -1.
        */
        public string getTask()
        {
            Controllers.HomeController hc = new Controllers.HomeController();
            string ans = hc.getTask(Context.User.Identity.Name);
            if (ans == null)
                return "-1";
            else
                return ans;
        }

        /*
        Метод возвращающий имя метода решения задачи поставленной данному пользователю.
        Выходные данные:
        строка с именам функции. Если задача не поставлена, строка равна -1.
        */
        public string getFunc()
        {
            Controllers.HomeController hc = new Controllers.HomeController();
            string ans = hc.getFunc(Context.User.Identity.Name);
            if (ans == null)
                return "-1";
            else
                return ans;
        }

        /*
        Метод ставящий задачу пользователя,если необходимо, при установке соединения. 
        */
        public override Task OnConnected()
        {
            Controllers.HomeController hc = new Controllers.HomeController();
            string ans = hc.getUserIdWithTask(Context.User.Identity.Name);
            if (ans != null)
                Clients.Caller.broadcast(ans);
            return base.OnConnected();
        }

        /*
        Метод ставящий задачу пользователя,если необходимо, при востановлении соединения. 
        */
        public override Task OnReconnected()
        {
            Controllers.HomeController hc = new Controllers.HomeController();
            string ans = hc.getUserIdWithTask(Context.User.Identity.Name);
            if (ans != null)
                Clients.Caller.broadcast(ans);
            return base.OnReconnected();
        }
    }
}