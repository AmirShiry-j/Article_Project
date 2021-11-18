using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.Post
{
    public class CreadPostDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        //[StringLength(300)]
        public string Texts { get; set; }
        [Required]
        public string Orders { get; set; }
        [Required]
        public string Subject { get; set; }
        public string Tags { get; set; }

    }
}
