function load_feed_news(){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                $("#feed-panel").empty();
                let feedPost = JSON.parse(this.responseText);
                for (i = 0; i < feedPost.length; i++) {
                    var str = "<div class='panel'><div class='media-block'>";
                    str += "<a class='media-left'> <img class='img-circle img-sm' alt='Profile Picture' src='lib/" + i + ".png'></a>";
                    str += "<div class='media-body'> <div class='mar-btm'>";
                    str += "<a class='btn-link text-semibold media-heading box-inline'>" + feedPost[i].sport + "</a>";
                    str += "<p class='text-muted text-sm'><i class='fa fa-mobile fa-lg'></i>" + feedPost[i].date.slice(0,10) + "</p></div>";
                    str += "<p>" + feedPost[i].content + "</p>";
                    str += "<div class='pad-ver'><div class='btn-group'>";
                    str += "<a class='btn btn-sm btn-default btn-hover-success' onclick = like(" + feedPost[i].postId + ") id='like" + feedPost[i].postId + "'><i class='zmdi zmdi-thumb-up'></i></a>";
                    str += "<a class='btn btn-sm btn-default btn-hover-success' onclick = dislike('" + feedPost[i].postId + "') id= 'dislike" + feedPost[i].postId + "'><i class='zmdi zmdi-thumb-down'></i></a></div >";
                    str += "<a class='small'><i class='fa fa-thumbs-up'></i>  <p id='" + feedPost[i].postId + "'>" + feedPost[i].likes + " Liked this!</p></a></div></div></div></div><span></span>";
                    $("#feed-panel").append(str);

                }
            } else {
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            }
        }
    };
    xhttp.open("GET", "https://localhost:5001/api/Feed", true);
    xhttp.send();
}
function like(post_id) {
    var username = sessionStorage.getItem('Username');
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                update_number_of_likes(post_id);
                document.getElementById("like" + post_id).style.backgroundColor = "#337ab7";
                document.getElementById("dislike" + post_id).style.backgroundColor = "#fff";
            } else {
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            }
        }
    };
    xhttp.open("POST", "https://localhost:5001/api/Feed/like/" + username + "&" + post_id, true);
    xhttp.send();
}
function dislike(post_id) {
    console.log(document.getElementById("like" + post_id).style.backgroundColor);
    if (document.getElementById("like" + post_id).style.backgroundColor == "rgb(51, 122, 183)") {
        document.getElementById("like" + post_id).style.backgroundColor = "#fff";
        var username = sessionStorage.getItem('Username');
        let xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState === 4) {
                if (this.status === 200) {
                    update_number_of_likes(post_id);
                    document.getElementById("dislike" + post_id).style.backgroundColor = "#f19c9c";
                } else {
                    console.log("Error", xhttp.statusText);
                    alert(xhttp.statusText);
                }
            }
        };
        xhttp.open("POST", "https://localhost:5001/api/Feed/dislike/" + username + "&" + post_id, true);
        xhttp.send();
    }
    
}
function update_number_of_likes(post_id) {
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let number_of_likes = JSON.parse(this.responseText);
                document.getElementById(post_id).textContent = number_of_likes + " Liked this!";
            } else {
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            }
        }
    };
    xhttp.open("GET", "https://localhost:5001/api/Feed/" + post_id, true);
    xhttp.send();

}
