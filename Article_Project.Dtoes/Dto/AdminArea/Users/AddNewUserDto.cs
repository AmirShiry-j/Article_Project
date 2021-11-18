using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.AdminArea.Users
{
    public class AddNewUserDto
    {
        [Required(ErrorMessage = "لطفا ایمیل کاربر را وارد کنید")]
        [EmailAddress(ErrorMessage = "قالب ایمیل وارد شده صحیح نیست")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "لطفا نام نمایشی کاربر را وارد کنید")]
        [MaxLength(15, ErrorMessage = "نام نمایشی شما حد اکثر میتواند 15 کاراکتر باشد")]
        public string NameShow { get; set; }

        [Required(ErrorMessage = "لطفا رمز عبور کاربر را وارد کنید")]
        [DataType(DataType.Password, ErrorMessage = "قالب پسورد وارد شده صحیح نیست")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا تکرار رمز عبور کاربر را وارد کنید")]
        [DataType(DataType.Password, ErrorMessage = "قالب پسورد وارد شده صحیح نیست")]
        [Compare(nameof(Password), ErrorMessage = "رمز عبور و تکرار آن با هم مطابقت ندارند")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
