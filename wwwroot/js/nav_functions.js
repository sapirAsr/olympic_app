//open log in page
function open_login_page() {
    var x = document.getElementById("myGrid");
    window.location.href = "login.html";
}

//open sign up page
function open_sign_up_page() {
    var x = document.getElementById("myGrid");
    window.location.href = "signup.html";
}

//open search page
function open_ask_us_page() {
    window.location.href = "search_page.html";
}

//open tests menu page
function open_tests_menu_page() {
    window.location.href = "tests_menu.html";
}

//open home(feed) page
function open_home_page() {
    window.location.href = "feed.html";
}

//open profile page
function open_profile_page() {
    window.location.href = "profile.html";
}

//open first page (this is the first page of the application)
function open_first_page() {
    window.location.href = "first_page.html";
}

//open change details inner page
function open_change_details_page() {

    document.getElementById("show_details").src = "change_details.html";
}

//open admin list inner page
function open_admin_list_page() {
    document.getElementById("show_details").src = "admin_list.html";
}