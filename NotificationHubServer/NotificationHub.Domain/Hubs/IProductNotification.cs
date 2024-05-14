using NotificationHub.Domain.Models.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Domain.Hubs
{
    public interface IProductNotification
    {
        Task<List<SignalRUser>> getMyGroups(string userName);
        Task<SignalRUser> addToGroupTable(SignalRUser user);
        Task<SignalRUser> removeFromGroupTable(SignalRUser user);
        Task<List<Notification>> getAllNotificationsList(string userName);
        Task<List<Notification>> getUnreadedNotifications(string userName);
        Task<List<Notification>> getreadedNotifications(string userName);
        Task makeNotificationsAsReaded(string userName);
    }
}
