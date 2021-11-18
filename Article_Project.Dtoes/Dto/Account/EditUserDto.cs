using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.Account
{
    public class EditUserDto
    {

        [Required(ErrorMessage = "لطفا نام کاربری خود را وارد کنید")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "لطفا نام نمایشی خود را وارد کنید")]
        [MaxLength(15, ErrorMessage = "نام نمایشی شما حد اکثر میتواند 15 کاراکتر باشد")]
        public string NameShow { get; set; }

        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
        [EmailAddress(ErrorMessage = "قالب ایمیل وارد شده صحیح نیست")]
        public string Email { get; set; }

        [MaxLength(200,ErrorMessage ="بیوگرافی شما حد اکثر میتواند 200 کاراکتر باشد")]
        public string Biography { get; set; }

        [RegularExpression(@"^(09)[0-9]{9}$",ErrorMessage ="قالب شماره موبایل وارد شده صحیح نیست")]
        public string MobilePhone { get; set; }
        public string NameImageProfile { get; set; }

        public bool TwoFactor { get; set; }


    }
}
