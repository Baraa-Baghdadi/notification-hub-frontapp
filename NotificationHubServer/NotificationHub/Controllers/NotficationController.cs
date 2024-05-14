using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationHub.Domain.Data;
using NotificationHub.Domain.Hubs;
using NotificationHub.Domain.Models.Hubs;

namespace NotificationHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotficationController : ControllerBase
    {
        private readonly IProductNotification _productNotification;

        public NotficationController(IProductNotification productNotification)
        {
            _productNotification = productNotification;
        }

        [HttpGet("getMyGroups")]
        public async Task<ActionResult> getMyGroups(string userName)
        {
            var result = await _productNotification.getMyGroups(userName);
            return Ok(result);
        }

        [HttpGet("getAllNotificationsList")]
        public async Task<ActionResult> getAllNotificationsList(string userName)
        {
            var result = await _productNotification.getAllNotificationsList(userName);
            return Ok(result);
        }

        [HttpGet("getReadedNotifications")]
        public async Task<ActionResult> getReadedNotifications(string userName)
        {
            var result = await _productNotification.getreadedNotifications(userName);
            return Ok(result);
        }

        [HttpGet("getUnreadedNotifications")]
        public async Task<IActionResult> getUnreadedNotifications(string userName)
        {
            var result = await _productNotification.getUnreadedNotifications(userName);
            return Ok(result);
        }

        [HttpGet("makeNotificationsAsReaded")]
        public async Task makeNotificationsAsReaded(string userName)
        {
            await _productNotification.makeNotificationsAsReaded(userName);
        }

    }
}
