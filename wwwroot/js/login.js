
var userLogin;

function login() {
    var username = document.getElementById('Username').value;
    var password = document.getElementById('Password').value;
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                console.log(this.responseText);
                userLogin = JSON.parse(this.responseText);
                open_home_page();
                
            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Users/login/" + username + "&" + password, true);
    xhttp.send();
}
