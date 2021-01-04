using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using olympic_app.Models;

namespace olympic_app.DB
{  

    public class DBConnect
    {
        private MySqlConnection connection;
        private MySqlDataReader dataReader;

        private List<string> sportsList = new List<string>();
        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
        
            string connectionString = "Server=127.0.0.1;Database=olympicapp;User Id=root;Password=Sapir1912" ;
          
            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                
                return false;
    
            }
        }

        //Close connection
        public bool CloseConnection()
        {
             try
             {
                connection.Close();
                return true;
             }
            catch (MySqlException ex)
            {
                return false;
            }

        }

        
        //Select statement
        public List <string> [] Select()
        {
            List< string >[] list = new List< string >[2];
            list[0] = new List< string >();
            list[1] = new List< string >();
           
            var queryString = "SELECT Athlete_Id,Name FROM olympicapp.athletes";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                list[0].Add(dataReader["Athlete_Id"] + "");
                list[1].Add(dataReader["Name"] + "");
                //list[2].Add(dataReader["age"] + "");
            }
            //close Data Reader
            dataReader.Close();

             return list;
           
        }

        //filter statement
        public List <string> [] BasicFilter(string table, List<string> atributes)
        {
            List< string >[] list = new List< string >[atributes.Count];
            for (int i = 0; i < atributes.Count; i++ ){
                list[i] = new List< string >();
            }
           
           string atributesStr = "";

           foreach (string item in atributes)
           {
               atributesStr += item;
               atributesStr += ",";
           }
            atributesStr = atributesStr.Remove(atributesStr.Length - 1);

            var queryString = "SELECT " + atributesStr + " FROM " + table + ";";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                for (int i = 0; i < atributes.Count; i++ ){
                    list[i].Add(dataReader[atributes[i]] + "");
                }               
            }
            //close Data Reader
            dataReader.Close();

             return list;
           
        }

        public List<string> GeneratePosts(){
            List<string> posts = new List<string>();
            List<string> temp = new List<string>();
            //populating list of sports only once
            if (sportsList.Count == 0){
                string query =  "SELECT DISTINCT Sport FROM olympicapp.event_types;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                List<string> sports =  new List<string>();
                dataReader = cmd.ExecuteReader();

                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    sports.Add(dataReader["Sport"] + "");
                            
                }
                //close Data Reader
                dataReader.Close();
            
                sportsList = sports;

            }
            // choosing a random sport
            int numberOfPosts = 1;
            for (int i = 0; i < numberOfPosts; i++)
            {
                var random = new Random();
                int index = random.Next(sportsList.Count);
                string sport = sportsList[index];
                string result = "The best athlete in the field of " + sport + " is ";
                result += TheBestAthlete(sport);
                result += ".\n The best athlete is the athlete who won the most medals.";
                posts.Add(result);
                InsertIntoFeedTable(result,sport);
                /**
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the oldest athlete in the field of " + sport + " is "; 
                temp = TheMostXAthlete(sport,"Birth_year","ASC");
                result += temp[0] + ".";
                posts.Add(result);
                
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the youngest athlete in the field of " + sport + " is ";
                temp= TheMostXAthlete(sport,"Birth_year","DESC");
                result += temp[0] + ".";
                posts.Add(result);
                */

                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the fatest athlete in the field of " + sport + " is ";
                temp = TheMostXAthlete(sport,"Weight","ASC");
                if (temp.Count > 0){
                    result += temp[0] + "?\n This athlete weight is " + temp[1] + ".";
                    posts.Add(result);
                    InsertIntoFeedTable(result,sport);
                }

                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the leanest athlete in the field of " + sport + " is ";
                temp = TheMostXAthlete(sport,"Weight","DESC");
                if (temp.Count > 0){

                    result += temp[0] + "?\n This athlete weight is " + temp[1] + ".";
                    posts.Add(result);
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the tallest athlete in the field of " + sport + " is ";
                temp = TheMostXAthlete(sport,"Height","ASC");
                if (temp.Count > 0){
                    result += temp[0] + "?\n This athlete height is " + temp[1] + ".";
                    posts.Add(result);
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the shortest athlete in the field of " + sport + " is ";
                temp = TheMostXAthlete(sport,"Height","DESC");
                if (temp.Count > 0){    
                    result += temp[0] + "?\n This athlete height is " + temp[1] + ".";
                    posts.Add(result);
                    InsertIntoFeedTable(result,sport);
                }
      
            }   
            return posts;
        }
        
        public void InsertIntoFeedTable(string content, string sport){
 
                string date = DateTime.Today.ToString("yyyy-MM-dd");
                //INSERT INTO olympicapp.feed (Post_content,Sport,Date)
                  //  VALUES ("test","test","2017-06-15");        
                string queryString = "INSERT INTO olympicapp.feed (Post_content,Sport,Date) VALUES (\"" + content + "\",\"" +  sport + "\",\"" + date + "\");";
                Console.WriteLine(queryString);
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {}    
                //close Data Reader
                dataReader.Close();
                
        }
        public void SportList(){
            string query =  "SELECT DISTINCT Sport FROM olympicapp.event_types;";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            List<string> sports =  new List<string>();
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                sports.Add(dataReader["Sport"] + "");
                          
            }
            //close Data Reader
             dataReader.Close();
        
             sportsList = sports;
            
        }
         
        public List<Post> FeedPosts(){
            string queryString = "SELECT * FROM olympicapp.feed ORDER BY RAND() LIMIT 10;";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            List<Post> posts =  new List<Post>();
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                Post p1 = new Post { PostId = Int32.Parse(dataReader["Post_id"] + ""), Content = dataReader["Post_content"] + "", Likes = 0, Date = DateTime.Parse(dataReader["Date"] + "" )};
                posts.Add(p1);

            }
            //close Data Reader
             dataReader.Close();
             foreach( Post p in posts){
                p.Likes = GetNumberOfLikes(p.PostId);
             }
            
            return posts;
        }

        // queries for feed  
        public List<string> TheMostXAthlete(string sport, string parameter, string order){

             var queryString = "SELECT Name,"+ parameter +" FROM" +
            "(SELECT Athlete_id, Name, "+ parameter +" FROM " +
            "(SELECT Athlete_id, Name, "+ parameter +" FROM olympicapp.athletes AS temp WHERE Athlete_id IN " + 
            "(SELECT Athlete_Id FROM olympicapp.medals WHERE (event_id IN " +
            "(SELECT event_id FROM olympicapp.event_types WHERE sport = \"" + sport + "\"" + "))" +
            "GROUP BY Athlete_Id) AND " + parameter +" <> \"NA\") AS temp2 ORDER BY " + parameter + " " + order + " LIMIT 1) AS temp3;";
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result.Add(dataReader["Name"] + "");
                result.Add(dataReader[parameter] + "");
     
            }
            //close Data Reader
             dataReader.Close();
             return result;
        }

        //the best athlete in specific sport

        public string TheBestAthlete(string sport){

            var queryString = "SELECT Name FROM olympicapp.athletes WHERE Athlete_Id = (SELECT Athlete_Id FROM (" +
            "SELECT Athlete_Id, COUNT(*) AS magnitude FROM (SELECT Athlete_Id, Medal FROM olympicapp.medals WHERE ((event_id IN " +
            "(SELECT event_id FROM olympicapp.event_types WHERE sport = \"" + sport + "\"" + ")) AND  medal <> \"NA\")) AS temp " +
            "GROUP BY Athlete_Id " +
             "ORDER BY magnitude DESC " +
            "LIMIT 1) AS temp2);";
            string result = "";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result += dataReader["Name"] + "";
                          
            }
            //close Data Reader
             dataReader.Close();
            
             return result;

        }
    
        //users

        public bool NewUserRegister(string username, string password){
            string queryString = "INSERT INTO olympicapp.users (User_name,Password,Is_admin) VALUES (\"" + username + "\",\"" +  password + "\",0);";            
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
            dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) {}
            dataReader.Close();
            return true;
            }
            catch (MySqlException ex){

                Console.WriteLine(ex.Data);
                Console.WriteLine("alredy exist");
                return false;
            }   
            //close Data Reader
        }
        public bool Login(string username, string password){
            string queryString ="SELECT User_name, Password FROM olympicapp.users WHERE User_name = \""+username+"\" AND Password = \""+password+"\"";
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) {
                        result.Add(dataReader["User_name"] + "");
                        result.Add(dataReader["Password"] + "");
                    }
                    dataReader.Close();
                    if (result.Count == 2){
                        return true;
                    }
            }
            catch (MySqlException ){
                        
                Console.WriteLine("password or user name incorrect");
            } 
            return false;
                        
                                    
        }

        public bool DeleteUser(string username, string password, bool isAdmin){
            string queryString =" DELETE FROM olympicapp.users WHERE User_name = \""+username+"\" AND Password = \""+password+"\"";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try
            {
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) {}
                    dataReader.Close();
                    return true;
            }
            catch (MySqlException ){
                        
                    Console.WriteLine("error while deleting this user");
            }
            if(isAdmin){
                queryString =" DELETE FROM olympicapp.admin_premission WHERE User_name = \""+username+"\";";
                cmd = new MySqlCommand(queryString, connection);
                try
                {
                        dataReader = cmd.ExecuteReader();
                        while (dataReader.Read()) {    }
                        dataReader.Close();
                        return true;
                }
                catch (MySqlException )
                {
                                
                        Console.WriteLine("error while deleting this admin user");
                }
            }
            return false;
                        
        }
        public bool ChangePassword(string username, string new_password){
      
            string queryString =" UPDATE olympicapp.users WHERE SET Password = \""+ new_password +"\"' WHERE User_name = \""+ username +"\"";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try
            {
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) {}
                    dataReader.Close();
                    return true;
            }
            catch (MySqlException ){
                        
                    Console.WriteLine("error while deleting this user");
            }
            return false;


        }
    
         //likes
        public bool LikePost(string username, int post_id){
            string queryString ="INSERT INTO olympicapp.likes (User_name,Post_id)"+
                                "VALUES (\""+username+"\","+ post_id+");";
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try {
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) {}
                    dataReader.Close();
                    return true;
            }
            catch (MySqlException ){
                        
                Console.WriteLine("user alredy liked this post.");
            } 
            return false;
        }

        public int GetNumberOfLikes(int post_id){
            string queryString = "SELECT COUNT(Post_id) AS NumberOfLikes FROM" +
                                    "(SELECT Post_id FROM olympicapp.likes WHERE Post_id =" + post_id +") AS temp";

            string result ="";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) {
                        result = dataReader["NumberOfLikes"] + "";
                    }
                    dataReader.Close();
                    return Int32.Parse(result);
                    
            }
            catch (MySqlException ){    
                return 0;
            } 
        
        }

    }

}      
    