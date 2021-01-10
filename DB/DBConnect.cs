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

        //Initialize values
        public bool OpenConnection()
        {        
            string connectionString = "Server=127.0.0.1;Database=olympicapp;User Id=root;Password=Sapir1912" ;
            connection = new MySqlConnection(connectionString);
            try {
                connection.Open();   
                return true;
            }
            catch (MySqlException) {
                return false;
            }

        }
         //Close connection
        public bool CloseConnection()
        {
            try {
                connection.Close();
                return true;
            }
            catch (MySqlException) {
                return false;
            }

        }
    
        //getter connection
        public MySqlConnection GetConnection(){
            return connection;
        }
    }
}
