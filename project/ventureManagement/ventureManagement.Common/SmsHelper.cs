using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VentureManagement.Models;
using VentureManagement.BLL;

namespace Common
{
    public class SmsHelper
    {
        private const string Account = "SCyouy999";
        private const string Pwd = "Huofw7325";

        public static void GetSmsStatus()
        {
            try
            {
                const string param = "account=" + Account + "&password=" + Pwd;

                var bs = Encoding.UTF8.GetBytes(param);
                var req = (HttpWebRequest)WebRequest.Create("http://sms.huoni.cn:8080/smshttp/sendStatus");
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;

                using (var reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }

                using (var wr = req.GetResponse())
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.Default);
                    var result = sr.ReadToEnd().Trim().Split(Convert.ToChar("\n"));

                    foreach (var status in result)
                    {
                        if(status.Contains("|"))
                            UpdateSmsStatus(status);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        public static Sms SendSms(int userId,string message)
        {
            try
            {
                var userService = new UserService();
                var smsService = new SmsService();

                var ran = new Random();
                var taskId = Account + "_" + DateTime.Now.ToString("yyyyMMddHHss") + "_http_" +
                             ran.Next(10000, 999999);

                var send2User = userService.Find(userId);

                var param = "account=" + Account + "&password=" + Pwd + "&content=" + message + "【隐患申报系统】"
                            + "&sendtime=&phonelist=" + send2User.Mobile + "&taskId=" + taskId;

                var sms = new Sms()
                {
                    Message = message,
                    Send2UserId = send2User.UserId,
                    Address = send2User.Mobile,
                    SendDateTime = DateTime.Now,
                    TaskId = taskId
                };

                var bs = Encoding.UTF8.GetBytes(param);
                var req = (HttpWebRequest) WebRequest.Create("http://sms.huoni.cn:8080/smshttp/infoSend");
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;

                using (var reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }

                using (var wr = req.GetResponse())
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.Default);
                    var result = sr.ReadToEnd().Trim().Split(Convert.ToChar(","));

                    if (result.Any())
                    {
                        sms.Status = Convert.ToInt32(result[0]);
                        if (sms.Status == 0)
                        {
                            sms.DeliverStats = "ToAgency";

                            if (result.Length > 2)
                            {
                                Debug.Print("发送结果为：" + result[0]);
                                Debug.Print("发送成功条数：" + result[1]);
                                Debug.Print("剩余短信条数：" + result[2]);
                            }
                        }
                        else
                        {
                            if (result.Count() > 2)
                                sms.BlockWord = result[1];
                        }
                        return smsService.Add(sms);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return null;
            }
            finally
            {
                Thread.Sleep(500);
                GetSmsStatus();
            }

            return null;
        }

        private static void UpdateSmsStatus(string status)
        {
            var statusArray = status.Split(Convert.ToChar("|"));

            if (statusArray.Count() < 4) return;

            var taskId = statusArray[0].Trim();
            var deliverStats = statusArray[1].Trim();
            //var dateTime = Convert.ToDateTime(statusArray[2].Trim());
            var address = statusArray[3].Trim();

            var smsService = new SmsService();

            var sms = smsService.Find(s => s.TaskId == taskId);

            sms.DeliverStats = deliverStats;

            smsService.Update(sms);
        }
    }
}
