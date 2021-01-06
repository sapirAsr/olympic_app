function delete_user() {
    // client needs to provide the username and if its admin adding it in the end of the url
    fetch("https://localhost:44328/api/Users/delete/sapir&", {
        method: 'DELETE',
    });

}

function submit_changes() {
    window.alert("Are you sure you want to save changes?");
}

function show_username() {
    var username_place = document.getElementById("Username");
    var cur_username = sessionStorage.getItem('Username');
    username_place.textContent = cur_username;
}
