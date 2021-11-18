using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.Comment
{
    public class CreateCommentDto
    {
        [Required]
        public long PostId { get; set; }
        [Required]
        [MinLength(1)]
        public string TextComment { get; set; }
        public long? ReplayComment { get; set; }
    }
}
