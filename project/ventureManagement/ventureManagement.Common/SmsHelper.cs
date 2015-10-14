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
using System.Xml.Serialization;
using VentureManagement.Models;
using VentureManagement.BLL;
// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToAutoProperty

namespace Common
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class returnsmsStatusbox
    {

        private ulong mobileField;

        private string taskidField;

        private byte statusField;

        private string receivetimeField;

        private string errorcodeField;

        /// <remarks/>
        public ulong mobile
        {
            get
            {
                return this.mobileField;
            }
            set
            {
                this.mobileField = value;
            }
        }

        /// <remarks/>
        public string taskid
        {
            get
            {
                return this.taskidField;
            }
            set
            {
                this.taskidField = value;
            }
        }

        /// <remarks/>
        public byte status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string receivetime
        {
            get
            {
                return this.receivetimeField;
            }
            set
            {
                this.receivetimeField = value;
            }
        }

        /// <remarks/>
        public string errorcode
        {
            get
            {
                return this.errorcodeField;
            }
            set
            {
                this.errorcodeField = value;
            }
        }
    }
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class returnsms
    {
        public static returnsms Deserialize(string templateFile)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(returnsms));
                var stream = new MemoryStream(Encoding.ASCII.GetBytes(templateFile));
                return serializer.Deserialize(stream) as returnsms;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            return null;
        }

        private returnsmsStatusbox[] statusboxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("statusbox")]
        public returnsmsStatusbox[] statusbox
        {
            get
            {
                return this.statusboxField;
            }
            set
            {
                this.statusboxField = value;
            }
        }

        private string returnstatusField;

        private string messageField;

        private string remainpointField;

        private string taskIDField;

        private string successCountsField;

        /// <remarks/>
        public string returnstatus
        {
            get
            {
                return this.returnstatusField;
            }
            set
            {
                this.returnstatusField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public string remainpoint
        {
            get
            {
                return this.remainpointField;
            }
            set
            {
                this.remainpointField = value;
            }
        }

        /// <remarks/>
        public string taskID
        {
            get
            {
                return this.taskIDField;
            }
            set
            {
                this.taskIDField = value;
            }
        }

        /// <remarks/>
        public string successCounts
        {
            get
            {
                return this.successCountsField;
            }
            set
            {
                this.successCountsField = value;
            }
        }
    }

    public class SmsHelper
    {
        private const string Account = "SCYYXX";
        private const string Pwd = "youyongkeji81452705";

        public static void GetSmsStatus()
        {
            try
            {
                const string param = "action=query&userid=1067&account=" + Account + "&password=" + Pwd;

                var bs = Encoding.UTF8.GetBytes(param);
                var req = (HttpWebRequest)WebRequest.Create("http://www.smsok.cn/statusApi.aspx");
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;

                using (var reqStream = req.GetRequestStream())
                {
#if DEBUG
                    reqStream.Write(bs, 0, 0);
#else
                    reqStream.Write(bs, 0, bs.Length);
#endif
                }

                using (var wr = req.GetResponse())
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.Default);
                    var result = returnsms.Deserialize(sr.ReadToEnd().Trim());

                    if (result != null)
                    {
                        foreach (var statusBox in result.statusbox)
                        {
                            UpdateSmsStatus(statusBox);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        public static Sms SendSms(string mobile, string message)
        {
            try
            {
                var smsService = new SmsService();

                message = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(message)) + "【隐患申报系统】";
                var param = "action=send&userid=1067&account=" + Account + "&password=" + Pwd + "&content=" + message
                            + "&mobile=" + mobile + "&sendTime=&extno=";

                var sms = new Sms
                {
                    Message = message,
                    Send2UserId = 1,
                    Address = mobile,
                    SendDateTime = DateTime.Now,
                };

                var bs = Encoding.UTF8.GetBytes(param);
                var req = (HttpWebRequest)WebRequest.Create("http://www.smsok.cn/sms.aspx");
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;

                using (var reqStream = req.GetRequestStream())
                {
#if DEBUG
                    reqStream.Write(bs, 0, 0);
#else
                    reqStream.Write(bs, 0, bs.Length);
#endif
                }

                using (var wr = req.GetResponse())
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.Default);
                    var result = returnsms.Deserialize(sr.ReadToEnd().Trim());
                    if (result != null)
                    {
                        if (result.returnstatus.ToLower().Contains("success"))
                        {
                            sms.TaskId = result.taskID.Trim();
                            sms.DeliverStats = "ToAgency";
                            sms.RecvDateTime = DateTime.MaxValue;
#if DEBUG
                            Debug.Print("余额：" + result.remainpoint);
#endif
                            CheckLackMoney(result.remainpoint);
                            return smsService.Add(sms);
                        }
                        else
                            Debug.Print("错误信息：" + result.message);
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

        public static Sms SendSms(int userId,string message)
        {
            try
            {
                var userService = new UserService();
                var smsService = new SmsService();

                //var ran = new Random();
                //var taskId = Account + "_" + DateTime.Now.ToString("yyyyMMddHHss") + "_http_" +
                //             ran.Next(10000, 999999);

                var send2User = userService.Find(userId);
                
                message = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(message))+"【隐患申报系统】"; 
                var param = "action=send&userid=1067&account=" + Account + "&password=" + Pwd + "&content=" + message
                            + "&mobile=" + send2User.Mobile + "&sendTime=&extno=";

                var sms = new Sms
                {
                    Message = message,
                    Send2UserId = send2User.UserId,
                    Address = send2User.Mobile,
                    SendDateTime = DateTime.Now,
                };

                var bs = Encoding.UTF8.GetBytes(param);
                var req = (HttpWebRequest)WebRequest.Create("http://www.smsok.cn/sms.aspx");
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = bs.Length;

                using (var reqStream = req.GetRequestStream())
                {
#if DEBUG
                    reqStream.Write(bs, 0, 0);
#else
                    reqStream.Write(bs, 0, bs.Length);
#endif
                }

                using (var wr = req.GetResponse())
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.Default);
                    var result = returnsms.Deserialize(sr.ReadToEnd().Trim());
                    if (result != null)
                    {
                        if (result.returnstatus.ToLower().Contains("success"))
                        {
                            sms.TaskId = result.taskID.Trim();
                            sms.DeliverStats = "ToAgency";
                            sms.RecvDateTime = DateTime.MaxValue;
#if DEBUG                            
                            Debug.Print("余额：" + result.remainpoint);
#endif
                            CheckLackMoney(result.remainpoint);
                            return smsService.Add(sms);
                        }
                        else
                            Debug.Print("错误信息：" + result.message);
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

        private static void CheckLackMoney(string remainpoint)
        {
            try
            {
                var remain = Convert.ToInt32(remainpoint);

                if (remain < 100)
                {
                    SendSms(1, "隐患申报系统余额不足,请尽快充值,还剩短信:" + remain);
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        private static void UpdateSmsStatus(returnsmsStatusbox status)
        {
            var taskId = status.taskid.Trim();
            var smsService = new SmsService();
            try
            {
                var sms = smsService.Find(s => s.TaskId == taskId);

                sms.DeliverStats = status.errorcode;
                sms.Status = status.status;
                sms.RecvDateTime = Convert.ToDateTime(status.receivetime);

                smsService.Update(sms);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
            
        }
    }
}
