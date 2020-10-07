using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ShindaLibrary
{
    public class NotificationTools
    {
        //寄件資訊
        public string Mail_Sender_Address { get; set; }
        public string Mail_Sender_DisplayName { get; set; }
        //帳密資訊
        public string Mail_Auth_Account { get; set; }
        public string Mail_Auth_Password { get; set; }
        //主機資訊
        public string Mail_Server_SMTP { get; set; }
        public int Mail_Server_Port { get; set; }
        public bool Mail_Server_EnableSSL { get; set; }

        public NotificationTools(
            string Mail_Sender_Address = "", string Mail_Sender_DisplayName = "",
            string Mail_Auth_Account = "", string Mail_Auth_Password = "",
            string Mail_Server_SMTP = "smtp.gmail.com", int Mail_Server_Port = 587, bool Mail_Server_EnableSSL = true)
        {
            //寄件資訊
            this.Mail_Sender_Address = Mail_Sender_Address;
            this.Mail_Sender_DisplayName = Mail_Sender_DisplayName;
            //帳密資訊
            this.Mail_Auth_Account = Mail_Auth_Account;
            this.Mail_Auth_Password = Mail_Auth_Password;
            //主機資訊
            this.Mail_Server_SMTP = Mail_Server_SMTP;
            this.Mail_Server_Port = Mail_Server_Port;
            this.Mail_Server_EnableSSL = Mail_Server_EnableSSL;
        }


        public bool SendNotification(Notification Notification, NotificationType type = NotificationType.Email)
        {
            //選擇寄送方式
            switch (type)
            {
                //寄信
                case NotificationType.Email:
                    SendNotificationByEmail(Notification);
                    break;

                //LINE 推播
                case NotificationType.Line:
                    SendNotificationByLine(Notification);
                    break;

                //FB 通知
                case NotificationType.FB:
                    SendNotificationByFB(Notification);
                    break;
                default:

                    break;
            }

            return true;
        }

        #region 傳訊功能實做
        private NotificationResult SendNotificationByEmail(Notification Notification)
        {
            var result = this.SendMail(Notification.Subject, Notification.Body, Notification.Recipient);

            return result;
        }

        private NotificationResult SendNotificationByLine(Notification Notification)
        {
            return new NotificationResult();
        }

        private NotificationResult SendNotificationByFB(Notification Notification)
        {
            return new NotificationResult();
        }

        /// <summary>
        /// 寄送信件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        private NotificationResult SendMail(
            string subject, string body, string mailTo, params Attachment[] attachments)
        {
            var result = new NotificationResult();

            try
            {
                //寄件地址
                var _mailAddress = new MailAddress(this.Mail_Sender_Address, this.Mail_Sender_DisplayName);
                //訊息內容
                var mailMessage = new MailMessage
                {
                    Sender = _mailAddress,
                    From = _mailAddress,
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body,
                    BodyEncoding = Encoding.UTF8
                };
                // SMTP 寄件元件
                var sc = new SmtpClient
                {
                    Host = this.Mail_Server_SMTP,
                    Port = this.Mail_Server_Port,
                    EnableSsl = this.Mail_Server_EnableSSL
                };

                string account = this.Mail_Auth_Account;
                string password = this.Mail_Auth_Password;

                if (!String.IsNullOrEmpty(account) && !String.IsNullOrEmpty(password))
                {
                    sc.Credentials = new NetworkCredential(account, password);
                }

                // 加入附件
                if (attachments != null)
                {
                    foreach (var item in attachments)
                    {
                        mailMessage.Attachments.Add(item);
                    }
                }

                // 加入寄件者
                mailMessage.To.Add(mailTo);

                // 寄信
                sc.Send(mailMessage);

                // 回收資源
                sc.Dispose();
                mailMessage.Dispose();
                sc = null;
                mailMessage = null;

                result.RtnCode = NotificationRtnCode.Success;
                result.RtnMsg = string.Format("已寄送信件至 {0}", mailTo);
            }
            catch (Exception ex)
            {
                result.RtnCode = NotificationRtnCode.SystemBusy;
                result.RtnMsg = $" EMAIL :: {mailTo} ;; Exception Message :: {ex.Message};; Stack Trace :: {ex.StackTrace} ;;";
            }

            return result;
        }

        public class NotificationResult
        {
            public NotificationRtnCode RtnCode { get; set; }

            public string RtnMsg { get; set; }

            public NotificationResult()
            {
                this.RtnCode = NotificationRtnCode.Fail;
                this.RtnMsg = "寄送失敗";
            }
        }

        #endregion

        public enum NotificationType
        {
            Email = 0,
            Line = 1,
            FB = 2,
        }

        public enum NotificationRtnCode
        {
            SystemBusy = -1,
            Success = 0,
            Fail = 1,
        }

        public class Notification
        {
            //public int Id { get; set; }
            public string Type { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public string Recipient { get; set; }
            //public string SendFrom { get; set; }
            //public DateTime? SentTime { get; set; }
            //public short RetryCount { get; set; }
            //public bool IsStopRetry { get; set; }

            public Notification(
                //int Id = 0,
                string Type = "", string Subject = "", string Body = "",
                string Recipient = ""//, string SendFrom = "",
               //DateTime? SentTime = null, short RetryCount = 0, bool IsStopRetry = false
               )
            {
                //this.Id = Id;
                this.Type = Type;
                this.Subject = Subject;
                this.Body = Body;
                this.Recipient = Recipient;
                //this.SendFrom = SendFrom;
                //this.SentTime = SentTime;
                //this.RetryCount = RetryCount;
               // this.IsStopRetry = IsStopRetry;
            }
        }
    }
}
