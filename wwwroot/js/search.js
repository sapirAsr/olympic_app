
// function is called when the body is loaded
function start(){
    // Click on the first tablink on load
    document.getElementsByClassName("tablink")[0].click();
    get_sports();
    getList("Select_Game", "gameslist");
}

// gets an id of an elemet to append the list we get from the server
// and a url that match the request for the server
function getList(idName, url){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let games = JSON.parse(this.responseText);
                for (i = 0; i < games.length; i++) {
                    var x =  "<option value=";
                    var y = '"' + games[i] + '"';
                    var z = ">" + games[i] + "</option>"
                    var res = x+y+z;
                    $("#"+idName).append(res);     
                }
            } else {                   
                alert("Connection problem, please try again later.");
            } 
        }                  
    };     
    xhttp.open("GET", "http://localhost:5001/api/Search/"+url, true);
    xhttp.send();  
}


// append list of sports to thr right element in the html page
function get_sports(){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let sports = JSON.parse(this.responseText);
                for (i = 0; i < sports.length; i++) {
                    var x =  "<option value=";
                    var y = '"' + sports[i] + '"';
                    var z = ">" + sports[i] + "</option>"
                    var res = x+y+z;
                    $(".Select_Sport").append(res);     
                }
            } else {                   
                alert("Connection problem, please try again later.");
            } 
        }           
    };     
    xhttp.open("GET", "http://localhost:5001/api/Search/sportslist", true);
    xhttp.send();  
}

// resets the options that are selected on the page
function resetSelects() {
    $("select").each(function() { this.selectedIndex = 0 });
    var x = document.getElementsByClassName("answer");
    for (i = 0; i < x.length; i++) {
      x[i].style.display = "none";
    }
    var x = document.getElementsByClassName("answer");
    for (i = 0; i < x.length; i++) {
      x[i].style.display = "none";
    }
    x = document.getElementById("dynamicAtr");
    x.style.display = "none";
}

// function display the best athlete on the page
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

// function display the location of a game on the page
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

// get a sport and return the best athlete answer from the server
function getBestAthlete(sport){
    var sportstr = sport + '';
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let best_athlete = this.responseText;
                let answer = document.getElementById("answer_athlete");
                answer.innerHTML = best_athlete;
                answer.style.display = "inline-block";
            } else {                   
                alert("Connection problem, please try again later.");
            } 
        }           
    };     
    xhttp.open("GET", "http://localhost:5001/api/Search/best_athlete/" + sportstr, true);
    xhttp.send(); 
}

// get a game and return the location of the game from the server
function getLocation(game){
    game = game.replace(' ', '');
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let location = JSON.parse(this.responseText);
                let answer = document.getElementById("answer_location");
                answer.innerHTML = location[1] + " , " + location[0];
                answer.style.display = "inline-block";
            } else {    
                alert("Connection problem, please try again later.");
            } 
        }           
    };     
    xhttp.open("GET", "http://localhost:5001/api/Search/location/" + game, true);
    xhttp.send(); 
}

// function display the fact answer on the page
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


// function gets the answer from the server of the facts
function getTheAnswerMost(str){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState != 4) {
            document.getElementById('loader_most').style.visibility = "visible"; 
        }if (this.readyState === 4) {
            if (this.status === 200) {
                document.getElementById('loader_most').style.visibility="hidden";
                let result = JSON.parse(this.responseText);
                let answer = document.getElementById("answer_most");
                answer.innerHTML = result[0] +", " + result[1];
                answer.style.display = "inline-block";
  
            } else {
                alert("Connection problem, please try again later.");
            } 
        }           
    };     
    xhttp.open("GET", "http://localhost:5001/api/Search/the_most/" + str, true);
    xhttp.send(); 
}

// gets the selected values of the user and sent it to the filter function
function getAtrToSearch(){
    //validation_filter();
    var atr = {};
    var e = document.getElementById("Search");
    var selectVal = e.value;
    if (selectVal == ""){
        alert("Please select what you want to see.");
        return;
    }
    atr["Search"] = selectVal;
    if (selectVal == "Athletes") {
        e = document.getElementById("Sex");
        var selectedSex = e.value;
        if(selectedSex != ""){
                atr["Sex"] =  selectedSex;
        } else {
            atr["Sex"] =  "";
        }
        e = document.getElementById("Team");
        var selectedTeam = e.value;
        if(selectedTeam != ""){
            atr["Team"] = "='" + selectedTeam + "'";
        } else {
            atr["Team"] =  "";
        }
        var b = document.getElementById("parameterHeight");
        e = document.getElementById("Height");
        var selectedHeight = e.value;
        if (selectedHeight != ""){
                atr["Height"] = b.value + selectedHeight;
        } else {
            atr["Height"] ="";
        }
        b = document.getElementById("parameterWeight");
        e = document.getElementById("Weight");
        var selectedWeight = e.value;
        if (selectedWeight != ""){
            atr["Weight"] = b.value + selectedWeight;
        } else {
            atr["Weight"] = "";
        }
        b = document.getElementById("parameterBirth");
        e = document.getElementById("Birth_year");
        var selectedBirth_year = e.value;
        if (selectedBirth_year != ""){
            atr["Birth_year"] =  b.value + selectedBirth_year;
        } else{
            atr["Birth_year"] = "";
        }
    }
    e = document.getElementById("FilterSport");
    var selectedSport = e.value;
    if (selectedSport != ""){
        atr["Sport"] = "='" +selectedSport + "'";
    } else{
        atr["Sport"] = "";
    }
    var check = validation_filter(atr);
    if(check){
    //check if there are values
        filter(atr, "answer_filter");       
        let answer = document.getElementById("answer_filter");
        answer.innerHTML = "";
    }  
}

// gets the atributes to filter and the id where the answer should be displaied
function filter(atr,answer){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState != 4) {
            document.getElementById('loader').style.visibility = "visible"; 
        }if (this.readyState === 4) {
            if (this.status === 200) {
                document.getElementById('loader').style.visibility="hidden";
                let results = JSON.parse(this.responseText);
                for (i = 0; i < results.length; i++) {
                    $("#" + answer).append("<li class ='answer_display'>" + results[i] + "</li>");   
                }
                document.getElementById(answer).style.display = "inline-block";
                
            } else { 
                document.getElementById('loader').style.visibility="hidden";
                alert("Connection problem, please try again later.");
            } 
        }           
    };
    var str = "http://localhost:5001/api/Search/filter/";
    // set the atributes on the url sent to the server
    for(var key in atr) {
        str += key;
        str += "-";
        str += atr[key];
        str += "&";
    }
    str = str.slice(0, -1);     
    xhttp.open("GET", str, true);
    xhttp.send(); 
}

// gets the selected values from the page and send it to filter to get the answer
function getTheMedal(){
    var atr = {};
    var e = document.getElementById("MedalSelect");
    var selectedMedal = e.value;
    atr["Medal"] = selectedMedal;
    e = document.getElementById("MedalSport");
    var selectedSport = e.value;
    atr["Sport"]= "='" +selectedSport + "'";
    e = document.getElementById("SexMedal");
    var selectedSex = e.value;
    atr["Sex"] = selectedSex;
    if (selectedSport == "" || selectedMedal == ""){
        alert("Please choose all required fields."); 
    } 
    else{
        filter(atr, "answer_medal");
        let answer = document.getElementById("answer_medal");
        answer.innerHTML = ""; 
    }
}

// function checks if the selected values are validate
function validation_filter(atr){
    if(atr["Search"] == "Events" &&  atr["Sport"] == ""){
        alert("Please choose sport");
        return false;
    }
    if(atr["Search"] == "Events" &&  atr["Sport"] != ""){
        return true;
    }
    var parmHeight = document.getElementById("parameterHeight").value;
    if (atr["Search"] == "Athletes" &&  (atr["Height"] != "" && parmHeight == "")){
        alert("Please choose parameter for height");
        return false;
    }
    var parmHeight = document.getElementById("parameterWeight").value;
    if (atr["Search"] == "Athletes" &&  (atr["Weight"] != "" && parmHeight == "")){
        alert("Please choose parameter for weight");
        return false;
    }
    var parmBirth = document.getElementById("parameterBirth").value;
    if (atr["Search"] == "Athletes" &&  (atr["Birth_year"] != "" && parmBirth == "")){
        alert("Please choose parameter for birth year");
        return false;
    }
    //if everything is empty
    if (atr["Search"] == "Athletes" && atr["Height"] == "" &&  atr["Weight"] == "" &&  atr["Birth_year"] == "" &&
          atr["Sport"] == "" && atr["Sex"] == "" && atr["Team"] == ""){
        alert("Please choose at least one parameter");
        return false;
    }
    //else
    return true;    
}

// open the correct menu after select of athletes or events in the filter tab
function rightMenu(selectedVal){
    var e = document.getElementById("dynamicAtr");
    e.style.display = "block";
    var x = document.getElementsByClassName("answer");
    for (i = 0; i < x.length; i++) {
      x[i].style.display = "none";
    }
    e.innerHTML ="";
    var str;
    if(selectedVal.value == "Athletes"){
        str = '<select id="Sex"> <option value="">Choose Sex</option><option value="='+"'M'"+'">Male</option>' + 
                    '<option value="='+"'F'"+'">Female</option></select>' +
                    '<select id="Team"><option value="">Choose Team</option></select><p>that are...</p>' +
                    '<select id="parameterHeight" required><option value="">Choose parameter..</option>' +
                    '<option value="=">equal</option><option value=">">grater</option> <option value="<">lesser</option></select>' +
                    '<select id="Height"> <option value="">Choose Height</option> </select>' +
                    '<select id="parameterWeight" required><option value="">Choose parameter..</option> <option value="=">equal</option>' +
                    '<option value=">">grater</option><option value="<">lesser</option></select>' +
                    '<select id="Weight"><option value="">Choose Weight</option> </select><select id="parameterBirth" required>' +
                    '<option value="">Choose parameter..</option><option value="=">equal</option><option value=">">grater</option>' +
                    '<option value="<">lesser</option></select><select id="Birth_year"><option value="">Choose Birth Year</option>' +
                    '</select><select class="Select_Sport" id="FilterSport"><option value="">Choose Sport</option></select></div>';

        $("#dynamicAtr").append(str); 
        get_sports();
        getList("Height", "heightslist");
        getList("Weight", "weightslist");
        getList("Team", "teamslist");
        getList("Select_Game", "gameslist");
        getList("Birth_year", "yearslist");

    }
    else{
        str = '<select class="Select_Sport" id="FilterSport"><option value="">Choose Sport</option></select></div>';
        $("#dynamicAtr").append(str); 
        get_sports();
    }
} 

// Tabs function
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
    resetSelects();
  }
