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
    public class SearchController : ControllerBase
    {
        private IAppManager manager;
        public SearchController(IAppManager manger)
        {
            this.manager = manger;
        }

        [HttpGet]
        [ActionName("sportslist")]

        public List<string> GetSports()
        {
            return manager.GetSportList();
        }

        [HttpGet]
        [ActionName("gameslist")]

        public List<string> GetGames()
        {
            return manager.GetGamesList();
        }
        [HttpGet]
        [ActionName("teamslist")]

        public List<string> GetTeams()
        {
            return manager.GetTeamsList();
        }
        [HttpGet]
        [ActionName("heightslist")]

        public List<string> GetHeights()
        {
            return manager.GetHeightsList();
        }
        [HttpGet]
        [ActionName("weightslist")]

        public List<string> GetWeights()
        {
            return manager.GetWeightsList();
        }
        [HttpGet]
        [ActionName("yearslist")]

        public List<string> GetBirthYears()
        {
            return manager.GetBirthYears();
        }

        [HttpGet ("{sport}")]
        // /api/Users/best_athlete
        [ActionName("best_athlete")]
        public string BestAthleteSport(string sport)
        {
            return manager.GetBestAthlete(sport);
        } 
        [HttpGet ("{game}")]
        // /api/Users/best_athlete
        [ActionName("location")]
        public List<string> GetLocationGame(string game)
        {
            game = game.Insert(4," ");
            return manager.GetLocationGame(game);
        }

        [HttpGet ("{query}")]
        // /api/Users/query/Basketball&Height&ASC
        [ActionName("the_most")]
        public List<string> TheMostX(string query)
        {
            string[] temp = query.Split('&',3);
            //sport,"Height","ASC"
            string sport = temp[0];
            string parameter = temp[1];
            string order = temp[2];
            return manager.GetTheMostXAthlete(sport, parameter, order);
        }

        [HttpGet ("{query}")]
        // /api/Users/filter/search=athletes&Team=china
        [ActionName("filter")]
        public List<string> Filter(string query)
        {
            Dictionary<string, string> dictAtr = new Dictionary<string, string>();
            string[] tempSplit = query.Split('&');
            for (int i = 0; i < tempSplit.Length; i++)
            {
                string h = tempSplit[i];
                string[] temp = h.Split('-');
                dictAtr.Add(temp[0], temp[1]);

            }
            return manager.Filter(dictAtr);
        }

/**
        
        [HttpPost]
        // /api/Search/basic_filter
        [ActionName("basic_filter")]
        //returns user with the info if this user is admin   
        public List <string> [] BasicFilter(string table, List<string> atributes)
   
        public User Post(string )
        {
            return manager.UserLogin(user.Username, user.Password);
        }
        
        [HttpPost]
        // /api/Users/sign_up
        [ActionName("best_athlete")]
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
        */

    }
}