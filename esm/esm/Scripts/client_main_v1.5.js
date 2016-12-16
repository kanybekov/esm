var conn = $.connection.backgroundHub;
conn.server.getTask();
conn.client.hello = function () { alert("hello"); };

conn.client.broadcast =
    function (user, mess) {
        alert("hello");
        var id = $.session.get("user_id");
        if (id == user)
            alert("succes");
    };