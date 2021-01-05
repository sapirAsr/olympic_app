function get_sports(){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let sports = JSON.parse(this.responseText);
                console.log(sports);  
                for (i = 0; i < sports.length; i++) {
                    var x = "<a class='dropdown-item' href='#' onclick='getTallestAthlete(";
                    //var y = '"' + sports[i] + '"';
                    var z = ")'>" + sports[i] + "</a>";
                    $(".dropdown-menu").append(x+z);     
                }

            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Search/gameslist", true);
    xhttp.send();  
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
                $("#answer").append("<p>" + best_athlete+ "</p>");     
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