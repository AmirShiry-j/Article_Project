using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.User
{
    public class ProfileDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NameShow { get; set; }
        public string Biography { get; set; }
        public string ImageProfileName { get; set; }
        public int CountFollowers { get; set; }
        public int CountFollowing { get; set; }
        public int CountPosts { get; set; }


    }
}
