using System.Security.Claims;

namespace StudentDojo.Core.Extensions;
public static class IdentityExtensions
{
    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return GetClaimValue(claimsPrincipal, ClaimTypes.Email, "email");
    }

    public static string? GetName(this ClaimsPrincipal claimsPrincipal)
    {
        return GetClaimValue(claimsPrincipal, ClaimTypes.Name, "name");
    }

    public static string? GetGivenName(this ClaimsPrincipal claimsPrincipal)
    {
        return GetClaimValue(claimsPrincipal, ClaimTypes.GivenName);
    }

    public static string? GetSurname(this ClaimsPrincipal claimsPrincipal)
    {
        return GetClaimValue(claimsPrincipal, ClaimTypes.Surname);
    }

    public static string? GetAuthId(this ClaimsPrincipal? claimsPrincipal)
    {
        return GetClaimValue(claimsPrincipal, ClaimTypes.NameIdentifier, "sub");
    }

    private static string? GetClaimValue(ClaimsPrincipal? claimsPrincipal, params string[] claimNames)
    {
        ArgumentNullException.ThrowIfNull(claimsPrincipal);

        for (int i = 0; i < claimNames.Length; i++)
        {
            string? currentValue = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimNames[i])?.Value;
            if (!string.IsNullOrEmpty(currentValue))
            {
                return currentValue;
            }
        }

        return null;
    }
}
