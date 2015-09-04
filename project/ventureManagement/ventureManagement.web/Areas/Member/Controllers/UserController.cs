using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ventureManagement.web.Areas.Member.Models;
using ventureManagement.Common;
using System.Drawing;
using ventureManagement.models;
using ventureManagement.IBLL;
using ventureManagement.BLL;
using ventureManagement.web.Attributes;
//using System.Web.Mvc.Filters;


namespace ventureManagement.Web.Areas.Member.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly InterfaceUserService _userService;
        public UserController() { _userService = new UserService(); }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel passwordViewModel)
        {
            if (!ModelState.IsValid) return View(passwordViewModel);

            var user = _userService.Find(User.Identity.Name);
            if (user.Password == Common.Security.Sha256(passwordViewModel.OriginalPassword))
            {
                user.Password = Common.Security.Sha256(passwordViewModel.Password);
                ModelState.AddModelError("", _userService.Update(user) ? "修改密码成功" : "修改密码失败");
            }
            else ModelState.AddModelError("", "原密码错误");
            return View(passwordViewModel);
        }

        /// <summary>
        /// 显示资料
        /// </summary>
        /// <returns></returns>
        public ActionResult Details()
        {
            return View(_userService.Find(User.Identity.Name));
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="returnUrl">返回Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.Find(loginViewModel.UserName);
                if (user == null) ModelState.AddModelError("UserName", "用户名不存在");
                else if (user.Password == Common.Security.Sha256(loginViewModel.Password))
                {
                    user.LoginTime = System.DateTime.Now;
                    user.LoginIP = Request.UserHostAddress;
                    _userService.Update(user);

                    if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");
                    else if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    else return RedirectToAction("Index", "Home");
                }
                else ModelState.AddModelError("Password", "密码错误");
            }
            return View();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            return Redirect(Url.Content("~/"));
        }

        /// <summary>
        /// 用户导航栏
        /// </summary>
        /// <returns>分部视图</returns>
        public ActionResult Menu()
        {
            return PartialView();
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Modify()
        {

            var user = _userService.Find(User.Identity.Name);
            if (user == null) ModelState.AddModelError("", "用户不存在");
            else
            {
                if (TryUpdateModel(user, new string[] { "DisplayName", "Email" }))
                {
                    if (ModelState.IsValid)
                    {
                        ModelState.AddModelError("", _userService.Update(user) ? "修改成功！" : "无需要修改的资料");
                    }
                }
                else ModelState.AddModelError("", "更新模型数据失败");
            }
            return View("Details", user);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel register)
        {
            if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != register.VerificationCode.ToUpper())
            {
                ModelState.AddModelError("VerificationCode", "验证码不正确");
                return View(register);
            }
            if (ModelState.IsValid)
            {

                if (_userService.Exist(register.UserName)) ModelState.AddModelError("UserName", "用户名已存在");
                else
                {
                    User user = new User()
                    {
                        UserName = register.UserName,
                        //默认用户组代码写这里
                        DisplayName = register.DisplayName,
                        Password = Security.Sha256(register.Password),
                        //邮箱验证与邮箱唯一性问题
                        Email = register.Email,
                        //用户状态问题
                        Status = 0,
                        RegistrationTime = System.DateTime.Now
                    };
                    user = _userService.Add(user);
                    if (user.UserID > 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else { ModelState.AddModelError("", "注册失败！"); }
                }
            }
            return View(register);
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult VerificationCode()
        {
            string verificationCode = Security.CreateVerificationText(6);
            Bitmap img = Security.CreateVerificationImage(verificationCode, 160, 30);
            img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            TempData["VerificationCode"] = verificationCode.ToUpper();
            return null;
        }
    }
}