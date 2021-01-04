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
        private List<string> gamesList = new List<string>();

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
                sportsList = SelectColFromTable("Sport","event_types");
                gamesList = SelectColFromTable("Game","olympic_games");
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
        //TODO ON OMER
        public List<string> GetSportList(){
            return sportsList;
        }

        public List<string> GeneratePosts(){
            List<string> posts = new List<string>();
            List<List<string>> temp = new List<List<string>>();           
            // choosing a random sport
            int numberOfPosts = 1;
            for (int i = 0; i < numberOfPosts; i++)
            {
                var random = new Random();
                int index = random.Next(sportsList.Count);
                string sport = sportsList[index];
                string result = "The best athlete in the field of " + sport + " is ";
                result += TheBestXAthlete(sport, " AND  medal <> \"NA\"")[0];
                result += ".\n The best athlete is the athlete who won the most medals.";
                posts.Add(result);
                InsertIntoFeedTable(result,sport);
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

                    result += temp[0][0] + "?\n This athlete weight is " + temp[0][1] + ".";
                    posts.Add(result);
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the tallest athlete in the field of " + sport + " is ";
                temp = TheMostXAthlete(sport,"Height","ASC");
                if (temp.Count > 0){
                    result += temp[0][0] + "?\n This athlete height is " + temp[0][1] + ".";
                    posts.Add(result);
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the shortest athlete in the field of " + sport + " is ";
                temp = TheMostXAthlete(sport,"Height","DESC");
                if (temp.Count > 0){    
                    result += temp[0][0] + "?\n This athlete height is " + temp[0][1] + ".";
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
        
        public List<string> SelectColFromTable(string col, string table){
            string query =  "SELECT DISTINCT "+ col + " FROM olympicapp."+ table +";";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            List<string> result =  new List<string>();
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result.Add(dataReader[col] + "");
                          
            }
            //close Data Reader
             dataReader.Close();
        
            return result;            
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
        public List<List<string>> TheMostXAthlete(string sport, string parameter, string order){

            var queryString = "SELECT Name,"+ parameter +" FROM" +
            "(SELECT Athlete_id, Name, "+ parameter +" FROM " +
            "(SELECT Athlete_id, Name, "+ parameter +" FROM olympicapp.athletes AS temp WHERE Athlete_id IN " + 
            "(SELECT Athlete_Id FROM olympicapp.medals WHERE (event_id IN " +
            "(SELECT event_id FROM olympicapp.event_types WHERE sport = \"" + sport + "\"" + "))" +
            "GROUP BY Athlete_Id) AND " + parameter +" <> \"NA\") AS temp2 ORDER BY " + parameter + " " + order + " LIMIT 4) AS temp3;";
            List<List<string>> result = new List<List<string>>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result.Add(new List<string> {dataReader["Name"] + "",dataReader[parameter] + ""});
     
            }
            //close Data Reader
            dataReader.Close();
            return result;
        }

        //the best athlete in specific sport
        /**
        public List<string> TheBestAthlete(string sport, string helper){
            var queryString = "SELECT Name FROM olympicapp.athletes WHERE Athlete_Id = (SELECT Athlete_Id FROM (" +
            "SELECT Athlete_Id, COUNT(*) AS magnitude FROM (SELECT Athlete_Id, Medal FROM olympicapp.medals WHERE ((event_id IN " +
            "(SELECT event_id FROM olympicapp.event_types WHERE sport = \"" + sport + "\"" + ")) AND  medal <> \"NA\")) AS temp " +
            "GROUP BY Athlete_Id " +
             "ORDER BY magnitude DESC " +
            "LIMIT 1) AS temp2);";
            List<string> result = new;
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
        */
    
        // quiz

        public List<Question> GetQuestions(string sport){
            List<Question> questions = new List<Question>();
            //q1 who's the best athlete in the given sport
            List<string> theBestAthleteAnswers = TheBestXAthlete(sport, " AND  medal <> \"NA\"");
            string question = "Who's the best athlete in the field of " + sport + "?\n Hint:The best athlete is the athlete who won the most medals.";
            Question q1 = new Question {QuestionString = question , CorrectAnswer = theBestAthleteAnswers[0], WrongAnswer1 = theBestAthleteAnswers[1],
                                        WrongAnswer2 = theBestAthleteAnswers[2], WrongAnswer3 = theBestAthleteAnswers[3], Sport = sport };
            questions.Add(q1);
            // q2 in which year the best athlete was born
            string birthYear = GetXByYWhereZFromAthletes("Birth_year", "Name", theBestAthleteAnswers[0]);
            List<string>wrongAnsersList = WrongYears(birthYear);
            question = "In which year " + theBestAthleteAnswers[0] + " was born?";
            Question q2 = new Question {QuestionString = question , CorrectAnswer = birthYear, WrongAnswer1 = wrongAnsersList[0],
                                        WrongAnswer2 = wrongAnsersList[1], WrongAnswer3 = wrongAnsersList[2], Sport = sport };
            questions.Add(q2);

            // q3 where this game took place
            var random = new Random();
            int index = random.Next(gamesList.Count);
            string randomGame = gamesList[index];
            string country = CountryOfOlympicGame(randomGame);
            wrongAnsersList = WrongCountries(country);
            question = "In which country the " + randomGame + "  games took place?";
            Question q3 = new Question {QuestionString = question , CorrectAnswer = country, WrongAnswer1 = wrongAnsersList[0],
                                        WrongAnswer2 = wrongAnsersList[1], WrongAnswer3 = wrongAnsersList[2], Sport = sport };
            questions.Add(q3);
             // q4 who is the athlete that participant the most in games
            List<string> answers = TheBestXAthlete(sport, "");
            question = "Who is the athlete that participant the most in the olympic games in the field of " + sport + "?";
            Question q4 = new Question {QuestionString = question , CorrectAnswer = answers[0], WrongAnswer1 = answers[1],
                                        WrongAnswer2 = answers[2], WrongAnswer3 = answers[3], Sport = sport };
            questions.Add(q4);
             // q4 who is the tallest athlete
            List<List<string>> results = TheMostXAthlete(sport,"Height","ASC");
            question = "Who is the tallest athlete in the field of " + sport + "?";
            Question q5 = new Question {QuestionString = question , CorrectAnswer = results[0][0], WrongAnswer1 = results[1][0],
                                        WrongAnswer2 = results[2][0], WrongAnswer3 = results[3][0], Sport = sport };
            questions.Add(q5);
            return questions;
        }

        /*
        public List<string> WrongsBestAthlete(string sport, string id){
            var queryString = "SELECT NAME FROM athletes INNER JOIN (SELECT Athlete_Id FROM (" +
            "SELECT Athlete_Id FROM (SELECT Athlete_Id, Medal FROM olympicapp.medals WHERE (event_id IN" + 
            "(SELECT event_id FROM olympicapp.event_types WHERE sport = \"" +sport + "\")) AND Athlete_id <> \""+ id + "\") AS temp " + 
            "GROUP BY Athlete_Id LIMIT 3) AS temp2) AS temp3 ON temp3.Athlete_id = athletes.Athlete_id;";
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result.Add(dataReader["Name"] + "");
                          
            }
            //close Data Reader
             dataReader.Close();
            
             return result;

        }*/

        public string GetXByYWhereZFromAthletes(string x, string y, string z){
            var queryString = @"SELECT " + x +" From olympicapp.athletes WHERE "+ y +" = '" + z + "';";
            string result = "";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result += dataReader[x] + "";
                          
            }
            //close Data Reader
             dataReader.Close();
            
             return result;

            
        }

        public List<string> WrongYears(string year){
            var queryString = "SELECT Birth_year From olympicapp.athletes WHERE Birth_year <> \"" + year + "\" LIMIT 3";
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result.Add(dataReader["Birth_year"] + "");
                        
            }
            //close Data Reader
            dataReader.Close();                
            return result;

        }

        public string CountryOfOlympicGame(string game){
            var queryString = "SELECT Country FROM countries WHERE City =(SELECT City FROM olympic_games WHERE Game = \"" + game + "\")";;
            string result = "";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result += dataReader["Country"] + "";
                          
            }
            //close Data Reader
             dataReader.Close();
            
             return result;
        }
        public List<string> WrongCountries(string country){
            var queryString = "SELECT DISTINCT Country From olympicapp.countries WHERE Country <> \"" + country + "\" LIMIT 3;";
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result.Add(dataReader["Country"] + "");
                        
            }
            //close Data Reader
            dataReader.Close();
                
            return result;

        }
        
        //get the best athlete with the helper " AND  medal <> \"NA\"" and if helper = "" get the most participant
        public List<string> TheBestXAthlete(string sport , string helper){
            var queryString = "SELECT Name FROM olympicapp.athletes WHERE Athlete_Id IN (SELECT Athlete_Id FROM (" +
            "SELECT Athlete_Id, COUNT(*) AS magnitude FROM (SELECT Athlete_Id, Medal FROM olympicapp.medals WHERE ((event_id IN " +
            "(SELECT event_id FROM olympicapp.event_types WHERE sport = \"" + sport + "\"" + ")))" + helper + ") AS temp " +
            "GROUP BY Athlete_Id " +
             "ORDER BY magnitude DESC " +
            "LIMIT 4) AS temp2);";
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result.Add(dataReader["Name"] + "");
                          
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
        public User Login(string username, string password){
            string queryString ="SELECT User_name, Password, Is_admin FROM olympicapp.users WHERE User_name = \"" + username + "\" AND Password = \"" + password + "\"";
            User result = new User();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) {
                        result.Username = dataReader["User_name"] + "";
                        result.Password = dataReader["Password"] + "";
                        result.isAdmin = Convert.ToBoolean(Convert.ToInt16(dataReader["Is_admin"] + ""));

                    }
                    dataReader.Close();
                    return result;
                    
            }
            catch (MySqlException ){
                        
                Console.WriteLine("password or user name incorrect");
            } 
            return result;
                        
                                    
        }

        public bool DeleteUser(User user){
            string queryString =" DELETE FROM olympicapp.users WHERE User_name = \""+user.Username+"\" AND Password = \""+user.Password+"\"";
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
            if(user.isAdmin){
                queryString =" DELETE FROM olympicapp.admin_premission WHERE User_name = \"" + user.Username + "\";";
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
      
            string queryString =" UPDATE olympicapp.users SET Password = \""+ new_password +"\"' WHERE User_name = \""+ username +"\"";
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
    
        public bool UpdateAdmin(User user,string sport, bool isAdmin)
        {
            string queryString = "";
            if (!isAdmin)
            {
                queryString = "INSERT INTO admin_premission (User_name,"+ sport +") VALUES(\"" + user.Username + "\", 1)";  
            } 
            else{
                queryString = "UPDATE olympicapp.admin_premission SET "+ sport +"= 1 WHERE User_name = " + user.Username + "\";";
            } 
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
    