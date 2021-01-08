function delete_user() {
    // client needs to provide the username and if its admin adding it in the end of the url by &
    var username = sessionStorage.getItem('Username');
    var isadmin = sessionStorage.getItem('IsAdmin');
    var temp = "";
    if (isadmin) {
        temp = "&";
    }
    fetch("https://localhost:44328/api/Users/delete/" + username + temp, {
        method: 'DELETE',
    });
    open_first_page();
}

function submit_changes() {
    window.alert("Are you sure you want to save changes?");
}

function show_username() {
    var username_place = document.getElementById("Username");
    var cur_username = sessionStorage.getItem('Username');
    username_place.textContent = cur_username;
}

function update_passord() {
    var name = sessionStorage.getItem('Username');
    var new_password = document.getElementById("Password").value;
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Password has changed successfully");
                document.getElementById("Password").value = "";
                console.log(this.responseText);
            } else {
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            }
        }
    };
    xhttp.open("POST", "https://localhost:44328/api/Users/change_password/" + name + "&" + new_password, true);
    xhttp.send();
}

function show_admins() {
    var name = sessionStorage.getItem('Username');
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let adminlist = JSON.parse(this.responseText);
                if (adminlist.length == 0) {
                    var str = "<div>" + "You don't have admin permission"+ "<br/>"+"go to Tests tab" + "</div> <br/>";
                    $("#admin_list").append(str);
                }
                for (i = 0; i < adminlist.length; i++) {
                    var str = "<li>" + adminlist[i] + "</li> <br/>";
                    $("#admin_list").append(str);
                }
            } else {
                    console.log("Error", xhttp.statusText);
                    alert(xhttp.statusText);
            }
        }
    };
        xhttp.open("GET", "https://localhost:44328/api/Users/adminlist/" + name, true);
    xhttp.send();
}