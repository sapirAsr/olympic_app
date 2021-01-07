using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using olympic_app.DB;

namespace olympic_app.Models
{
    public interface IAppManager
    {
        //feed
        List<Post> getPosts(); 
        int GetNumberOfLikes(string post_id);  
        //search
        List<string> GetSportList();
        List<string> GetGamesList();
        List<string> GetTeamsList();
        List<string> GetHeightsList();
        List<string> GetWeightsList();
        List<string> GetBirthYears();
        string GetBestAthlete(string sport);
        List<string> GetLocationGame(string game);
        List<string> GetTheMostXAthlete(string sport, string parameter, string order);
        List <string> BasicFilter(Dictionary<string, string> dictAtr);
        //quiz
        List<Question> GetQuestions(string sport);
        //users
        User UserLogin(string username, string password);
        User UserSignup(string username, string password);
        bool LikePost(string username, string post_id);
        bool DislikePost(string username, string post_id);

        void DeleteUser(string username);
        bool ChangePassword(string username, string password);
        bool UpdateAdmin(string user,string sport, bool isAdmin);
        List<string> GetAdminList(string username);

    }
}
