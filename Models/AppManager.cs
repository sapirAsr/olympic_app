using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using olympic_app.Models;
using olympic_app.DB;


namespace olympic_app.Models
{
    public class AppManager : IAppManager
    {
        private DBConnect dBConnect;

        public AppManager(){
            dBConnect = new DBConnect();
            dBConnect.OpenConnection();


        }
        public List<Post> getPosts()
        { 
            List< Post > list = dBConnect.FeedPosts(); 
            return list;
        }
    }
}
