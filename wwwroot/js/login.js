var userLogin;

function login() {
    var username = document.getElementById('Username').value;
    console.log("blaa");
    sessionStorage.setItem('Username', username);
    var password = document.getElementById('Password').value;
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                console.log(this.responseText);
                userLogin = JSON.parse(this.responseText);
                console.log(userLogin.username);
                console.log(userLogin.password);
                if (userLogin.username != null && userLogin.password) {
                    sessionStorage.setItem('Password', userLogin.password);
                    sessionStorage.setItem('IsAdmin', userLogin.isAdmin);
                    open_home_page();
                }
                else {
                    alert("You need to fill this form.\n Or you can go back to homepage.");
                }


            } else {
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            }
        }
    };
    xhttp.open("GET", "https://localhost:5001/api/Users/login/" + username + "&" + password, true);
    xhttp.send();
}

function signup() {
    var username = document.getElementById('Username').value;
    sessionStorage.setItem('Username', username);
    var password = document.getElementById('Password').value;
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                console.log(this.responseText);
                userLogin = this.responseText;
                if (userLogin) {
                    open_home_page();
                }
                else {
                    alert("Username already exists.\n");
                }


            } else {
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            }
        }
    };
    xhttp.open("POST", "https://localhost:5001/api/Users/sign_up/" + username + "&" + password, true);
    xhttp.send();
}
function open_signup_page() {
    window.location.href = "new_signup.html";
}