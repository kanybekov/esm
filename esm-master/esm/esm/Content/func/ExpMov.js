function f(data, params)
{
	var alpha = params.alpha;
	var beta = 1 - alpha;
	var result = [];
	result[0] = data[0];
	for(i = 1; i < data.length; i++){
		result[i] = alpha * data[i] + beta * result[i-1];
	}
	return result;
}