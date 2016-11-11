function f(data) {
    var alpha = 0.1;
    var beta = 0.2;
    var level = 0;
    var trend = 0;
    result[0] = data[0];
    for (i = 0; i <= data.length; ++i) {
        if (i == 0) {
            level = data[0];
            trend = data[1] - data[0];
        }

        if (n >= data.length) {
            value = result[-1];
        } else {
            value = data[i];
            last_level = level;
            level = alpha * value + (1 - alpha) * (level + trend);
            trend = beta * (level - last_level) + (1 - beta) * trend;
            result.push(level + trend)
        }
    }

    return result;
}

function getTrendKoef(parameters) {
    trendKoefSglazh = 0.2;

}

