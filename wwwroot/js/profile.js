function delete_user() {
    // client needs to provide the username and if its admin adding it in the end of the url by &
    var username = sessionStorage.getItem('Username');
    var isadmin = sessionStorage.getItem('IsAdmin');
    var temp = "";
    if (isadmin) {
        temp = "&";
    }
    fetch("https://localhost:5001/api/Users/delete/" + username + temp, {
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
