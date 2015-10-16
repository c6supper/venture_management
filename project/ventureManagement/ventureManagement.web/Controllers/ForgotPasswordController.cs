using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.IBLL;
using VentureManagement.BLL;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Controllers
{
    public class ForgotPasswordController : BaseController
    {
        //
        // GET: /ForgotPassword/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Submit(string txtUsername)
        {
            InterfaceUserService userService = new UserService();
            try
            {
                var user = userService.FindList(u => u.UserName == txtUsername || u.Mobile == txtUsername, "UserId", false).First();

                if (user != null)
                {
                    InterfaceSmsService smsService = new SmsService();
                    if (smsService.FindList(s => s.Message.Contains("请妥善保管您的密码") &&
                                                 s.Send2UserId == user.UserId).ToArray().Any(sms => DateTime.Now - sms.SendDateTime < TimeSpan.FromMinutes(30)))
                    {
                        X.Msg.Alert("", "半小时内，仅允许查询一次密码.").Show();
                        return this.FormPanel();
                    }

                    SmsHelper.SendSms(user.UserId,
                        "用户名:" + user.UserName + ".密码为:" + Utility.DesDecrypt(user.Password) + ".请妥善保管您的密码.");
                }

                X.Msg.Alert("", "已发送，请查看短信信息.").Show();
            }
            catch (Exception ex)
            {
                X.Msg.Alert("", "参数输入错误.").Show();
                Debug.Print(ex.Message);
                return this.FormPanel();
            }

            return this.FormPanel();
        }
    }
}
