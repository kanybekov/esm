//пример функции анализа
function f(data)
{
    sum = 0;
    for (i = 0; i < data.length; ++i)
        sum = 0.1 * sum + 0.9 * data[i];
    return sum;
}