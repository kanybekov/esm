var conn = $.connection.backgroundHub;
var id = $.cookie("user").substring(3);
var task = "-1";
var func = "-1";

conn.client.broadcast =
    function (user)
    {
        if (id == user)
        {
            conn.server.getTask().done(
                function (res)
                {
                    task = res;
                    conn.server.getFunc().done(
                        function (res)
                        {
                            func = res;
                            if(task != "-1" && func != "-1")
                            {
                                window.location.href = "Calculation?task=" + task + "&func=" + func;
                            }
                        }
                    );
                }
            );     
        }
    };

$.connection.hub.start().done(
    function ()
    {
    }
);