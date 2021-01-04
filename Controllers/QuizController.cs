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
    public class QuizController : ControllerBase
    {
        private IAppManager manager;
        public QuizController(IAppManager manger)
        {
            this.manager = manger;
        }
        // GET: api/Feed
        [HttpGet ("{sport}", Name = "Get")]
        public List<Question> GetQuestions(string sport)
        {
            return manager.GetQuestions(sport);
        }


    }
}