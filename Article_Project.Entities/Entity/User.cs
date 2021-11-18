using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Entities.Entity
{
    public class User:IdentityUser
    {
        public string NameShow { get; set; }
        public string Biography { get; set; }
        public string MobilePhone { get; set; }
        public string ImageProfileName { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Follow> Followings { get; set; }
        public ICollection<Follow> Followers { get; set; }

    }

}
