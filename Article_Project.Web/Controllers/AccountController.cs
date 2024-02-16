using Article_Project.Dtoes.Dto.Account;
using Article_Project.Entities.Entity;
using Article_Project.Web.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.Controllers
{
    [Route("/{Controller}/{Action}/")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IServiceAccount _serviceAccount;

        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager
            , SignInManager<User> signInManager
            , ILogger<AccountController> logger
            ,IServiceAccount serviceAccount)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _logger = logger;

            _serviceAccount = serviceAccount;
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDto registerUser)
        {
            if (ModelState.IsValid == false)
            {
                return View(registerUser);
            }

            _signInManager.SignOutAsync();

            User newUser = new User()
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
                NameShow = registerUser.NameShow,
                EmailConfirmed = true
            };

            var result = _userManager.CreateAsync(newUser, registerUser.Password).Result;

            if (result.Succeeded)
            {
                ////Confirm Email
                //TempData["Email"] = newUser.Email;
                //return RedirectToAction("ConfirmEmail");

                var resultLogin = _signInManager.PasswordSignInAsync(newUser, registerUser.Password, true, true).Result;
                if (resultLogin.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                string Message = "";
                foreach (var er in result.Errors)
                {
                    Message += er.Description;
                }

                ModelState.AddModelError(string.Empty, Message);

                return View(registerUser);
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDto loginUser)
        {
            if (ModelState.IsValid == false)
            {
                return View(loginUser);
            }

            _signInManager.SignOutAsync();

            var user = _userManager.FindByEmailAsync(loginUser.Email).Result;

            if (user == null)
            {
                ModelState.AddModelError("", "کاربری با این ایمیل یافت نشد");
                return View(loginUser);
            }

            var result = _signInManager.PasswordSignInAsync(user, loginUser.Password, loginUser.IsPersistens, true).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "حساب کاربری شما قفل شد");

                return View(loginUser);
            }
            else if (result.IsNotAllowed)
            {

                TempData["Email"] = user.Email;

                return RedirectToAction("ConfirmEmail");
            }
            if (result.RequiresTwoFactor)
            {
                TempData["UserId"] = user.Id;
                TempData["IsPersistans"] = loginUser.IsPersistens;

                return RedirectToAction("TwoFactorLogin");
            }
            else
            {
                ModelState.AddModelError("", "پسورد وارد شده صحیح نیست");

                return View(loginUser);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit()
        {

            User user = _userManager.GetUserAsync(User).Result;

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            EditUserDto infoEditUser = new EditUserDto()
            {
                UserName = user.UserName,
                NameShow = user.NameShow,
                Email = user.Email,
                MobilePhone = user.MobilePhone,
                Biography = user.Biography,
                NameImageProfile = user.ImageProfileName,
                TwoFactor = user.TwoFactorEnabled
            };

            return View(infoEditUser);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(EditUserDto infoEditUser, IFormFile imgFile)
        {
            if (ModelState.IsValid == false)
            {
                return View(infoEditUser);
            }

            User userForUpdateImgProfile = _userManager.GetUserAsync(User).Result;

            if (userForUpdateImgProfile == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (imgFile != null)
            {
                if (userForUpdateImgProfile.ImageProfileName != null)
                {
                    _serviceAccount.DeleteImageProfile(userForUpdateImgProfile.ImageProfileName);
                }

                string newNameImage;
                string newPath;
                do
                {
                    newNameImage = Guid.NewGuid() + Path.GetExtension(imgFile.FileName);
                    newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ImgProfile/", newNameImage);
                }
                while (Directory.Exists(newPath));

                using (var newFileStream = new FileStream(newPath, FileMode.Create))
                {
                    imgFile.CopyToAsync(newFileStream).Wait();
                }

                userForUpdateImgProfile.ImageProfileName = newNameImage;

                var resultUpdateImageProfile = _userManager.UpdateAsync(userForUpdateImgProfile).Result;

                infoEditUser.NameImageProfile = newNameImage;
            }


            userForUpdateImgProfile.UserName = infoEditUser.UserName;
            userForUpdateImgProfile.NameShow = infoEditUser.NameShow;
            //userForUpdateImgProfile.Email = infoEditUser.Email;
            userForUpdateImgProfile.MobilePhone = infoEditUser.MobilePhone;
            userForUpdateImgProfile.Biography = infoEditUser.Biography;
            userForUpdateImgProfile.TwoFactorEnabled = infoEditUser.TwoFactor;

            var result = _userManager.UpdateAsync(userForUpdateImgProfile).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Profile", "User", new { userForUpdateImgProfile.UserName });
            }
            else
            {
                string Message = "";

                foreach (var error in result.Errors)
                    Message += error.Description;

                ModelState.AddModelError("", Message);

                return View(infoEditUser);
            }
        }

        [Authorize]
        public bool DeleteImageProfile()
        {
            User user = _userManager.GetUserAsync(User).Result;

            if (user == null)
            {
                return false;
            }

            if (user.ImageProfileName != null)
            {
                _serviceAccount.DeleteImageProfile(user.ImageProfileName);

                user.ImageProfileName = null;

                var result = _userManager.UpdateAsync(user).Result;

                return result.Succeeded;
            }

            return false;
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordDto changePassword)
        {
            if (ModelState.IsValid == false)
            {
                return View(changePassword);
            }

            var user = _userManager.GetUserAsync(User).Result;

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var result = _userManager.ChangePasswordAsync(user, changePassword.NowPassword, changePassword.NewPassword).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Edit");
            }
            else
            {
                string Message = "";
                foreach (var error in result.Errors)
                    Message += error.Description;

                ModelState.AddModelError("", Message);

                return View(changePassword);
            }
        }

        public IActionResult ConfirmEmail()
        {
            try
            {
                string emailUser = TempData["Email"].ToString();

                if (emailUser == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                var user = _userManager.FindByEmailAsync(emailUser).Result;

                if (user == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

                string redirectUrl = Url.Action("VerifyEmail", "Account", new { UserId = user.Id, token = token }, Request.Scheme);

                string bodyEmail = $"لطفا برای فعالسازی حساب خود در سایت وبلاگر بر روی لینک زیر کلیک کنید. <br/> <a href='{redirectUrl}'><h3> تایید حساب کاربری </h3></a>";
                _serviceAccount.SendEmail(user.Email, bodyEmail, "تایید حساب");

                return View("ConfirmEmail", user.Email);
            }
            catch(Exception error)
            {
                _logger.LogError(error.ToString());

                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult VerifyEmail(string UserId, string token)
        {
            if (UserId == null || token == null)
            {
                return View("FailedConfirmEmail");
            }

            var user = _userManager.FindByIdAsync(UserId).Result;

            if (user == null)
            {
                return View("FailedConfirmEmail");

            }

            var result = _userManager.ConfirmEmailAsync(user, token).Result;

            if (result.Succeeded)
            {
                _signInManager.SignInAsync(user, false).Wait();

                return View("SuccessConfirmEmail");
            }
            else
            {
                return View("FailedConfirmEmail");
            }
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordDto forgetPassword)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(forgetPassword);
                }

                var user = _userManager.FindByEmailAsync(forgetPassword.Email).Result;

                if (user == null)
                {
                    ModelState.AddModelError("", "کاربری با این ایمیل یافت نشد");

                    return View(forgetPassword);
                }

                var resultEmailConfirm = _userManager.IsEmailConfirmedAsync(user).Result;

                if (resultEmailConfirm == false)
                {
                    TempData["Email"] = user.Email;

                    return RedirectToAction("ConfirmEmail");
                }

                var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                var redirectUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, token = token }, Request.Scheme);

                string bodyEmail = $"برای بازیابی رمز عبور خود در سایت وبلاگر بر روی لینک زیر کلیک کنید <br/> <a href={redirectUrl}> <h3> بازیابی رمز عبور </h3> </a>";
                _serviceAccount.SendEmail(user.Email, bodyEmail, "بازیابی رمز عبور");

                return View("SendEmailResetPassword", user.Email);
            }
            catch (Exception error)
            {
                _logger.LogError(error.ToString());

                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult ResetPassword(string UserId, string token)
        {

            ResetPasswordDto resetPassword = new ResetPasswordDto
            {
                UserId = UserId,
                Token = token
            };
            return View(resetPassword);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDto resetPassword)
        {
            if (ModelState.IsValid == false)
            {
                return View(resetPassword);
            }

            var user = _userManager.FindByIdAsync(resetPassword.UserId).Result;

            if (user == null)
            {
                return View("FailedResetPassword");
            }

            var result = _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password).Result;

            if (result.Succeeded)
            {
                return View("SuccessResetPassword");
            }
            else
            {
                string Message = "";
                foreach (var error in result.Errors)
                    Message += error.Description;
                ModelState.AddModelError("", Message);

                return View(resetPassword);
            }
        }

        [HttpGet]
        public IActionResult TwoFactorLogin()
        {

            try
            {
                string userId = TempData["UserId"].ToString();

                bool? IsPersistans = TempData["IsPersistans"] as bool?;

                if (userId == null || IsPersistans == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                var user = _userManager.FindByIdAsync(userId).Result;

                if (user == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                TwoFactorDto twoFactorLogin = new TwoFactorDto();

                twoFactorLogin.IsPersistans = (bool)IsPersistans;

                var providers = _userManager.GetValidTwoFactorProvidersAsync(user).Result;
                if (providers.Contains("Email"))
                {
                    string codeEmail = _userManager.GenerateTwoFactorTokenAsync(user, "Email").Result;

                    string bodyEmail = $"لطفا برای تکمیل ورود دو مرحله به حساب خود کد زیر را در فرم مربوطه وارد کنید <br/> <h2>{codeEmail}</h2>";
                    _serviceAccount.SendEmail(user.Email, bodyEmail, "ورود دو مرحله ای");

                    twoFactorLogin.Provider = "Email";

                    return View(twoFactorLogin);
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error.ToString());

                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult TwoFactorLogin(TwoFactorDto twoFactor)
        {
            if (ModelState.IsValid == false)
            {
                return View(twoFactor);
            }

            var user = _signInManager.GetTwoFactorAuthenticationUserAsync().Result;

            if (user == null)
            {
                return BadRequest();
            }

            var result = _signInManager.TwoFactorSignInAsync(twoFactor.Provider, twoFactor.Code, twoFactor.IsPersistans, false).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "حساب کاربری شما قفل است");

                return View(twoFactor);
            }
            else
            {
                ModelState.AddModelError("", "کد وارد شده صحیح نیست");

                return View(twoFactor);
            }
        }

        public bool HasLogin()
        {
            return User.Identity.IsAuthenticated;
        }
    }
}
