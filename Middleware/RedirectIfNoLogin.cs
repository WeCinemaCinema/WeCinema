namespace ITDIV_TICKET.Middleware;

public class RedirectIfNoLogin
{   
    private readonly RequestDelegate _next;
    public RedirectIfNoLogin(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {

         var isAuthenticated = context.User?.Identity?.IsAuthenticated ?? false;

        if (!isAuthenticated)
        {

            if (!context.Request.Path.StartsWithSegments("/WeCinema/Login") &&
                !context.Request.Path.StartsWithSegments("/WeCinema/Register") &&
                !context.Request.Path.StartsWithSegments("/WeCinema/Logout") &&
                !context.Request.Path.StartsWithSegments("/") && 
                !context.Request.Path.StartsWithSegments("/WeCinema/DoLoginUser") &&
                !context.Request.Path.StartsWithSegments("/WeCinema/DoRegisterUser") &&
                !context.Request.Path.StartsWithSegments("/Login/Login") &&
                !context.Request.Path.StartsWithSegments("/Login/Register") &&
                !context.Request.Path.StartsWithSegments("/Login/GoogleResponseRegister") &&
                !context.Request.Path.StartsWithSegments("/Login/GoogleResponse"))  
            {

                context.Response.Redirect("/WeCinema/Login");
                return; 
            }
        }
        
        await _next(context);
    }
}