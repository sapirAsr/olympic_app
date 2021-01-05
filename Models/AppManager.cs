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
        //feed
        public List<Post> getPosts()
        { 
            List< Post > list = dBConnect.FeedPosts(); 
            return list;
        }
        //search
         public List<string> GetGamesList()
        { 
            List< string > list = dBConnect.GetGamesList(); 
            return list;
        }
        public List<string> GetSportList(){
            return dBConnect.GetSportList();           
        }
        public string GetBestAthlete(string sport){
            return dBConnect.TheBestXAthlete(sport, " AND  medal <> \"NA\"")[0];           
        }

        public List<string> GetLocationGame(string game){
           return dBConnect.LocationOfOlympicGame(game);           

        }
        public List<string> GetTheMostXAthlete(string sport, string parameter, string order){
            return dBConnect.TheMostXAthlete(sport, parameter, order)[0];           
        }

        //quiz
        public List<Question> GetQuestions(string sport){
            return dBConnect.GetQuestions(sport);
        }
        //users
        public User UserLogin(string username, string password){
            return dBConnect.Login(username, password);
        }
        public bool UserSignup(string username, string password){
            return dBConnect.NewUserRegister(username, password);
        }
    
        public bool LikePost(string username, int post_id){
             return dBConnect.LikePost(username,post_id);
        }
    
        public void DeleteUser(string username){
            dBConnect.DeleteUser(username);
        }
        public bool ChangePassword(string username, string password){
             return dBConnect.ChangePassword(username,password);

        }
        public bool UpdateAdmin(User user,string sport, bool isAdmin)
        {
            return dBConnect.UpdateAdmin(user, sport, isAdmin);
        }
        public List<string> GetAdminList(string username){
            return dBConnect.GetAdminList(username);
        }


    }
}
