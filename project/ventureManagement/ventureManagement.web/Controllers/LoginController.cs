using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web.Mvc;
using System.Web.Security;
using Ext.Net;
using Ext.Net.MVC;
using VentureManagement.BLL;
using VentureManagement.IBLL;
using VentureManagement.Models;
using VentureManagement.Web.Attributes;

namespace VentureManagement.Web.Controllers
{
    [DirectController(AreaName = "Member", GenerateProxyForOtherControllers = false, IDMode = DirectMethodProxyIDMode.None)]
    public class LoginController : Controller
    {
        readonly InterfaceUserService _userService = new UserService();
        readonly InterfaceOrganizationService _orgService = new OrganizationService();
        readonly InterfaceOrganizationRelationService _orgrService = new OrganizationRelationService();
        readonly InterfaceUserOrganizationRelationService _uorgService = new UserOrganizationRelationService();

        [AllowAnonymous]
        public ActionResult Index()
        {
            if(!Request.IsAuthenticated)
                return View();

            return RedirectToAction("Index","Main");
        }

        [AllowAnonymous]
        public ActionResult Login(string txtUsername, string txtPassword, string returnUrl)
        {
#if DEBUG
            if (string.IsNullOrEmpty(txtUsername))
                txtUsername = txtPassword = "master";
#endif
            // Validate the user login.
            if (Membership.ValidateUser(txtUsername, txtPassword))
            {
                // Create the authentication ticket.
                FormsAuthentication.SetAuthCookie(txtUsername, false);

                var currentUser = _userService.Find(txtUsername);
                if (currentUser.Status != VentureManagement.Models.User.STATUS_VALID)
                {
                    X.Msg.Alert("提示", string.Concat("此用户名", currentUser.Status, "，请联系管理员")).Show();
                    return this.Direct();
                }
                Session["User"] = currentUser;

                var orgs = _uorgService.FindList(txtUsername).Select(uorg=>uorg.Organization).ToList();
                Session["Organization"] = orgs;
                var currentOrgList = new List<int>();
                foreach (var org in orgs)
                {
                    currentOrgList.Add(org.OrganizationId);
                    currentOrgList.AddRange(_orgrService.GetChildrenOrgList(org.OrganizationName));
                }
                Session["currentOrgList"] = currentOrgList;

                //login time/ip
                currentUser.LoginTime = DateTime.Now;
                currentUser.LoginIP = GetLocalIP();
                _userService.Update(currentUser);

                // Redirect to the secure area.
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToRoute("Default", new { Controller = "Main", Action = "Index" }); ;
                }
            }
            else
            {
                X.Msg.Alert("提示", "用户名或密码错误,请重新输入").Show();
            }

            return this.Direct();
        }

        [AllowAnonymous]
        private static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {          
            return RedirectToAction("Index", "Register");
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            // Delete the user details from cache.
            System.Web.HttpContext.Current.Cache.Remove(User.Identity.Name);

            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Main");
        }
    }
}
