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
        //search
        List<string> GetSportList();
        List<string> GetGamesList();
        string GetBestAthlete(string sport);
        List<string> GetLocationGame(string game);
        List<string> GetTheMostXAthlete(string sport, string parameter, string order);
        List <string> [] BasicFilter(string table, List<string> atributes);

        //quiz
        List<Question> GetQuestions(string sport);
        //users
        User UserLogin(string username, string password);
        bool UserSignup(string username, string password);
        bool LikePost(string username, int post_id);
        void DeleteUser(string username);
        bool ChangePassword(string username, string password);
        bool UpdateAdmin(User user,string sport, bool isAdmin);
        List<string> GetAdminList(string username);

    }
}
