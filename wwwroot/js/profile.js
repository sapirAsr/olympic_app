

function delete_user(){
    $.ajax({
            url: 'https://localhost:5001/api/Users/delete',
            method: 'DELETE',
            data: {
                Username: "sapir",
                Password: "00001",
                IsAdmin: "1"
    
            },
            headers: {
                "Content-Type": "application/json;charset=utf-8"
            }
        }).then(function(res) {
            console.log(res.data);
        }, function(error) {
            console.log(error);
    });
    
}
  