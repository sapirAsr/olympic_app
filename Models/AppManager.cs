using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using olympic_app.Models;
using System.Linq;
using olympic_app.DB;


namespace olympic_app.Models
{
    public class AppManager : IAppManager
    {
        private DBConnect dbConnect;
        private DBUsers dbUsers;
        private DBGeneral dbGeneral;
        private DBFeed dbFeed;
        private DBSearch dbSearch;
        private DBQuiz dbQuiz;

        public AppManager(){

            dbConnect = new DBConnect();
            bool connection = dbConnect.OpenConnection();
            if(!connection) {
                Console.WriteLine("could not connect to the SQL Server");
            }
            MySqlConnection conn = dbConnect.GetConnection();
            dbUsers = new DBUsers(conn);
            dbGeneral = new DBGeneral(conn);
            dbFeed = new DBFeed(conn, dbGeneral);
            dbSearch = new DBSearch(conn);
            dbQuiz = new DBQuiz(conn, dbGeneral);
        }
        //feed
        public List<Post> getPosts()
        { 
            List< Post > list = dbFeed.FeedPosts(); 
            return list;
        }
        public int GetNumberOfLikes(string post_id)
        {
            return dbFeed.GetNumberOfLikes(post_id);
        } 
        
        
        //search
         public List<string> GetGamesList()
        { 
            List< string > list = dbGeneral.GetGamesList(); 
            return list;
        }
        public List<string> GetSportList(){
            return dbGeneral.GetSportList();           
        }
        public List<string> GetTeamsList(){
            return dbGeneral.GetTeamsList();           
        }
        public List<string> GetHeightsList(){
            return dbGeneral.GetHeightsList();           
        }
        public List<string> GetWeightsList(){
            return dbGeneral.GetWeightsList();           
        }
        public List<string> GetBirthYears(){
            return dbGeneral.GetBirthYears();           
        }
        public string GetBestAthlete(string sport){
            return dbGeneral.TheBestXAthlete(sport, " AND  medal <> \"NA\"")[0];           
        }

        public List<string> GetLocationGame(string game){
           return dbGeneral.LocationOfOlympicGame(game);           
        }
        public List<string> GetTheMostXAthlete(string sport, string parameter, string order){
            return dbGeneral.TheMostXAthlete(sport, parameter, order)[0];           
        }

        
        public List<string> Filter(Dictionary<string, string> dictAtr) {
            return dbSearch.Filter(dictAtr);
        }


        //quiz
        public List<Question> GetQuestions(string sport){
            return dbQuiz.GetQuestions(sport);
        }
        //users
        public User UserLogin(string username, string password){
            return dbUsers.Login(username, password);
        }
        public User UserSignup(string username, string password){
            return dbUsers.NewUserRegister(username, password);
        }
    
        public bool LikePost(string username, string post_id){
             return dbFeed.LikePost(username,post_id);
        }
        public bool DislikePost(string username, string post_id)
        {
            return dbFeed.DislikePost(username, post_id);
        }

        public void DeleteUser(string username){
            dbUsers.DeleteUser(username);
        }
        public bool ChangePassword(string username, string password){
             return dbUsers.ChangePassword(username,password);

        }
        public bool UpdateAdmin(string user,string sport, bool isAdmin)
        {
           // return dBConnect.UpdateAdmin(user, sport, isAdmin);
            return dbUsers.UpdateAdmin(user, sport, isAdmin);

        }
        public List<string> GetAdminList(string username){
            return dbUsers.GetAdminList(username);
        }


    }
}
