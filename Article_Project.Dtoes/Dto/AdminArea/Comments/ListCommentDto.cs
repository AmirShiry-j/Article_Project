using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.AdminArea.Comments
{
    public class ListCommentDto
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public bool IsReply { get; set; }
        public int CountReplys { get; set; }
        public long postId { get; set; }
    }
}
