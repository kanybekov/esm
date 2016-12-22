function makeCalculation(taskId)
{
    var res = f(data, params);
    var xhr = new XMLHttpRequest();
    var params2 = 'task=' + taskId + '&result=' + res;
    xhr.open('GET', 'TransferOut?' + params2, false);
    xhr.onreadystatechange = function ()
    {
        if (xhr.readyState == 4)
        {// запрос завершён
            if (xhr.status == 200)
            {// код 200 - успешно
                alert('Calculation has been completed.');
            }
            else
            {
                alert('Connection with server has been lost. Please, reload page.');
            }
        }
    };
    xhr.send(null);
    window.location.href = "Master";
}