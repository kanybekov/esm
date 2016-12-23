function f(data, params) {
    var alpha = 0.2;
    var beta = 0.8;
    if (params != undefined){
        if (params.alpha < 0 || params.alpha > 1) {
            alert("Some params are out of range. Used default params: alpha = " + alpha + ", beta = " + beta);
        }
        else {
            if (params.alpha != undefined) {
                alpha = params.alpha;
                beta = 1 - alpha;
            }
            else {
                alert("Params.alpha is undefined. Used default params: alpha = " + alpha + ", beta = " + beta);
            }
        }
    }
    else {
        alert("Params are undefined. Used default params: alpha = " + alpha + ", beta = " + beta);
    }
	var result = [];
	result[0] = data[0];
	for(i = 1; i < data.length; i++){
		result[i] = alpha * data[i] + beta * result[i-1];
	}
	return result[data.length - 1];
}