using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using olympic_app.DB;

namespace olympic_app.Models
{
    public interface IAppManager
    {
        List<Post> getPosts();
        User UserLogin(string username, string password);
        bool UserSignup(string username, string password);
        bool LikePost(string username, int post_id);
        bool DeleteUser(User user);
        bool ChangePassword(string username, string password);
        bool UpdateAdmin(User user,string sport, bool isAdmin);
        List<string> GetSportList();
        List<Question> GetQuestions(string sport);


    }
}
