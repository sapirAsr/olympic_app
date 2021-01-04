function get_posts(){
    console.log("SAPIRRRRRRR");  
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let feedPost = JSON.parse(this.responseText);
                let listOfPosts = [];    
                let post1 = feedPost[0];
                console.log(post1.Content);  
                for (i = 0; i < feedPost.length; i++){
                    $(".feed").append("<p>" + feedPost[i].content + "</p>");
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

function get_quiz(){
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let questions = JSON.parse(this.responseText);
                console.log(questions);  
                for (i = 0; i < questions.length; i++) {
                    $(".quiz").append("<p>" + questions[i].questionString + "</p>");
                    $(".quiz").append("<p>" + questions[i].correctAnswer + "</p>");
                    $(".quiz").append("<p>" + questions[i].wrongAnswer1 + "</p>");
                    $(".quiz").append("<p>" + questions[i].wrongAnswer2 + "</p>");
                    $(".quiz").append("<p>" + questions[i].wrongAnswer3 + "</p>");
                    $(".quiz").append("<p>" + questions[i].sport + "</p>");
                }


            } else {                   
                console.log("Error", xhttp.statusText);
                alert(xhttp.statusText);
            } 
        }           
    };     
    xhttp.open("GET", "https://localhost:5001/api/Quiz/Basketball", true);
    xhttp.send();  
}
