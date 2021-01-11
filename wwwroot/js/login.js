﻿var userLogin;

// login function
// get username and password from html form
// verify if the username exists in the database and the password is match to this username
// if true- open home page, else- send an alert of wrong details
function login() {
    var username = document.getElementById('Username').value;
    var password = document.getElementById('Password').value;
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                userLogin = JSON.parse(this.responseText);
                if (userLogin.username != null && userLogin.password) {
                    sessionStorage.setItem('Username', username);
                    sessionStorage.setItem('Password', userLogin.password);
                    sessionStorage.setItem('IsAdmin', userLogin.isAdmin);
                    open_home_page();
                }
                else {
                    alert("Wrong username or password.");
                }
            } else {
                alert("Connection problem, please try again later.");
            }
        }
    };
    xhttp.open("GET", "https://localhost:5001/api/Users/login/" + username + "&" + password, true);
    xhttp.send();
}

// sign up function
// get username and password from html form
// check if the username exists in the database
// if not- create a new user and save his details in the database, else- send an alert of an existing username
function signup() {
    var username = document.getElementById('Username').value;
    var password = document.getElementById('Password').value;
    if (alphanumeric(username) && alphanumeric(password)) {
        let xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState === 4) {
                if (this.status === 200) {
                    userLogin = JSON.parse(this.responseText);
                    sessionStorage.setItem('IsAdmin', 'false');
                    if (userLogin.username != null) {
                        sessionStorage.setItem('Username', username);
                        open_home_page();

                    }
                    else {
                        alert("Username already exists.\n");
                    }
                } else {
                    alert("Connection problem, please try again later.");
                }
            }
        };
        xhttp.open("POST", "https://localhost:5001/api/Users/sign_up/" + username + "&" + password, true);
        xhttp.send();
    }
}

//Function to check letters and numbers
function alphanumeric(inputtxt) {
    var letterNumber = /^[0-9a-zA-Z_]+$/;
    if (inputtxt.match(letterNumber)) {
        return true;
    }
    else {
        alert('Please input alphanumeric characters and _ only');
        return false;
    }
}