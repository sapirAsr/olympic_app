function start(){
    // Click on the first tablink on load
    document.getElementsByClassName("tablink")[0].click();
    get_sports();
    getList("Height", "heightslist");
    getList("Weight", "weightslist");
    getList("Team", "teamslist");
    getList("Select_Game", "gameslist");
    getList("Birth_year", "yearslist");

}


function getList(idName, url){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let games = JSON.parse(this.responseText);
                console.log(games);  
                for (i = 0; i < games.length; i++) {
                    var x =  "<option value=";
                    var y = '"' + games[i] + '"';
                    var z = ">" + games[i] + "</option>"
                    var res = x+y+z;
                    $("#"+idName).append(res);     
                }

            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Search/"+url, true);
    xhttp.send();  
}

function get_sports(){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let sports = JSON.parse(this.responseText);
                console.log(sports);  
                for (i = 0; i < sports.length; i++) {
                    var x =  "<option value=";
                    var y = '"' + sports[i] + '"';
                    var z = ">" + sports[i] + "</option>"
                    var res = x+y+z;
                    $(".Select_Sport").append(res);     
                }

            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Search/sportslist", true);
    xhttp.send();  
}

function resetSelects() {
    $("select").each(function() { this.selectedIndex = 0 });
    var x = document.getElementsByClassName("answer");
    for (i = 0; i < x.length; i++) {
      x[i].style.display = "none";
    }
}

function findBestAthlete(){
    var e = document.getElementById("BestAthleteSelect");
    var selectedSport = e.value;
    if (selectedSport != "base"){
        getBestAthlete(selectedSport);
    } else{
        let answer = document.getElementById("answer_athlete");
        answer.innerHTML = ""; 
        alert("Please choose sport."); 
    }
}

function findLocation(){
    var e = document.getElementById("Select_Game");
    var selectedGame = e.value;
    if (selectedGame != "base"){
        getLocation(selectedGame);
    } else{
        let answer = document.getElementById("answer_location");
        answer.innerHTML = ""; 
        alert("Please choose game."); 
 
    }
}

function getBestAthlete(sport){
    var sportstr = sport + '';
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                console.log(this.responseText);
                let best_athlete = this.responseText;
                console.log(best_athlete);
                let answer = document.getElementById("answer_athlete");
                answer.innerHTML = best_athlete;
                answer.style.display = "inline-block";
  
            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Search/best_athlete/" + sportstr, true);
    xhttp.send(); 
}
function getLocation(game){
    game = game.replace(' ', '');
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let location = JSON.parse(this.responseText);
                console.log(location);  
                let answer = document.getElementById("answer_location");
                answer.innerHTML = location[1] + " , " + location[0];
                answer.style.display = "inline-block";
                } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Search/location/" + game, true);
    xhttp.send(); 
}


function getThefact(sport, fact){
    var e = document.getElementById("Fact");
    // for example athletes or Games
    var selectedFact = e.value;
    var res = selectedFact.split(" ");
    e = document.getElementById("MostSport");
    var selectedSport = e.value;
    var str = selectedSport + "&"+ res[0] + "&"+ res[1];
    console.log(selectedFact);
    console.log(selectedSport);
    if (selectedFact == "" || selectedSport == ""){
        alert("Please choose all fields."); 
    }
    else{
    getTheAnswerMost(str);
    }
}

function getTheAnswerMost(str){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                console.log(this.responseText);
                let result = JSON.parse(this.responseText);
                console.log(result);
                let answer = document.getElementById("answer_most");
                answer.innerHTML = result[0] +", " + result[1];
                answer.style.display = "inline-block";
  
            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Search/the_most/" + str, true);
    xhttp.send(); 
}


function getAtrToSearch(){
    //validation_filter();
    var atr = [];
    var e = document.getElementById("Search");
    var selectVal = e.value;
    atr.push({
        key:   "Search",
        value: selectVal
    });
    e = document.getElementById("Sex");
    var selectedSex = e.value;
    if(selectedSex != ""){
        atr.push({
            key:   "Sex",
            value:  selectedSex
        });
    }
    e = document.getElementById("Team");
    var selectedTeam = e.value;
    if(selectedTeam != ""){
        atr.push({
            key:   "Team",
            value: "='" + selectedTeam + "'"
        });
    }
    var b = document.getElementById("parameterHeight");
    e = document.getElementById("Height");
    var selectedHeight = e.value;
    if (selectedHeight != ""){
        atr.push({
            key:   "Height",
            value: b.value + selectedHeight
        }); 
    }
    b = document.getElementById("parameterWeight");
    e = document.getElementById("Weight");
    var selectedWeight = e.value;
    if (selectedWeight != ""){
        atr.push({
            key:   "Weight",
            value: b.value + selectedWeight
        }); 
    }
    b = document.getElementById("parameterBirth");
    e = document.getElementById("Birth_year");
    var selectedBirth_year = e.value;
    if (selectedBirth_year != ""){
        atr.push({
            key:   "Birth_year",
            value: b.value + selectedBirth_year
        }); 
    }
    e = document.getElementById("FilterSport");
    var selectedSport = e.value;
    if (selectedSport != ""){
        atr.push({
            key:   "Sport",
            value: "='" +selectedSport + "'"
        });
    }
    console.log(atr);
    //check if there are values
    filter(atr, "answer_filter");
    let answer = document.getElementById("answer_filter");
    answer.innerHTML = "";  
    
}


function filter(atr,answer){
    //game = game.replace(' ', '');
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let results = JSON.parse(this.responseText);
                //console.log(sports); 
                var flag = 0; 
                for (i = 0; i < results.length; i++) {
                    if (i >= 12) {
                        $("#" + answer).append("<p class='answer_display hide'>" + results[i] + "</p>");
                        flag =1 ;    
                    } 
                    else{
                    $("#" + answer).append("<p class ='answer_display'>" + results[i] + "</p>"); 
                    }    
                }
                // if(flag ==1) {
                //     $("#answer_filter").append('<a href="#" onclick="next()" class="next round">&#8250;</a>');
                // }
                document.getElementById(answer).style.display = "inline-block";
                
            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };
    var str = "https://localhost:5001/api/Search/filter/";
    for (i = 0; i < atr.length; i++) {
        str += atr[i].key;
        str += "-";
        str += atr[i].value;
        str += "&";
    }
    str = str.slice(0, -1);
    console.log(str);     
    xhttp.open("GET", str, true);
    xhttp.send(); 
}

function next() {
    var answers = document.getElementsByClassName("answer_display");
    var hidden_answers = document.getElementsByClassName("hide");
    var check = 0;
    console.log(hidden_answers);
    var arr = Array.prototype.slice.call(hidden_answers);
    console.log(arr);
    for (let index = 0; index < answers.length; index++) {
        if (!arr.includes(answers[index])) {
            answers[index].remove();;
        }
        // if (arr.includes(answers[index]) && check < 5){
        //     answers[index].className = "answer_display";
        //     //answers[index].className = "answer hide";
        //     check += 1;
        // }

    }
}


function getTheMedal(){
    var atr = [];
    var e = document.getElementById("MedalSelect");
    var selectedMedal = e.value;
    atr.push({
        key:   "Medal",
        value: selectedMedal
    });
    e = document.getElementById("MedalSport");
    var selectedSport = e.value;
    atr.push({
        key:   "Sport",
        value: "='" +selectedSport + "'"
    });
    e = document.getElementById("SexMedal");
    var selectedSex = e.value;
    atr.push({
        key:   "Sex",
        value: selectedSex
    });
    if (selectedSport == "" || selectedMedal == ""){
        alert("Please choose all fields."); 
    } 
    else{
        filter(atr, "answer_medal");
        let answer = document.getElementById("answer_medal");
        answer.innerHTML = ""; 
    }
}


function validation_filter(){
    var e = document.getElementById("Search");
    var selectVal = e.value;
    e = document.getElementById("Sex");
    var selectedSex = e.value;
    e = document.getElementById("Team");
    var selectedTeam = e.value;
    var b = document.getElementById("parameterHeight");
    e = document.getElementById("Height");
    var selectedHeight = e.value;
    var parmHeight = b.value;
    b = document.getElementById("parameterWeight");
    e = document.getElementById("Weight");
    var selectedWeight = e.value;
    var parmWeight = b.value;
    b = document.getElementById("parameterBirth");
    e = document.getElementById("Birth_year");
    var selectedBirth_year = e.value;
    var parmBirth = b.value;
    e = document.getElementById("FilterSport");
    var selectedSport = e.value;
}

/*
function rightMenu(selectedVal){
    if(selectedVal.value == "Athletes"){
        var str = '<select id="Sex"> <option value="">Choose Sex</option> <option value="=' + "'M'" +"'>Male</option>" + 
                    'option value="='+"'F'"+'">Female</option> </select>' +
                    '<select id="Team"><option value="">Choose Team</option></select><p>that are...</p>' +
                    '<select id="parameterHeight" required><option value="">Choose parameter..</option>' 
        $("#atributes").append(str);
    }
} <option value="=">equal</option> 
<option value=">">grater</option> 
<option value="<">lesser</option> 
</select>
<select id="Height"> 
<option value="">Choose Height</option> 
</select>
<select id="parameterWeight" required>
<option value="">Choose parameter..</option>  
<option value="=">equal</option> 
<option value=">">grater</option> 
<option value="<">lesser</option> 
</select>
<select id="Weight"> 
<option value="">Choose Weight</option> 
</select>
<select id="parameterBirth" required>
<option value="">Choose parameter..</option>  
<option value="=">equal</option> 
<option value=">">grater</option> 
<option value="<">lesser</option> 
</select>
<select id="Birth_year"> 
<option value="">Choose Birth Year</option>
</select>
<select class="Select_Sport" id="FilterSport">
<option value="">Choose Sport</option>
</select>
</div> */





  // Tabs
  function openLink(evt, linkName) {
    var i, x, tablinks;
    x = document.getElementsByClassName("myLink");
    for (i = 0; i < x.length; i++) {
      x[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablink");
    for (i = 0; i < x.length; i++) {
      tablinks[i].className = tablinks[i].className.replace(" w3-red", "");
    }
    document.getElementById(linkName).style.display = "block";
    evt.currentTarget.className += " w3-red";
  }
