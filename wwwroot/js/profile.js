function delete_user(){
    // client needs to provide the username and if its admin adding it in the end of the url
        fetch("https://localhost:5001/api/Users/delete/sapir&" , {
            method: 'DELETE',
        });     
      
}