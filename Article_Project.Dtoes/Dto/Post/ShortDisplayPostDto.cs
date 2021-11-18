using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.Post
{
    public class ShortDisplayPostDto
    {
        public long PostId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string UserId { get; set; }
        public DateTime TimeCreate { get; set; }
    }
}
