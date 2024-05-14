using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NotificationHub.Domain.Data;
using NotificationHub.Domain.Hubs;
using NotificationHub.Domain.Models;
using NotificationHub.Domain.Models.Hubs;

namespace NotificationHub.Domain.ProductService
{
    public class InMemoryProductService : IProductService
    {

        private readonly IHubContext<ProductNotificationHub> _productNotification;

        private readonly AppDbContext _context;

        public InMemoryProductService(IHubContext<ProductNotificationHub> hubContext, AppDbContext context)
        {
            _productNotification = hubContext;
            _context = context;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.products.ToListAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            var foundProduct = _context.products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (foundProduct != null)
            {
                foundProduct.ProductName = product.ProductName;
                foundProduct.Description = product.Description;
                foundProduct.Price = product.Price;
                foundProduct.Stock = product.Stock;
            }

            await _context.SaveChangesAsync();

            await _productNotification.Clients.Group(product.ProductId).SendAsync("privateMessageMethodName", $"{product.ProductId} Updated !!!");

            var SubscribedUser = await _context.signalRUsers.Where(u => u.IsLeftThisGroup == false && u.GroupName == product.ProductId).ToListAsync();

            foreach (var item in SubscribedUser)
            {
                var notfication = new Notification(item.UserName, $"{product.ProductId} Updated !!!");

                var newNotfication = await _context.notifications.AddAsync(notfication);

                await _context.SaveChangesAsync();
            }
        }
    }
}
