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
        public int GetNumberOfLikes(string post_id)
        {
            return dBConnect.GetNumberOfLikes(post_id);
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
        public List<string> GetTeamsList(){
            return dBConnect.GetTeamsList();           
        }
        public List<string> GetHeightsList(){
            return dBConnect.GetHeightsList();           
        }
        public List<string> GetWeightsList(){
            return dBConnect.GetWeightsList();           
        }
        public List<string> GetBirthYears(){
            return dBConnect.GetBirthYears();           
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

        
        public List <string>  BasicFilter(Dictionary<string, string> dictAtr) {
            return dBConnect.BasicFilter(dictAtr);
        }


        //quiz
        public List<Question> GetQuestions(string sport){
            return dBConnect.GetQuestions(sport);
        }
        //users
        public User UserLogin(string username, string password){
            return dBConnect.Login(username, password);
        }
        public User UserSignup(string username, string password){
            return dBConnect.NewUserRegister(username, password);
        }
    
        public bool LikePost(string username, string post_id){
             return dBConnect.LikePost(username,post_id);
        }
        public bool DislikePost(string username, string post_id)
        {
            return dBConnect.DislikePost(username, post_id);
        }

        public void DeleteUser(string username){
            dBConnect.DeleteUser(username);
        }
        public bool ChangePassword(string username, string password){
             return dBConnect.ChangePassword(username,password);

        }
        public bool UpdateAdmin(string user,string sport, bool isAdmin)
        {
            return dBConnect.UpdateAdmin(user, sport, isAdmin);
        }
        public List<string> GetAdminList(string username){
            return dBConnect.GetAdminList(username);
        }


    }
}
