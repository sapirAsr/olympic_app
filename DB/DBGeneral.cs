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
    public class DBGeneral
    {
        private MySqlConnection connection;
        private MySqlDataReader dataReader;
        // lists for the app to use
        private List<string> sportsList = new List<string>();
        private List<string> gamesList = new List<string>();
        private List<string> teamsList = new List<string>();
        private List<string> heightsList = new List<string>();
        private List<string> weightsList = new List<string>();
        private List<string> yearsList = new List<string>();
        public DBGeneral(MySqlConnection conn)
        {
            connection = conn;
            SetLists();
        }

        //setting all lists for the app to use only one time
        public void SetLists(){
            sportsList = SelectColFromTable("Sport","event_types", " ORDER BY Sport ASC");
            gamesList = SelectColFromTable("Game","olympic_games", " ORDER BY Game ASC");
            teamsList =  SelectColFromTable("Team","athletes"," ORDER BY Team ASC");
            heightsList =  SelectColFromTable("Height","athletes"," WHERE Height<>'NA' ORDER BY Height ASC");
            weightsList =  SelectColFromTable("Weight","athletes", " WHERE Weight<>'NA' ORDER BY cast(Weight as unsigned)  ASC");
            yearsList =  SelectColFromTable("Birth_year","athletes"," WHERE Birth_year<>'NA' ORDER BY Birth_year ASC");

        }
         // select column from a specific table, helper can be apllied for more conditions
        public List<string> SelectColFromTable(string col, string table, string helper){
            string query =  "SELECT DISTINCT "+ col + " FROM olympicapp."+ table +  helper + ";";
            List<string> result =  new List<string>();
            try{
                MySqlCommand cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result.Add(dataReader[col] + "");                           
                }
            } 
            catch(MySqlException){}
            if(dataReader != null){
                dataReader.Close();
            }
            return result;            
        }

        //getters of the lists
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
        
        // function gets a sport a parameter and order
        // and returns the names of the athletes in the sport that are the most by the parameter and order
        public List<List<string>> TheMostXAthlete(string sport, string parameter, string order){

            var queryString = "SELECT Name,"+ parameter +" FROM" +
            "(SELECT Athlete_id, Name, "+ parameter +" FROM " +
            "(SELECT Athlete_id, Name, "+ parameter +" FROM olympicapp.athletes AS temp WHERE Athlete_id IN " + 
            "(SELECT Athlete_Id FROM olympicapp.medals WHERE (event_id IN " +
            "(SELECT event_id FROM olympicapp.event_types WHERE sport = \"" + sport + "\"" + "))" +
            "GROUP BY Athlete_Id) AND " + parameter +" <> \"NA\") AS temp2 ORDER BY cast(" + parameter + " as unsigned) " + order + " LIMIT 4) AS temp3;";
            List<List<string>> result = new List<List<string>>();
            try{
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                //Read the data and store the name in string
                while (dataReader.Read())
                {
                    result.Add(new List<string> {dataReader["Name"] + "",dataReader[parameter] + ""});
                }
            }
            catch (MySqlException){}
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            }   
            return result;
        }
      
       //function gets a sport and helper
       //gets the names of the 4 best athlete with the helper  AND  medal <> \"NA\"" "
       //gets the names of 4 most participants with the helper -""
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

        // function gets a game and returns where it took place (city,country)
        public List<string> LocationOfOlympicGame(string game){
            var queryString = "SELECT Country,City FROM countries WHERE City =(SELECT City FROM olympic_games WHERE Game = \"" + game + "\")";;
            List<string> result = new List<string>();
            try{
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
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
            if(dataReader != null){
                dataReader.Close();
            } 
            return result;
        }

    }
}