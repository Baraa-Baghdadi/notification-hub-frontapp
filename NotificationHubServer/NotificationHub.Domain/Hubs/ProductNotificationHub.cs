using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NotificationHub.Domain.Data;
using NotificationHub.Domain.Models;
using NotificationHub.Domain.Models.Hubs;
using System.Collections.Generic;

namespace NotificationHub.Domain.Hubs
{
    public class ProductNotificationHub : Hub
    {
        private readonly IProductNotification _productNotification;
        private readonly AppDbContext _context;


        public ProductNotificationHub(IProductNotification productNotification, AppDbContext context)
        {
            _productNotification = productNotification;
            _context = context;
        }
        public void GetDataFromClient(string userId, string connectionId)
        {

            Clients.Client(connectionId).SendAsync("clientMethodName", $"Updated userid {userId}");
        }

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            Clients.Client(connectionId).SendAsync("WelcomeMethodName", connectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            return base.OnDisconnectedAsync(exception);
        }


        // For Subscribe to Group:
        public async Task SuscribeToProduct(string productId,string userName)
        {
            await _productNotification.addToGroupTable(new SignalRUser(Context.ConnectionId, userName,productId));
            await Groups.AddToGroupAsync(Context.ConnectionId, productId);
        }

        // For UnSubscribe From Group:
        public async Task UnSubscribeFromProduct(string productId, string userName)
        {
            await _productNotification.removeFromGroupTable(new SignalRUser(Context.ConnectionId, userName, productId,true));
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, productId);
        }
    }
}
