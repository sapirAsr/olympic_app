function start(){
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
    //Basketball&Height&ASC
    console.log(str);
    getTheAnswerMost(str);
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


function getTallestAthlete(){
    //game = game.replace(' ', '');
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let tallest = JSON.parse(this.responseText);
                $("#answer").append("<p>" + tallest + "</p>");     
            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Search/the_most/Basketball&Height&ASC", true);
    xhttp.send(); 
}



function getAtrToSearch(){
    var atr = [];
    var e = document.getElementById("Search");
    // for example athletes or Games
    var selectVal = e.value;
    atr.push({
        key:   "Search",
        value: selectVal
    });
    e = document.getElementById("Sex");
    var selectedSex = e.value;
    atr.push({
        key:   "Sex",
        value: selectedSex
    });
    e = document.getElementById("Team");
    var selectedTeam = e.value;
    atr.push({
        key:   "Team",
        value: selectedTeam
    }); 
    e = document.getElementById("Height");
    var selectedHeight = e.value;
    atr.push({
        key:   "Height",
        value: selectedHeight
    }); 
    e = document.getElementById("Weight");
    var selectedWeight = e.value;
    atr.push({
        key:   "Weight",
        value: selectedWeight
    }); 
    e = document.getElementById("Birth_year");
    var selectedBirth_year = e.value;
    atr.push({
        key:   "Birth_year",
        value: selectedBirth_year
    }); 
    e = document.getElementById("FilterSport");
    var selectedSport = e.value;
    atr.push({
        key:   "Sport",
        value: selectedSport
    });
    console.log(atr);
    //check if there are values
    filter(atr);
    let answer = document.getElementById("answer_filter");
    answer.innerHTML = "";  
    
}


function filter(atr){
    //game = game.replace(' ', '');
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let results = JSON.parse(this.responseText);
                //console.log(sports);  
                for (i = 0; i < results.length; i++) {
                    if (i >= 10) {
                        $("#answer_filter").append("<p class='hide'>" + results[i] + "</p>");     
                    } 
                    else{
                    $("#answer_filter").append("<p>" + results[i] + "</p>"); 
                    }    
                }
                document.getElementById("answer_filter").style.display = "inline-block";
            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };
    var str = "https://localhost:5001/api/Search/filter/";
    for (i = 0; i < atr.length; i++) {
        str += atr[i].key;
        str += "=";
        str += atr[i].value;
        str += "&";
    }
    str = str.slice(0, -1);
    console.log(str);     
    xhttp.open("GET", str, true);
    xhttp.send(); 
}

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
  
  // Click on the first tablink on load
  document.getElementsByClassName("tablink")[0].click();