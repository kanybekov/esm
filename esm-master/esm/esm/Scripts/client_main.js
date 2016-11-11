var timerId = setTimeout(sendRequest, 15 * 1000);//ждём 15 секунд

function sendRequest()
{
    var xhr = new XMLHttpRequest();
    var params = 'request=1';
    xhr.open('GET', 'BackgroundCheck?' + params, true);
    xhr.onreadystatechange = function ()
    {
        if (xhr.readyState == 4)
        {// запрос завершён
            if (xhr.status == 200)
            {// код 200 - успешно
                var res = xhr.responseText;
                if (res == "ok")
                {
                    timerId = setTimeout(sendRequest, 15 * 1000);//повторно ставим таймер.
                    //Благодаря рекурсии 15 секунд проходит между концом предыдущего вызова и следующего.
                    //А не между началами вызовов.
                }
                else if (res == "task")
                {
                    var taskId = getTask();
                    var func = getFunction();
                    window.location.href = "Calculation?task="+ taskId + "&func=" + func;
                }
            }
            else
            {
                alert('Connection with server has been lost. Please, reload page.');
            }
        }
    };
    xhr.send(null);
}

function getTask()
{
    var xhr = new XMLHttpRequest();
    var params = 'request=2';
    var res = "";
    xhr.open('GET', 'BackgroundCheck?' + params, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {// запрос завершён
            if (xhr.status == 200) {// код 200 - успешно
                res = xhr.responseText;
            }
            else {
                alert('Connection with server has been lost. Please, reload page.');
            }
        }
    };
    xhr.send(null);
    return res;
}

function getFunction() {
    var xhr = new XMLHttpRequest();
    var params = 'request=3';
    var res = "";
    xhr.open('GET', 'BackgroundCheck?' + params, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4) {// запрос завершён
            if (xhr.status == 200) {// код 200 - успешно
                res = xhr.responseText;
            }
            else {
                alert('Connection with server has been lost. Please, reload page.');
            }
        }
    };
    xhr.send(null);
    return res;
}