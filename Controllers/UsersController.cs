using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using olympic_app.Models;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;


namespace olympic_app.Controllers

{
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class UsersController : ControllerBase
    {
        private IAppManager manager;
        public UsersController(IAppManager manger)
        {
            this.manager = manger;
        }
        
        [HttpGet ("{username}")]
        // /api/Users
        [ActionName("login")]

        //returns user with the info if this user is admin      
        public User Post(string username)
        {
            string[] temp = username.Split('&', 2);
            string user_name = temp[0];
            string password = temp[1];
            return manager.UserLogin(user_name, password);
        }
        
        [HttpPost("{username}")]
        // /api/Users/sign_up
        [ActionName("sign_up")]
        public User SignupPost(string username)
        {
            string[] temp = username.Split('&', 2);
            string user_name = temp[0];
            string password = temp[1];
            return manager.UserSignup(user_name, password);
        }
       [HttpPost("{update_psw}")]
        // /api/Users/change_password
        [ActionName("change_password")]
        public bool UpdatePassword(string update_psw)
        {
            string[] temp = update_psw.Split('&', 2);
            string username = temp[0];
            string password = temp[1];
            return manager.ChangePassword(username, password);
        }

        [HttpDelete("{username}")]
        [ActionName("delete")]
        // /api/Users
        public void DeleteUser(string username)
        {
            manager.DeleteUser(username);
        }

        [HttpPost("{update}")]
        // /api/Users/admin/username&sport&true
        [ActionName("admin")]
        public bool UpdateAdmin(string update)
        {
            string[] temp = update.Split('&', 3);
            string username = temp[0];
            string sport = temp[1];
            string isAdmin = temp[2];
            return manager.UpdateAdmin(username, sport, bool.Parse(isAdmin));
        }
        


        [HttpGet("{username}")]
        [ActionName("adminlist")]
        public List<string> GetAdminList(string username){
            return manager.GetAdminList(username);

        }

    }
}