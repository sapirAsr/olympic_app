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
    public class DBUsers
    {
        private MySqlConnection connection;
        private MySqlDataReader dataReader;

        //Constructor
        public DBUsers(MySqlConnection conn)
        {
            connection = conn;
        }

        // function gets username and password 
        //and returns user object if its null the user was already exist
        public User NewUserRegister(string username, string password){
            string queryString = "INSERT INTO olympicapp.users (User_name,Password,Is_admin) VALUES (\"" + username + "\",\"" +  password + "\",0);";            
            User result = new User();
            try{
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
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
            if(dataReader != null){
                dataReader.Close();
            }  
            return result;
        }
        // function gets username and password 
        //and returns user object if its null the user doesnt exist
        public User Login(string username, string password){
            string queryString ="SELECT User_name, Password, Is_admin FROM olympicapp.users WHERE User_name = \"" + username + "\" AND Password = \"" + password + "\"";
            User result = new User();
            try{
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
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
            if(dataReader != null){
                dataReader.Close();
            } 
            return result;                
        }

        //function gets username and delete the user from users table
        // if the user is admin deletes it from the admin table too.
        // and deletes its likes on posts
        public void DeleteUser(string username)
        {
            // is admin
            bool isAdmin = false;
            MySqlCommand cmd;
            if (username.Last() == '&')
            {
                isAdmin = true;
                username = username.Remove(username.Length - 1);
            }
            string queryString = " DELETE FROM olympicapp.users WHERE User_name = \"" + username + "\"";
            try
            {
                cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) { }
                dataReader.Close();               
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
                    dataReader.Close(); 
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
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            }               
        }
        
        // function changes password of user
        // return true if the password changed else false
        public bool ChangePassword(string username, string new_password){
      
            string queryString =" UPDATE olympicapp.users SET Password = \""+ new_password +"\" WHERE User_name = \""+ username +"\";";
            try
            {
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) {}
                dataReader.Close();
                return true;
            }
            catch (MySqlException ){      
                Console.WriteLine("error while deleting this user");
            }
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            } 
            return false;


        }
    

        // function update admin users in the admin table and if needed in the users table
        //return true if the the update went successfully changed else false
        public bool UpdateAdmin(string user, string sport, bool isAdmin)
        {
            string queryString = "";
            MySqlCommand cmd;
            if(!isAdmin){
                queryString = "UPDATE olympicapp.users SET Is_admin = 1 WHERE User_name = \"" + user + "\";";
                cmd = new MySqlCommand(queryString, connection);
                try
                {
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read()) { }
                    dataReader.Close();
                }
                catch (MySqlException)
                {
                    //close Data Reader
                    if(dataReader != null){
                        dataReader.Close();
                    } 
                    return false;
                }

            }
            queryString = "INSERT INTO admin_permissions (User_name, Sport) VALUES(\"" + user + "\",\"" + sport + "\");";
            try
            {
                cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read()) { }
                dataReader.Close();
                return true;
            }
            catch (MySqlException)
            {
                //close Data Reader
                if(dataReader != null){
                    dataReader.Close();
                } 
                return false;
            }

        }

        // function returns a list of sports that the username has admin permissions
        public List<string> GetAdminList(string username)
        {
            string queryString = "SELECT Sport FROM olympicapp.admin_permissions WHERE User_name = \"" + username + "\";";
            List<string> result = new List<string>();
            try
            {
                MySqlCommand cmd = new MySqlCommand(queryString, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    result.Add(dataReader["Sport"] + "");
                }

            }
            catch (MySqlException)
            {
                Console.WriteLine("password or user name incorrect");
            }
            //close Data Reader
            if(dataReader != null){
                dataReader.Close();
            } 
            return result;
        }
    }
}