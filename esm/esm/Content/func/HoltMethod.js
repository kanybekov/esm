function f(data, params) {
    var alpha = 0.2;
	var beta = 0.3;
	if (!(params == undefined && params.alpha == undefined && params.beta == undefined)) {
	    if (params.alpha < 0 || params.alpha > 1 || params.beta < 0 || params.beta > 1) {
	        alert("Some params are out of range. Used default params: alpha = " + alpha + ", beta = " + beta);
	    }
	    else {
	        if (!(params.alpha == undefined  && params.beta == undefined)) {
	            alpha = params.alpha;
	            beta = params.beta;
	        }
	        else {
	            alert("Some params fields are undefined. Used default params: alpha = " + alpha + ", beta = " + beta);
	        }
	    }
	}
	else {
	    alert("Params are undefined. Used default params: alpha = " + alpha + ", beta = " + beta);
	}
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

