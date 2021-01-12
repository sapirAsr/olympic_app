﻿//delete user function
//send the details of the current user to the server, delete this account and open first page
function delete_user() {
    var username = sessionStorage.getItem('Username');
    var isadmin = sessionStorage.getItem('IsAdmin');
    var temp = "";
    if (isadmin) {
        temp = "&";
    }
    fetch("http://localhost:5001/api/Users/delete/" + username + temp, {
        method: 'DELETE',
    }).catch(function () {
        alert("Connection problem, please try again later.");
    });

    open_first_page();
}

// display the username of the current account on profile page
function show_username() {
    var username_place = document.getElementById("Username");
    var cur_username = sessionStorage.getItem('Username');
    username_place.textContent = cur_username;
}

// get a new password from user
// send to the server the username of the current account and the new password and update it 
function update_passord() {
    var name = sessionStorage.getItem('Username');
    var new_password = document.getElementById("Password").value;
    if (alphanumeric(new_password)) {
        let xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState === 4) {
                if (this.status === 200) {
                    alert("Password has changed successfully");
                    document.getElementById("Password").value = "";
                } else {
                    alert("Connection problem, please try again later.");
                }
            }
        };
        xhttp.open("POST", "http://localhost:5001/api/Users/change_password/" + name + "&" + new_password, true);
        xhttp.send();
    }
}

//Displays the sport fields in which the user has passed the tests
function show_admins() {
    var name = sessionStorage.getItem('Username');
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                $("#admin_list").empty();
                let adminlist = JSON.parse(this.responseText);
                if (adminlist.length == 0) {
                    var str = "<div>" + "You haven't completed any test yet.. " + "<br/>" + "Go to the Tests tab to try your best!" + "</div> <br/>";
                    $("#admin_list").append(str);
                }
                for (i = 0; i < adminlist.length; i++) {
                    var str = "<li>" + adminlist[i] + "</li> <br/>";
                    $("#admin_list").append(str);
                }
            } else {
                alert("Connection problem, please try again later.");
            }
        }
    };
    xhttp.open("GET", "http://localhost:5001/api/Users/adminlist/" + name, true);
    xhttp.send();
}