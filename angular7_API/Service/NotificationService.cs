using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angular_API.Service
{
    public class NotificationService
    {
        /// <summary>
        /// 寄信
        /// </summary>
        /// <param name="to">對象</param>
        /// <param name="subject">主旨</param>
        /// <param name="content">內文</param>
        public void SendEmail(string to, string subject, string content)
        {
            using (var mySmtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
            {
                mySmtp.Credentials = new System.Net.NetworkCredential("vincent86629@gmail.com", "mipvrgjbsfncuroi");
                mySmtp.EnableSsl = true;
                mySmtp.Send("vincent.hong@shinda.com.tw",
                    to,
                    subject,
                    content);
            }
        }
    }
}
