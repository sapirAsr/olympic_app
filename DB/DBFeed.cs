using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using olympic_app.Models;
using System.Linq;


namespace olympic_app.DB
{  
    public class DBFeed
    {
        private MySqlConnection connection;
        private MySqlDataReader dataReader;
        private DBGeneral dbGeneral;
        private List<string> sportsList = new List<string>();
        private List<string> gamesList = new List<string>();
        private List<string> teamsList = new List<string>();
        //Constructor
        public DBFeed(MySqlConnection conn, DBGeneral gen)
        {
            connection = conn;
            dbGeneral = gen;
            sportsList = dbGeneral.GetSportList();
            gamesList = dbGeneral.GetGamesList();
            teamsList = dbGeneral.GetTeamsList();
        }
        public void GeneratePosts(){
            List<List<string>> temp = new List<List<string>>();
            List<string> check = new List<string>();                    
            int numberOfPosts = 1;
            for (int i = 0; i < numberOfPosts; i++)
            {
                var random = new Random();
                int index = random.Next(sportsList.Count);
                string sport = sportsList[index];
                string result = "The best athlete in the field of " + sport + " is ";
                check = dbGeneral.TheBestXAthlete(sport, " AND  medal <> \"NA\"");
                if (check.Count > 0){
                    result += check[0];
                    result += ".<br> The best athlete is the athlete who won the most medals.";
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the heaviest athlete in the field of " + sport + " is ";
                temp = dbGeneral.TheMostXAthlete(sport,"Weight","DESC");
                if (temp.Count > 0){
                    result += temp[0][0] + "?<br> This athlete weight is " + temp[0][1] + "kg.";
                    InsertIntoFeedTable(result,sport);
                }

                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the leanest athlete in the field of " + sport + " is ";
                temp = dbGeneral.TheMostXAthlete(sport,"Weight","ASC");
                if (temp.Count > 0){

                    result += temp[0][0] + "?<br> This athlete weight is " + temp[0][1] + "kg.";
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the tallest athlete in the field of " + sport + " is ";
                temp = dbGeneral.TheMostXAthlete(sport,"Height","DESC");
                if (temp.Count > 0){
                    result += temp[0][0] + "?<br> This athlete height is " + temp[0][1] + "cm.";
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the shortest athlete in the field of " + sport + " is ";
                temp = dbGeneral.TheMostXAthlete(sport,"Height","ASC");
                if (temp.Count > 0){    
                    result += temp[0][0] + "?<br> This athlete height is " + temp[0][1] + "cm.";
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(teamsList.Count);
                string team = teamsList[index];
                result = "Did you know that the " + team + " team was represented by ";
                string number = GetNumberOfAthletesFromTeam(team);
                if (number != "")
                {
                    result += number + " athletes?<br>";
                    InsertIntoFeedTable(result, "General");
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that there are ";
                number = GetDistinctEvents(sport);
                if (number != "")
                {
                    result += number + " different events in the " + sport +" field?<br>";
                    InsertIntoFeedTable(result, "General");
                }
                index = random.Next(gamesList.Count);
                string game = gamesList[index];
                result = "Did you know that the " + game + "  Olympics took place in ";
                check = dbGeneral.LocationOfOlympicGame(game);
                if (check.Count > 0)
                {
                    result += check[1] +", " + check[0] + "?<br>";
                    InsertIntoFeedTable(result, sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the average height in the field of " + sport + " is ";
                string maleAvg = GetAvgOfGender(sport, "M");
                string femaleAvg = GetAvgOfGender(sport, "F");
                if (maleAvg != "" && femaleAvg !=" ")
                {
                    result += maleAvg + " for men and " + femaleAvg + " for women?<br>";
                    InsertIntoFeedTable(result, sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that ";
                check = GetRandomWin(sport);
                if (check.Count > 0)
                {
                    result += check[0] + " won a " + check[1] + " medal in the " + sport + " field?<br>";
                    InsertIntoFeedTable(result, sport);
                }
      
            }   
        }
        
        public void InsertIntoFeedTable(string content, string sport){
            string date = DateTime.Today.ToString("yyyy-MM-dd");      
            string queryString = @"INSERT INTO olympicapp.feed (Post_content,Sport,Date) VALUES ('" + content + "',\"" +  sport + "\",\"" + date + "\");";
            try{
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read()){}
            }
            catch (MySqlException){} 
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            }        
        }
        
         
        public List<Post> FeedPosts(){
            GeneratePosts();
            string queryString = "SELECT * FROM olympicapp.feed ORDER BY RAND() LIMIT 10;";
            List<Post> posts =  new List<Post>();
            try{
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    Post p1 = new Post { PostId = Int32.Parse(dataReader["Post_id"] + ""), Content = dataReader["Post_content"] + "", Likes = 0, Date = DateTime.Parse(dataReader["Date"] + ""), Sport = dataReader["Sport"] + ""};
                    posts.Add(p1);
                }
            }
            catch (MySqlException){} 
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            }   
            foreach( Post p in posts){
                p.Likes = GetNumberOfLikes(p.PostId.ToString());
            }
            return posts;
        }

         public int GetNumberOfLikes(string post_id){
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
                //close Data Reader
                if(dataReader != null){
                    dataReader.Close();
                }              
                return 0;
            } 
        
        }



       public string GetNumberOfAthletesFromTeam (string team)
        {
            var queryString = @"SELECT COUNT(distinct athletes.Name) as number FROM olympicapp.athletes WHERE athletes.Team='" + team +"';";
            string result = "";
            try{
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result += dataReader["number"] + "";
                }
            }
            catch(MySqlException){}
           //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            } 
            return result;
        }
        public string GetDistinctEvents(string sport)
        {
            var queryString = @"SELECT COUNT(distinct event_types.event) as number FROM olympicapp.event_types WHERE event_types.Sport='" + sport + "';";
            string result = "";
            try{
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result += dataReader["number"] + "";
                }
            } 
            catch(MySqlException){}
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            } 
            return result;
        }
        public string GetAvgOfGender(string sport, string gender)
        {
            var queryString = @"SELECT ROUND(AVG(athletes.Height),2) as avg FROM olympicapp.athletes WHERE athletes.Sex='" + gender +"'" +
                " AND NOT athletes.Height='NA' and Athlete_id IN" +
                "(SELECT Athlete_Id FROM olympicapp.medals WHERE((event_id IN " +
                "(SELECT event_id FROM olympicapp.event_types WHERE sport = '" + sport + "'))));";
            string result = "";
            try {
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result += dataReader["avg"] + "";
                }
            }
            catch(MySqlException){}
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            } 
            return result;
        }
        public List<string> GetRandomWin(string sport)
        {
            var queryString = "(SELECT olympicapp.athletes.Name, olympicapp.medals.Medal " +
                              "FROM olympicapp.medals JOIN olympicapp.event_types " +
                              "ON olympicapp.medals.Event_id = olympicapp.event_types.Event_id JOIN olympicapp.athletes " +
                              "ON olympicapp.athletes.Athlete_id = olympicapp.medals.Athlete_id " +
                              "WHERE NOT olympicapp.medals.Medal = \"NA\" AND olympicapp.event_types.Sport = '" + sport +"') " +
                              "ORDER BY RAND() LIMIT 1;";
            List<string> result = new List<string>();
            try {
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result.Add(dataReader["Name"] + "");
                    result.Add(dataReader["Medal"] + "");
                }
            }
            catch(MySqlException){}
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            } 
            return result;
        }
                public bool LikePost(string username, string post_id){
            string queryString ="INSERT INTO olympicapp.likes (User_name,Post_id)"+
                                "VALUES (\""+username+"\","+ post_id+");";
            List<string> result = new List<string>();
            try {
                    MySqlCommand cmd = new MySqlCommand(queryString, connection);
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) {}
                    dataReader.Close();
                    return true;
            }
            catch (MySqlException ){} 
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            }             
            return false;
        }
        public bool DislikePost(string username, string post_id)
        {
            string queryString = "DELETE FROM olympicapp.likes WHERE User_name='" + username + "' and Post_id = " + post_id + ";";
            try
            {
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) { }
                dataReader.Close();
                return true;
            }
            catch (MySqlException) {}
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            }
            return false;
        }

    }
}