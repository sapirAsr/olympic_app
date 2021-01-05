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
        
        [HttpPost]
        // /api/Users
        [ActionName("login")]

        //returns user with the info if this user is admin      
        public User Post(User user)
        {
            return manager.UserLogin(user.Username, user.Password);
        }
        
        [HttpPost]
        // /api/Users/sign_up
        [ActionName("sign_up")]
        public bool SignupPost(User user)
        {
            //User user = new User();
            return manager.UserSignup(user.Username, user.Password);
        }
        [HttpPost]
        // /api/Users/change_password
        [ActionName("change_password")]
        public bool UpdatePassword(User user)
        {
            return manager.ChangePassword(user.Username, user.Password);
        }
        [HttpDelete("{username}")]
        [ActionName("delete")]
        // /api/Users
        public void DeleteUser(string username)
        {
            manager.DeleteUser(username);
        }

        [HttpPost]
        // /api/Users/sign_up
        [ActionName("admin")]
        public bool UpdateAdmin(User user,string sport, bool isAdmin)
        {
            return manager.UpdateAdmin(user, sport, isAdmin);
        }

    }
}