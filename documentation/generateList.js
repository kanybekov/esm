function generateList(data){
	for(var k in data){
		olname = k;
		var ol = document.getElementById(k);
		ol.style.display = "none";
		document.getElementById("ModelsDiv").style.display = "none";
		document.getElementById("ControllersDiv").style.display = "none";
		document.getElementById("ViewsDiv").style.display = "none";
		document.getElementById("ClassesDiv").style.display = "none";
		var l = data[k].length;
		for(var i = 0; i < l; i++){
			var q = data[k][i].name;
			var li = document.createElement("LI");
			li.onmouseover = "";
			li.style = "cursor: pointer;";
			li.onmouseout = function(){ this.style.color='black'; };
			li.onmouseover = function(){ this.style.color='yellow'; };
			li.innerHTML = q;
			li.onclick = function (){ generateContent(data, this);};
			ol.appendChild(li);
		}
	}

}

function generateContent(data, qq){
	method = qq.innerHTML
	model = qq.parentNode.id;
	var div = document.getElementById("main");
	div.innerHTML = "";
	var obj = data[model];
	for(var i = 0; i < data[model].length; i++){
		if(data[model][i].name == method){
			obj = data[model][i];
			break;
		}
	}
	var code = document.createElement("code");
	code.innerHTML = obj.code;
	div.appendChild(code);
	div.innerHTML += "<br><br>";
	var table = document.createElement("table");
	table.border = "1";
	table.cellPadding = "7";
	
	var row1 = table.insertRow(0);
	var cell1 = row1.insertCell(0);
	cell1.innerHTML = "Описание";
	var cell2 = row1.insertCell(1);
	cell2.innerHTML = obj.description;
	var row2 = table.insertRow(1);
	var cell3 = row2.insertCell(0);
	cell3.innerHTML = "Входные параметры";
	var cell4 = row2.insertCell(1);
	cell4.innerHTML = obj.input;
	
	var row3 = table.insertRow(2);
	var cell5 = row3.insertCell(0);
	cell5.innerHTML = "Выходные данные";
	var cell6 = row3.insertCell(1);
	cell6.innerHTML = obj.output;
	
	var row4 = table.insertRow(3);
	var cell7 = row4.insertCell(0);
	cell7.innerHTML = "Побочные эффекты";
	var cell8 = row4.insertCell(1);
	cell8.innerHTML = obj.side;
	
	div.appendChild(table);
}




