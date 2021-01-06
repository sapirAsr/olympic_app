using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using olympic_app.Models;
namespace olympic_app.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private IAppManager manager;
        public FeedController(IAppManager manger)
        {
            this.manager = manger;
        }
        // GET: api/Feed
        [HttpGet]
        public List<Post> Get()
        {
            return manager.getPosts();
        }
        [HttpGet ("{post_id}")]
        // /api/Feed/14
        public int GetNumberOfLikes(string post_id)
        {
            return manager.GetNumberOfLikes(post_id);
        } 
        
        [HttpPost ("{details}")]
        // /api/Feed/sapir&14
        public bool LikePost(string details)
        {
            string[] temp = details.Split('&');
            string username = temp[0];
            string post_id = temp[1];
            return manager.LikePost(username,post_id);
        }
    }
}