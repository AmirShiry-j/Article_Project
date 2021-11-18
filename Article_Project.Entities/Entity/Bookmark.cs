using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Entities.Entity
{
    public class Bookmark
    {
        public long BookmarkId { get; set; }

        public long PostId { get; set; }
        public Post Post { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }


    }
}
