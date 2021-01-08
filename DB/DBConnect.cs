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
    public class DBConnect
    {
        private MySqlConnection connection;
        private MySqlDataReader dataReader;
        private List<string> sportsList = new List<string>();
        private List<string> gamesList = new List<string>();
        private List<string> teamsList = new List<string>();
        private List<string> heightsList = new List<string>();
        private List<string> weightsList = new List<string>();
        private List<string> yearsList = new List<string>();



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
                sportsList = SelectColFromTable("Sport","event_types", " ORDER BY Sport ASC");
                gamesList = SelectColFromTable("Game","olympic_games", " ORDER BY Game ASC");
                teamsList =  SelectColFromTable("Team","athletes"," ORDER BY Team ASC");
                heightsList =  SelectColFromTable("Height","athletes"," WHERE Height<>'NA' ORDER BY Height ASC");
                weightsList =  SelectColFromTable("Weight","athletes", " WHERE Weight<>'NA' ORDER BY cast(Weight as unsigned)  ASC");
                yearsList =  SelectColFromTable("Birth_year","athletes"," WHERE Birth_year<>'NA' ORDER BY Birth_year ASC");

                return true;
            }
            catch (MySqlException)
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
            catch (MySqlException)
            {
                return false;
            }

        }

    

        //search function
        public List <string> BasicFilter(Dictionary<string, string> dictAtr)
        {
            List< string > result = new List<string >();
            string selectStr = "";
            string table = "";
            bool flag = false;
            // handle which col we select and from where and change atr to match with the schema
            if (dictAtr.ContainsKey("Search")){
                if (dictAtr["Search"] == "Events"){
                    selectStr = "event";
                    table = "event_types"; 
                    flag = true;
                } else {
                    selectStr = "Name";
                    table = "Athletes";
                }
                dictAtr.Remove("Search");
            } else {
                selectStr = "Name";
                table = "Athletes";
            }
            if (!flag){
                if (dictAtr.ContainsKey("Sport")){
                        table = "(SELECT Athlete_id, Game_id, e.Event_id, Medal, Name, Sex, Height, Weight, Team,Birth_year,event, Sport " +
                                    "FROM event_types AS e " +
                                    "LEFT JOIN "+
                                    "(SELECT m.Athlete_id, Game_id, Event_id, Medal, Name, Sex, Height, Weight, Team,Birth_year "+
                                    "FROM medals AS m "+
                                    "LEFT JOIN athletes AS a ON a.Athlete_id= m.Athlete_id) AS temp "+
                                    "ON temp.Event_id= e.Event_id) AS temp";
                } 
            }
           //handle the where statment 
            string whereVals = "";
            foreach(KeyValuePair<string, string> pair in dictAtr){
                if (pair.Value.Length > 0){
                    whereVals += pair.Key;
                    whereVals += pair.Value;
                    whereVals +=  " AND " + pair.Key + "<>'NA'";
                    whereVals += " AND ";
                }

            }
            whereVals = whereVals.Remove(whereVals.Length - 4);

            var queryString = "SELECT distinct " + selectStr + " FROM " + table + " WHERE " + whereVals + " ORDER BY RAND() LIMIT 30;";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
                dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    result.Add(dataReader[selectStr] + "");            
                }
                
            }
            catch(MySqlException){}
            if (result.Count == 0){
                result.Add("Sorry, there are no results that match this search.\n Search for something else!");
            }
            //close Data Reader
            dataReader.Close();
            return result;
        }


        // getters
        public List<string> GetSportList()
        {
            return sportsList;
        }
        public List<string> GetGamesList()
        {
            return gamesList;
        }
        public List<string> GetTeamsList()
        {
            return teamsList;           
        }
        public List<string> GetHeightsList()
        {
            return heightsList;           
        }
        public List<string> GetWeightsList()
        {
            return weightsList;           
        }    
        public List<string> GetBirthYears()
        {
            return yearsList;           
        }
        

        // select column fynction
        public List<string> SelectColFromTable(string col, string table, string helper){
            string query =  "SELECT DISTINCT "+ col + " FROM olympicapp."+ table +  helper + ";";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            List<string> result =  new List<string>();
            try{
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result.Add(dataReader[col] + "");
                            
                }
                dataReader.Close();

            } catch(MySqlException){
                if(dataReader != null){
                    dataReader.Close();
                }
            }

            //close Data Reader
            return result;            
        }

        // feed functions
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
                check = TheBestXAthlete(sport, " AND  medal <> \"NA\"");
                if (check.Count > 0){
                    result += check[0];
                    result += ".<br> The best athlete is the athlete who won the most medals.";
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the heaviest athlete in the field of " + sport + " is ";
                temp = TheMostXAthlete(sport,"Weight","DESC");
                if (temp.Count > 0){
                    result += temp[0][0] + "?<br> This athlete weight is " + temp[0][1] + "kg.";
                    InsertIntoFeedTable(result,sport);
                }

                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the leanest athlete in the field of " + sport + " is ";
                temp = TheMostXAthlete(sport,"Weight","ASC");
                if (temp.Count > 0){

                    result += temp[0][0] + "?<br> This athlete weight is " + temp[0][1] + "kg.";
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the tallest athlete in the field of " + sport + " is ";
                temp = TheMostXAthlete(sport,"Height","DESC");
                if (temp.Count > 0){
                    result += temp[0][0] + "?<br> This athlete height is " + temp[0][1] + "cm.";
                    InsertIntoFeedTable(result,sport);
                }
                index = random.Next(sportsList.Count);
                sport = sportsList[index];
                result = "Did you know that the shortest athlete in the field of " + sport + " is ";
                temp = TheMostXAthlete(sport,"Height","ASC");
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
                check = LocationOfOlympicGame(game);
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
                //INSERT INTO olympicapp.feed (Post_content,Sport,Date)
                  //  VALUES ("test","test","2017-06-15");        
                string queryString = @"INSERT INTO olympicapp.feed (Post_content,Sport,Date) VALUES ('" + content + "',\"" +  sport + "\",\"" + date + "\");";
                try{
                    MySqlCommand cmd = new MySqlCommand(queryString, connection);
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {}    
                    //close Data Reader
                    dataReader.Close();
                }
                catch (MySqlException){

                }

                
        }
        
         
        public List<Post> FeedPosts(){
            GeneratePosts();
            string queryString = "SELECT * FROM olympicapp.feed ORDER BY RAND() LIMIT 10;";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            List<Post> posts =  new List<Post>();
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                Post p1 = new Post { PostId = Int32.Parse(dataReader["Post_id"] + ""), Content = dataReader["Post_content"] + "", Likes = 0, Date = DateTime.Parse(dataReader["Date"] + ""), Sport = dataReader["Sport"] + ""};
                posts.Add(p1);

            }
            //close Data Reader
             dataReader.Close();
             foreach( Post p in posts){
                p.Likes = GetNumberOfLikes(p.PostId.ToString());
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
            "GROUP BY Athlete_Id) AND " + parameter +" <> \"NA\") AS temp2 ORDER BY cast(" + parameter + " as unsigned) " + order + " LIMIT 4) AS temp3;";
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

       public string GetNumberOfAthletesFromTeam (string team)
        {
            var queryString = @"SELECT COUNT(distinct athletes.Name) as number FROM olympicapp.athletes WHERE athletes.Team='" + team +"';";
            string result = "";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result += dataReader["number"] + "";
                }
            }
            catch(MySqlException){}
            //close Data Reader
            dataReader.Close();
            return result;
        }
        public string GetDistinctEvents(string sport)
        {
            var queryString = @"SELECT COUNT(distinct event_types.event) as number FROM olympicapp.event_types WHERE event_types.Sport='" + sport + "';";
            string result = "";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
                dataReader = cmd.ExecuteReader();

                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result += dataReader["number"] + "";

                }
            } 
            catch(MySqlException){}
            //close Data Reader
            dataReader.Close();
            return result;
        }
        public string GetAvgOfGender(string sport, string gender)
        {
            var queryString = @"SELECT ROUND(AVG(athletes.Height),2) as avg FROM olympicapp.athletes WHERE athletes.Sex='" + gender +"'" +
                " AND NOT athletes.Height='NA' and Athlete_id IN" +
                "(SELECT Athlete_Id FROM olympicapp.medals WHERE((event_id IN " +
                "(SELECT event_id FROM olympicapp.event_types WHERE sport = '" + sport + "'))));";
            string result = "";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try {
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result += dataReader["avg"] + "";
                }
            }
            catch(MySqlException){}
            //close Data Reader
            dataReader.Close();
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
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
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
            dataReader.Close();

            return result;
        }
    
        // quiz

        public List<Question> GetQuestions(string sport){
            List<Question> questions = new List<Question>();
            //q1 who's the best athlete in the given sport
            List<string> theBestAthleteAnswers = TheBestXAthlete(sport, " AND  medal <> \"NA\"");
            if (theBestAthleteAnswers.Count < 4) {
                getWorngAnswers(sport, theBestAthleteAnswers);
            }
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
            string country = LocationOfOlympicGame(randomGame)[0];
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


        public void getWorngAnswers(string sport, List<string> answers){
            List<string> list = get4randomAthletesBySport(sport);
            for (int i = 0; i < 4; i++)
            {
                if (!answers.Contains(list[i])){
                    answers.Add(list[i]);
                }   
            }

        }
        public List<string> get4randomAthletesBySport(string sport){
            var queryString = "(SELECT DISTINCT olympicapp.athletes.Name, olympicapp.medals.Medal " +
                              "FROM olympicapp.medals JOIN olympicapp.event_types " +
                              "ON olympicapp.medals.Event_id = olympicapp.event_types.Event_id JOIN olympicapp.athletes " +
                              "ON olympicapp.athletes.Athlete_id = olympicapp.medals.Athlete_id " +
                              "WHERE olympicapp.event_types.Sport = '" + sport +"') " +
                              "ORDER BY RAND() LIMIT 4;";
                              List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result.Add(dataReader["Name"] + "");
                }
            }
            catch(MySqlException){}
            //close Data Reader
            dataReader.Close();
            return result;
        }
        public string GetXByYWhereZFromAthletes(string x, string y, string z){
            var queryString = @"SELECT " + x +" From olympicapp.athletes WHERE "+ y +" = '" + z + "';";
            string result = "";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result += dataReader[x] + "";             
                }
            }
            catch(MySqlException){}
            //close Data Reader
            dataReader.Close();
            return result;
        }

        public List<string> WrongYears(string year){
            var queryString = "SELECT Birth_year From olympicapp.athletes WHERE Birth_year <> \"" + year + "\" LIMIT 3";
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try {
                dataReader = cmd.ExecuteReader();

                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result.Add(dataReader["Birth_year"] + "");               
                }
            }
            catch(MySqlException){}
            //close Data Reader
            dataReader.Close();                
            return result;
        }

        public List<string> LocationOfOlympicGame(string game){
            var queryString = "SELECT Country,City FROM countries WHERE City =(SELECT City FROM olympic_games WHERE Game = \"" + game + "\")";;
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result.Add(dataReader["Country"] + "");
                    result.Add(dataReader["City"] + "");
                }
            }
            catch(MySqlException){}
            //close Data Reader
            dataReader.Close();
            return result;
        }
        public List<string> WrongCountries(string country){
            var queryString = "SELECT DISTINCT Country From olympicapp.countries WHERE Country <> \"" + country + "\" LIMIT 3;";
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try{
                dataReader = cmd.ExecuteReader();

                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result.Add(dataReader["Country"] + "");
                            
                }
            }
            catch(MySqlException){}
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
            try {
                dataReader = cmd.ExecuteReader();

                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result.Add(dataReader["Name"] + "");
                            
                }
            }
            catch(MySqlException){}
            //close Data Reader
            dataReader.Close();
            return result;

        }
        //users

        public User NewUserRegister(string username, string password){
            string queryString = "INSERT INTO olympicapp.users (User_name,Password,Is_admin) VALUES (\"" + username + "\",\"" +  password + "\",0);";            
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            User result = new User();
            try{
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {}
                result.Username = username;
                result.Password = password;
                result.isAdmin = false;
            }
            catch (MySqlException ex){
                Console.WriteLine(ex.Data);
                Console.WriteLine("alredy exist");
            }   
            //close Data Reader
            dataReader.Close();  
            return result;
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
            }
            catch (MySqlException){        
                Console.WriteLine("password or user name incorrect");
            }
            //close Data Reader
            dataReader.Close();
            return result;                
        }

        public void DeleteUser(string username)
        {
            // is admin
            bool isAdmin = false;
            if (username.Last() == '&')
            {
                isAdmin = true;
                username = username.Remove(username.Length - 1);
            }
            string queryString = " DELETE FROM olympicapp.users WHERE User_name = \"" + username + "\"";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try
            {
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) { }
            }
            catch (MySqlException)
            {
                Console.WriteLine("error while deleting this user");
            }
            if (isAdmin)
            {
                //delete from admin_permissions
                queryString = " DELETE FROM olympicapp.admin_permissions WHERE User_name = \"" + username + "\";";
                cmd = new MySqlCommand(queryString, connection);
                try
                {
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) { }
                }
                catch (MySqlException)
                {

                    Console.WriteLine("error while deleting this admin user");
                }
                //delete from likes
                queryString = " DELETE FROM olympicapp.likes WHERE User_name = \"" + username + "\";";
                cmd = new MySqlCommand(queryString, connection);
                try
                {
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) { }
                }
                catch (MySqlException)
                {
                    Console.WriteLine("error while deleting this admin user");
                }

            } 
            dataReader.Close();               
        }
        public bool ChangePassword(string username, string new_password){
      
            string queryString =" UPDATE olympicapp.users SET Password = \""+ new_password +"\" WHERE User_name = \""+ username +"\";";
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
            dataReader.Close();
            return false;


        }
    
        public bool UpdateAdmin(string user, string sport, bool isAdmin)
        {
            string queryString = "";
            MySqlCommand cmd;
            if(!isAdmin){
                queryString = "UPDATE olympicapp.users SET Is_admin = 1 WHERE User_name = " + user + "\";";
                cmd = new MySqlCommand(queryString, connection);
                try
                {
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) { }
                    dataReader.Close();
                    return true;
                }
                catch (MySqlException)
                {
                    dataReader.Close();
                    return false;
                }

            }
            queryString = "INSERT INTO admin_permissions (User_name, Sport) VALUES(\"" + user + "\",\"" + sport + "\");";
            cmd = new MySqlCommand(queryString, connection);
            try
            {
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) { }
                dataReader.Close();
                return true;
            }
            catch (MySqlException ex)
            {

                Console.WriteLine(ex.Data);
                Console.WriteLine("alredy exist");
                dataReader.Close();
                return false;
            }

        }

        public List<string> GetAdminList(string username)
        {
            string queryString = "SELECT Sport FROM olympicapp.admin_permissions WHERE User_name = \"" + username + "\";";
            List<string> result = new List<string>();
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try
            {
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    result.Add(dataReader["User_name"] + "");
                }
                //dataReader.Close();
                //return result;

            }
            catch (MySqlException)
            {
                Console.WriteLine("password or user name incorrect");
            }
            dataReader.Close();
            return result;

        }


         //likes
        public bool LikePost(string username, string post_id){
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
            dataReader.Close();
            return false;
        }
        public bool DislikePost(string username, string post_id)
        {
            string queryString = "DELETE FROM olympicapp.likes WHERE User_name='" + username + "' and Post_id = " + post_id + ";";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            try
            {
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) { }
                dataReader.Close();
                return true;
            }
            catch (MySqlException) {}
            dataReader.Close();
            return false;
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
                dataReader.Close(); 
                return 0;
            } 
        
        }

    }

}      
    