using System.Security.Claims;

namespace Storage.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("UserId claim not found")
                );
        }
    }
}
