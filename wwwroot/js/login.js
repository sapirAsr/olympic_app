var userLogin;

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
    var password = document.getElementById('Password').value;
    if(alphanumeric(username) && alphanumeric(password)) {
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
                    console.log("Error", xhttp.statusText);
                    alert(xhttp.statusText);
                }
            }
        };
        xhttp.open("POST", "https://localhost:5001/api/Users/sign_up/" + username + "&" + password, true);
        xhttp.send();
    }
}

//Function to check letters and numbers
function alphanumeric(inputtxt)
{
    var letterNumber = /^[0-9a-zA-Z]+$/;
    if(inputtxt.match(letterNumber)) {
        return true;
    }
    else
    { 
        alert('Please input alphanumeric characters only');
        return false; 
    }
  }
  
