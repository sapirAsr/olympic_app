using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using olympic_app.Models;
using olympic_app.DB;


namespace olympic_app.Models
{
    public class AppManager : IAppManager
    {
        private DBConnect dBConnect;

        public AppManager(){
            dBConnect = new DBConnect();
            dBConnect.OpenConnection();
        }
        public List<Post> getPosts()
        { 
            List< Post > list = dBConnect.FeedPosts(); 
            return list;
        }

        public bool UserLogin(string username, string password){
            return dBConnect.Login(username, password);
        }
        public bool UserSignup(string username, string password){
            return dBConnect.NewUserRegister(username, password);
        }
    
        public bool LikePost(string username, int post_id){
             return dBConnect.LikePost(username,post_id);
        }
    }
}
