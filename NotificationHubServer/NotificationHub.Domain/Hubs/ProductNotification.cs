using Microsoft.EntityFrameworkCore;
using NotificationHub.Domain.Data;
using NotificationHub.Domain.Models.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Domain.Hubs
{
    public class ProductNotification : IProductNotification
    {
        private readonly AppDbContext _context;
        public ProductNotification(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SignalRUser>> getMyGroups(string userName)
        {
            return await 
                _context.signalRUsers.Where(u => u.UserName == userName && u.IsLeftThisGroup == false)
                .ToListAsync();
        }

        public async Task<SignalRUser> addToGroupTable(SignalRUser newUser)
        {
            var oldUser = await _context.signalRUsers.Where(u => u.UserName == newUser.UserName).ToListAsync();

            if (oldUser != null)
            {
                foreach (var item in oldUser)
                {
                    item.UserId = newUser.UserId;
                }
                await _context.SaveChangesAsync();
            }

            var userExist = await _context.signalRUsers.FirstOrDefaultAsync(e => 
            e.UserName == newUser.UserName && e.GroupName == newUser.GroupName);
            if (userExist != null)
            {
                userExist.UserId = newUser.UserId;
                userExist.IsLeftThisGroup = false;
                await _context.SaveChangesAsync();
                return userExist;
            }
            else
            {
                await _context.signalRUsers.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return newUser;
            }
        }

        public async Task<SignalRUser> removeFromGroupTable(SignalRUser newUser)
        {
            var oldUser = await _context.signalRUsers.Where(u => u.UserName == newUser.UserName).ToListAsync();

            if (oldUser != null)
            {
                foreach (var item in oldUser)
                {
                    item.UserId = newUser.UserId;
                }
                await _context.SaveChangesAsync();
            }
            var userExist = await _context.signalRUsers.FirstOrDefaultAsync(u => 
            u.UserName == newUser.UserName && u.GroupName == newUser.GroupName);
            userExist.IsLeftThisGroup = true;
            await _context.SaveChangesAsync();
            return userExist;
        }


        public async Task<List<Notification>> getAllNotificationsList(string userName)
        {
            var myMessages = await _context.notifications
                .Where(p => p.UserName == userName)
                .ToListAsync();
            return myMessages;
        }

        public async Task<List<Notification>> getUnreadedNotifications(string userName)
        {
            var myMessages = await _context.notifications
            .Where(p => p.UserName == userName && p.IsReaded == false)
            .ToListAsync();
            return myMessages;
        }

        public async Task<List<Notification>> getreadedNotifications(string userName)
        {
            var myMessages = await _context.notifications
            .Where(p => p.UserName == userName && p.IsReaded == true)
            .ToListAsync();
            return myMessages;
        }

        public async Task makeNotificationsAsReaded(string userName)
        {
            var meMessage = await _context.notifications.Where(p => p.UserName == userName && p.IsReaded == false)
                .ToListAsync();
            foreach (var item in meMessage)
            {
                item.IsReaded = true;
            }
            await _context.SaveChangesAsync();
        }

    }
}
