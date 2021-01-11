
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

namespace olympic_app
{
    class PostsAndLikes
    {  
        static void Main(string[] args)
        {
            Console.WriteLine("satartttt");
            DBConnect dbConnect = new DBConnect();
            bool connection = dbConnect.OpenConnection();
            if(!connection) {
                Console.WriteLine("could not connect to the SQL Server");
            }
            MySqlConnection conn = dbConnect.GetConnection();
            DBUsers dbUsers = new DBUsers(conn);
            DBGeneral dbGeneral = new DBGeneral(conn);
            DBFeed dbFeed = new DBFeed(conn, dbGeneral);
            DBSearch dbSearch = new DBSearch(conn);
            DBQuiz dbQuiz = new DBQuiz(conn, dbGeneral);
            // creates 100 posts
            dbFeed.GeneratePosts();
            
            //get lists of users
            string queryString ="SELECT User_name FROM olympicapp.users;";
            List<string> users = new List<string>();
            try
            {
                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    users.Add(dataReader["User_name"] + "");
                }
                dataReader.Close();

            }
            catch (MySqlException)
            {
                Console.WriteLine("cant get users");
            }
            //get a list of number of posts
            queryString ="SELECT Post_id FROM olympicapp.feed;";
            List<string> posts_id = new List<string>();
            try
            {
                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    posts_id.Add(dataReader["Post_id"] + "");
                }
                dataReader.Close();

            }
            catch (MySqlException)
            {
                Console.WriteLine("cant get post_id");
            }
            // do likes

            for (int i = 0; i < users.Count; i++)
            {
                for (int j = 0; j < posts_id.Count; j++)
                {
                    Random gen = new Random();
                    int prob = gen.Next(100);
                    // do like randomly
                    if(prob <= 20) {
                        dbFeed.LikePost(users[i],posts_id[j]);
                    }
                }
            }
            
        }       
    }
}

