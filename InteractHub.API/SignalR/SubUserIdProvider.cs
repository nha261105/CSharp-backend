using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace InteractHub.API.SignalR;

public class SubUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? connection.User?.FindFirstValue("sub");
    }
}