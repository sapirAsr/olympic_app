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
                    var z = " onclick='getBestAthlete("
                    var w = ")'>" + sports[i] + "</option>"
                    var res = x+y+z+y+w;
                    $("#Select_Sport").append(res);     
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

function findBestAthlete(){
    var e = document.getElementById("Select_Sport");
    var selectedSport = e.value;
    if (selectedSport != "base"){
        getBestAthlete(selectedSport);
    } else{
        let answer = document.getElementById("answer");
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
                let answer = document.getElementById("answer");
                answer.innerHTML = best_athlete;  
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
                $("#answer").append("<p>" + location[1] +","+ location[0] + "</p>");     
            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Search/location/" + game, true);
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

function filter(){
    //game = game.replace(' ', '');
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let results = JSON.parse(this.responseText);
                //console.log(sports);  
                for (i = 0; i < results.length; i++) {
                    $("#answer").append("<p>" + results[i] + "</p>");     
                }
            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Search/filter/athletes&Name", true);
    xhttp.send(); 
}



function filter_pressed() {
    var query_result = document.getElementById("result_content");
      var n = document.getElementById("noam");
      n.textContent = "filter questions";
    query_result.textContent = "filter result";
  }
  
  function whos_the_best_pressed(){
     var query_result = document.getElementById("result_content");
    query_result.textContent = "whos the best result";
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