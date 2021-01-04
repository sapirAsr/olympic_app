using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using olympic_app.Models;
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

        public bool Post(User user)
        {
            return manager.UserLogin(user.Username, user.Password);
        }
        
        [HttpPost]
        // /api/Users/sign_up
        [ActionName("sign_up")]
        public bool SignupPost(User user)
        {
            return manager.UserSignup(user.Username, user.Password);
        }
        [HttpPost]
        // /api/Users/change_password
        [ActionName("change_password")]
        public bool UpdatePassword(User user)
        {
            return manager.ChangePassword(user.Username, user.Password);
        }
        [HttpDelete]
        // /api/Users
        public bool DeleteUser(User user, bool isAdmin)
        {
            return manager.DeleteUser(user.Username, user.Password, isAdmin);
        }

    }
}