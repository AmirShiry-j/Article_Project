using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.Account
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
        [EmailAddress(ErrorMessage = "قالب ایمیل وارد شده صحیح نیست")]
        public string Email { get; set; }
    }
}
