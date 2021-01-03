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
        bool UserLogin(string username, string password);
        bool UserSignup(string username, string password);

        bool LikePost(string username, int post_id);

    }
}
