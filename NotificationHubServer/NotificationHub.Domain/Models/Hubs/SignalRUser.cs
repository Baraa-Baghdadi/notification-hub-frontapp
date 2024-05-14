using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Domain.Models.Hubs
{
    public class SignalRUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public bool IsLeftThisGroup { get; set; }

        public SignalRUser(string userId,string userName, string groupName, bool isLeftThisGroup = false)
        {
            UserId = userId;
            UserName = userName;
            GroupName = groupName;
            IsLeftThisGroup = isLeftThisGroup;

        }
    }
}
