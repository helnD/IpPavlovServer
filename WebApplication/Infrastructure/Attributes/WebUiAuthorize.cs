using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Infrastructure.Attributes;

/// <summary>
/// Authorization policy to be applied for web pages of the backend.
/// </summary>
public class WebUiAuthorizeAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public WebUiAuthorizeAttribute()
    {
        AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme;
    }
}