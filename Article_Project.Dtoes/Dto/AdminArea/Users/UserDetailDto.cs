using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.AdminArea.Users
{
    public class UserDetailDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool VerifyEmail { get; set; }
        public string Roles { get; set; }
        public bool IsLocked { get; set; }
        public bool TwoFactor { get; set; }
        public int CountPosts { get; set; }
        public int CountComment { get; set; }
        public string ImageProName { get; set; }

    }
}
