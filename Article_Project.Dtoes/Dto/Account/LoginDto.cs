using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.Account
{
    public class LoginDto
    {
        [Required(ErrorMessage ="لطفا ایمیل خود را وارد کنید")]
        [EmailAddress(ErrorMessage ="قالب ایمیل وارد شده صحیح نیست")]
        public string Email { get; set; }


        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد کنید")]
        [DataType(DataType.Password,ErrorMessage ="قالب پسورد وارد شده صحیح نیست")]
        public string Password { get; set; }

        public bool IsPersistens { get; set; } = false;
    }
}
