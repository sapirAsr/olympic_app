function get_post(){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let feedPost = JSON.parse(this.responseText);
                let listOfPosts = [];    
                let post1 = feedPost[0];  
                $(".panel-body").append("<tr class='select'" + "id='" + post1.Content + "</tr>");
                              
            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Feed", true);
    xhttp.send();  
}

function GetBest() {
    var element = document.getElementById("omer");
    fetch('https://localhost:44328/api/Feed')
        .then(data => element.textContent = data.toString());
}
