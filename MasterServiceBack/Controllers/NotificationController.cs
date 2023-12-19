using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MasterServiceBack.Notify;

namespace MasterServiceBack.Controllers;


[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationController(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("send/{userId}")]
    public async Task<IActionResult> SendNotification(string userId, [FromBody] string message)
    {
        await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        return Ok();
    }
}