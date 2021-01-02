using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace olympic_app.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public DateTime Date { get; set; }

    }
}
