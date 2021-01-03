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

        // GET: api/Feed/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Feed
        [HttpPost]
        public void Post(string username, int post_id)
        {
        }

        // PUT: api/Feed/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

    }
}