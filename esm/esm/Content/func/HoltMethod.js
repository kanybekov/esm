function f(data, params) {
    var alpha = params.alpha;
    var beta = params.beta;
    var level = 0;
    var trend = 0;
    var result = [];
    result[0] = data[0];
    for (i = 0; i <= data.length; ++i) {
        if (i === 0) {
            level = data[0];
            trend = data[1] - data[0];
        }
        var value;
        if (i >= data.length) {
            value = result[-1];
        } else {
            value = data[i];
            var lastLevel = level;
            level = alpha * value + (1 - alpha) * (level + trend);
            trend = beta * (level - lastLevel) + (1 - beta) * trend;
            result.push(level + trend);
        }
    }

    return result[data.length - 1];
}

function getTrendKoef(parameters) {
    var trendKoefSglazh = 0.2;
}

