using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article_Project.Dtoes.Dto.Account
{
    public class TwoFactorDto
    {
        [Required(ErrorMessage ="کد امنیتی ارسال شده را وارد کنید")]
        [MaxLength(6,ErrorMessage ="کد وارد شده بایستی 6 رقم باشد")]
        public string Code { get; set; }
        [Required]
        public string Provider { get; set; }
        public bool IsPersistans { get; set; } = false;
    }
}
