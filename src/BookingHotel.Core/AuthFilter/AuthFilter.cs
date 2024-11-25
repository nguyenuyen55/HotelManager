using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

public class RoleAuthorizeAttribute : TypeFilterAttribute
{
    public RoleAuthorizeAttribute(string role) : base(typeof(RoleAuthorizeFilter))
    {
        Arguments = new object[] { role };
    }
}

public class RoleAuthorizeFilter : IAuthorizationFilter
{
    private readonly string _role;

    public RoleAuthorizeFilter(string role)
    {
        _role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        // Kiểm tra xem người dùng đã đăng nhập
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Kiểm tra xem người dùng có vai trò phù hợp không
        if (!user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _role))
        {
            context.Result = new ForbidResult(); // Trả về 403 Forbidden nếu không có quyền
        }
    }
}
