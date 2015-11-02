using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Common;
using Ext.Net;
using Ext.Net.MVC;
using Lucene.Net.Documents;
using Microsoft.Office.Interop.Excel;
using VentureManagement.Models;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Areas.Member.Controllers
{
    [DirectController(AreaName = "Member")]
    public class ChangePasswordController : MemberBaseController
    {
        //
        // GET: /Member/ChangePassword/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VerificationCode()
        {
            string verificationCode = Security.CreateVerificationText(4);
            var _img = Security.CreateVerificationImage(verificationCode, 160, 30);
            _img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            TempData["VerificationCode"] = verificationCode.ToUpper();
            return null;
        }

        [DirectMethod(Namespace = "ChangePassword")]
        public ActionResult Submit(string verificationCode, string oldPassword, string newPassword)
        {
            try
            {
                if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != verificationCode.ToUpper())
                {
                    X.Msg.Alert("", "验证码错误，请重试").Show();
                    return this.FormPanel();
                }

                if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
                {
                    X.Msg.Alert("", "输入密码不能为空，请重试").Show();
                    return this.FormPanel();
                }

                if (oldPassword == newPassword)
                {
                    X.Msg.Alert("", "新密码和原密码相同，请重试").Show();
                    return this.FormPanel();
                }

                var user = _userService.Find(_currentUser.UserName);
                if (Utility.DesDecrypt(user.Password) != oldPassword)
                {
                    X.Msg.Alert("", "原密码错误，请重试").Show();
                    return this.FormPanel();
                }

                user.Password = Utility.DesEncrypt(newPassword);
                _userService.Update(user);
                System.Web.HttpContext.Current.Cache.Remove(User.Identity.Name);
                // Delete the authentication ticket and sign out.
                FormsAuthentication.SignOut();
                X.Msg.Confirm("提示", "密码修改成功，请重新登录", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "window.parent.document.location.reload();",
                        Text = "确定"
                    }
                }).Show();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                X.Msg.Alert("", "修改密码失败，请检查原密码和新密码").Show();
                return this.FormPanel();
            }
            
            return this.FormPanel();
        }

        [DirectMethod(Namespace = "ChangePassword")]
        public ActionResult LogOff()
        {
            return RedirectToRoute("Default", new { Controller = "Login", Action = "LogOff" });
        }
    }
}
