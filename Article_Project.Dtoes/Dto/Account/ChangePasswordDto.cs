using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.Account
{
    public class ChangePasswordDto
    {

        [Required(ErrorMessage = "لطفا رمز عبور فعلی خود را وارد کنید")]
        [DataType(DataType.Password, ErrorMessage = "قالب پسورد وارد شده صحیح نیست")]
        public string NowPassword { get; set; }


        [Required(ErrorMessage = "لطفا رمز عبور جدید خود را وارد کنید")]
        [DataType(DataType.Password, ErrorMessage = "قالب پسورد وارد شده صحیح نیست")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "لطفا تکرار رمز عبور جدید خود را وارد کنید")]
        [DataType(DataType.Password, ErrorMessage = "قالب پسورد وارد شده صحیح نیست")]
        [Compare(nameof(NewPassword), ErrorMessage = "رمز عبور جدید شما و تکرار آن با هم مطابقت ندارند")]
        public string ConfirmPassword { get; set; }
    }
}
