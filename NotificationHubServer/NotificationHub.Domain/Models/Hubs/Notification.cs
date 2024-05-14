using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Domain.Models.Hubs
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public bool IsReaded { get; set; }
        public DateTime AddingDate { get; set; }

        public Notification(string userName,string message, bool isReaded = false)
        {
            UserName = userName;
            Message = message;
            IsReaded = isReaded;
            AddingDate = DateTime.Now;
        }
    }
}
