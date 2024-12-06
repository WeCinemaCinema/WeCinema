using ITDIV_TICKET.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var url = builder.Configuration["UrlSupabase"] ?? "";
var key = builder.Configuration["KeySupabase"] ?? ""; 

var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};

var supabase = new Supabase.Client(url, key, options);
await supabase.InitializeAsync();   

builder.Services.AddSingleton(supabase);

builder.Services.AddSingleton<MovieService>();

builder.Services.AddTransient<EmailSender>();

builder.Services.AddAuthentication("CookieScheme")
    .AddCookie("CookieScheme",options => {
        options.LoginPath = "/WeCinema/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    })
    .AddGoogle("GoogleScheme",options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"]?? "";
        options.ClientSecret = builder.Configuration["Google:ClientSecret"]?? "";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/WeCinema/Error");
    
    app.UseHsts();
}


app.UseWebSockets();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RedirectIfNoLogin>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=WeCinema}/{action=Index}/{id?}");

app.Run();
