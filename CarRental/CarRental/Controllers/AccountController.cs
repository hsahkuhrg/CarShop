using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkWithData.Models;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CarRental.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Surname = model.Surname, Year = model.Year, ApplicationUserRealName = model.ApplicationUserRealName };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "user");
                    // For Email Confirmed
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    /*await UserManager.SendEmailAsync(user.Id, "Подтверждение электронной почты",
                               "Для завершения регистрации перейдите по ссылке:: <a href=\""
                                                               + callbackUrl + "\">завершить регистрацию</a>");*/
                    var from = "vitalik.vlasenko.000@gmail.com";
                    var pass = "varvar2000";
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(from, pass);
                    client.EnableSsl = true;
                    client.Send(new System.Net.Mail.MailMessage("vitalik.vlasenko.000@gmail.com", model.Email.ToString(), "Подтверждение электронной почты", "Для завершения регистрации перейдите по ссылке: " + callbackUrl));
                    return View("DisplayEmail");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return HttpNotFound();
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else 
                {
                    if (user.EmailConfirmed == true)
                    {
                        ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            //return RedirectToAction("Index", "Home");
                            //bool IsAdmin = HttpContext.User.IsInRole("admin");
                            //bool IsManager = HttpContext.User.IsInRole("manager");
                            if (HttpContext.User.IsInRole("admin"))
                            {
                                return View("~/Views/AdminOffice/ShowOffice.cshtml"); //RedirectToAction("ShowOffice", "AdminOffice"); 
                            }
                            else if (HttpContext.User.IsInRole("manager"))
                            {
                                return View("~/Views/ManagerOffice/ShowOffice.cshtml"); //RedirectToAction("ShowOffice", "ManagerOffice"); 
                            }
                            else
                            {
                                return View("~/Views/UserOffice/ShowOffice.cshtml"); //RedirectToAction("ShowOffice", "UserOffice"); 
                            }
                        }
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Не подтвержден email.");
                    }
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        [Authorize(Roles = "admin")]
        public ActionResult RegisterManager()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> RegisterManager(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Surname = model.Surname, Year = model.Year, ApplicationUserRealName = model.ApplicationUserRealName };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "manager");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return View("ForgotPasswordConfirmation");
                }
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                var from = "vitalik.vlasenko.000@gmail.com";
                var pass = "varvar2000";
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(from, pass);
                client.EnableSsl = true;
                client.Send(new System.Net.Mail.MailMessage("vitalik.vlasenko.000@gmail.com", model.Email.ToString(), "Сброс пароля", "Для сброса пароля, перейдите по ссылке" + callbackUrl));
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            return View(model);
        }

        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Name);
            if (user == null)
            {
                // for unshow that user is not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            return View();
        }

        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}