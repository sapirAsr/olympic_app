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


        public List<Post> FeedPosts(){
            Post p1 = new Post { PostId = 12, Content = TheBestAthlete("Basketball"), Likes = 3 };
            Post p2 = new Post { PostId = 13, Content = TheBestAthlete("Swimming"), Likes = 13 };
            List<Post> posts = new List<Post>();
            posts.Add(p1);
            posts.Add(p2);
            return posts;
        }


        public string TheMostXAthlete(string sport, string parameter, string order){

             var queryString = "SELECT Name FROM" +
            "(SELECT Athlete_id, Name, Height FROM " +
            "(SELECT Athlete_id, Name, Height FROM olympicapp.athletes AS temp WHERE Athlete_id IN " + 
            "(SELECT Athlete_Id FROM olympicapp.medals WHERE (event_id IN " +
            "(SELECT event_id FROM olympicapp.event_types WHERE sport = \"" + sport + "\"" + "))" +
            "GROUP BY Athlete_Id) AND Height <> \"NA\") AS temp2 ORDER BY " + parameter + " " + order + " LIMIT 1) AS temp3;";
            Console.WriteLine(queryString);
            string result = "" ;
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



        //the best athlete in specific sport

        public string TheBestAthlete(string sport){

            var queryString = "SELECT Name FROM olympicapp.athletes WHERE Athlete_Id = (SELECT Athlete_Id FROM (" +
            "SELECT Athlete_Id, COUNT(*) AS magnitude FROM (SELECT Athlete_Id, Medal FROM olympicapp.medals WHERE ((event_id IN " +
            "(SELECT event_id FROM olympicapp.event_types WHERE sport = \"" + sport + "\"" + ")) AND  medal <> \"NA\")) AS temp " +
            "GROUP BY Athlete_Id " +
             "ORDER BY magnitude DESC " +
            "LIMIT 1) AS temp2);";
            string result = "The best athlete in the field of " + sport + " is ";
            MySqlCommand cmd = new MySqlCommand(queryString, connection);
            dataReader = cmd.ExecuteReader();

            //Read the data and store the name in string
            while (dataReader.Read())
            {
                result += dataReader["Name"] + "";
                          
            }
            //close Data Reader
             dataReader.Close();
            
            result += ". The best athlete is the athlete who won the most medals.";

             return result;

        }
    
    }
}      
    