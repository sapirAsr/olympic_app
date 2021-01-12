function load_feed_news() {
    var first_elem = document.getElementById("1st_msg");
    var second_elem = document.getElementById("2nd_msg");
    var third_elem = document.getElementById("3rd_msg");
    var four_elem = document.getElementById("4th_msg");
    var five_elem = document.getElementById("5th_msg");
    var six_elem = document.getElementById("6th_msg");
    var seven_elem = document.getElementById("7th_msg");
    var eight_elem = document.getElementById("8th_msg");
    var nine_elem = document.getElementById("9th_msg");
    var ten_elem = document.getElementById("10th_msg");
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let feedPost = JSON.parse(this.responseText);
                first_elem.textContent = feedPost[0].content;
                second_elem.textContent = feedPost[1].content;
                third_elem.textContent = feedPost[2].content;
                four_elem.textContent = feedPost[3].content;
                five_elem.textContent = feedPost[4].content;
                six_elem.textContent = feedPost[5].content;
                seven_elem.textContent = feedPost[6].content;
                eight_elem.textContent = feedPost[7].content;
                nine_elem.textContent = feedPost[8].content;
                ten_elem.textContent = feedPost[9].content;
            } else {
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            }
        }
    };
    xhttp.open("GET", "http://localhost:5001/api/Feed", true);
    xhttp.send();
}