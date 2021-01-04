function load_feed_news() {
    var first_elem = document.getElementById("1st_msg");
    var second_elem = document.getElementById("2nd_msg");
    var feed_arr = [];
    var index_arr = [];
    var posts_amount = 2;
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let feedPost = JSON.parse(this.responseText);
                let i;
                for (i = 0; i < posts_amount;) {
                    //change to -1 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    let num = Math.floor( Math.random() * (feedPost.length));
                    let is_in = index_arr.includes(num);
                    if (is_in == false) {
                        index_arr.push(num); //new post
                        feed_arr.push(feedPost[num]);
                        i++;
                    }

                }
                first_elem.textContent = feed_arr[0].content;
                second_elem.textContent = feed_arr[1].content;
            } else {
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            }
        }
    };
    xhttp.open("GET", "https://localhost:5001/api/Feed", true);
    xhttp.send();
}
