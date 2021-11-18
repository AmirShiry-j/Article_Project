using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.Account
{
    public class ResetPasswordDto
    {

        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد کنید")]
        [DataType(DataType.Password, ErrorMessage = "قالب پسورد وارد شده صحیح نیست")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا تکرار رمز عبور خود را وارد کنید")]
        [DataType(DataType.Password, ErrorMessage = "قالب پسورد وارد شده صحیح نیست")]
        [Compare(nameof(Password), ErrorMessage = "رمز عبور و تکرار آن با هم مطابقت ندارند")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
