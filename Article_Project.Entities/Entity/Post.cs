using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Entities.Entity
{ 
    public class Post
    {

        public long PostId { get; set; }
        public string Title { get; set; }
        public string Texts { get; set; }
        public string ImageNames { get; set; }
        public string Orders { get; set; }
        public DateTime TimeCreate { get; set; }
        public string Subject { get; set; }
        public string Tags { get; set; }
        public long Views { get; set; } = 0;


        //Nav
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }

    }
}
