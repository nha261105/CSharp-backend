using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InteractHub.Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace InteractHub.API.Middlewares;

public class JwtBearerEventHandler
{
    public static Func<MessageReceivedContext, Task> OnMessageReceived =>
        context =>
        {
            // SignalR - Lấy token từ query string khi kết nối tới notification hub
            if (context.HttpContext.Request.Path.StartsWithSegments("/hubs/notifications") &&
                context.Request.Query.TryGetValue("access_token", out var accessToken))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        };

    public static Func<TokenValidatedContext, Task> OnTokenValidated =>
        async context =>
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
            var principal = context.Principal;

            // [1] Extract user ID từ token claims
            var userId = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? principal?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!long.TryParse(userId, out var parsedUserId))
            {
                context.Fail("Invalid token subject");
                return;
            }

            // [2] Kiểm tra user tồn tại và đang active
            var user = await userManager.FindByIdAsync(parsedUserId.ToString());
            if (user == null || !user.IsActive)
            {
                context.Fail("User is not active");
                return;
            }

            // [3] Kiểm tra token bị revoke (so sánh security stamp)
            var tokenSecurityStamp = principal?.FindFirstValue("security_stamp");
            if (string.IsNullOrWhiteSpace(tokenSecurityStamp)
                || !string.Equals(tokenSecurityStamp, user.SecurityStamp, StringComparison.Ordinal))
            {
                context.Fail("Token has been revoked");
            }
        };
}
