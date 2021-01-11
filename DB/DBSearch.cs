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
    public class DBSearch
    {
        private MySqlConnection connection;
        private MySqlDataReader dataReader;

        //Constructor
        public DBSearch(MySqlConnection conn)
        {
            connection = conn;
        }
       
       // gets a dictionary of values to search for
       // returns a list of the results
        public List <string> Filter(Dictionary<string, string> dictAtr)
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
            // if we need to compare sport we need another table
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
            try{
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    result.Add(dataReader[selectStr] + "");            
                }
                
            }
            catch(MySqlException){}
            if (result.Count == 0){
                result.Add("Sorry, there are no results that match this search.<br>Search for something else!");
            }
           //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            }   
            return result;
        }
    }
}