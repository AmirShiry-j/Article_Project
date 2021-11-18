using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Entities.Entity
{
    public class Comment
    {
        public long CommentId { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }

        public string UserId { get; set; }
        public long PostId {  get; set; }
        public long? ReplyComment { get; set; }
        //Nav
        public User User { get; set; }
        public Post Post { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
