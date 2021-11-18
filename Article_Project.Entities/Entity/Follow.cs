using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Entities.Entity
{
    public class Follow
    {
        public long FollowId { get; set; }


        public string FollowTo { get; set; }
        public User UserTo { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

    }
}
