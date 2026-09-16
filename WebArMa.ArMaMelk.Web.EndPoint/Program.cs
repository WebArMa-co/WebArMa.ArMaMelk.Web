using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WebArMa.ArMaMelk.Web.Application._Shared.Delegates;
using WebArMa.ArMaMelk.Web.Application._Shared.Helpers;
using WebArMa.ArMaMelk.Web.Application.Auth.DTOs;
using WebArMa.ArMaMelk.Web.Application.Auth.Services;
using WebArMa.ArMaMelk.Web.Application.OTP.Services;
using WebArMa.ArMaMelk.Web.Application.Toast.Services;
using WebArMa.ArMaMelk.Web.Application.Users.Services;
using WebArMa.ArMaMelk.Web.EndPoint.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ToastService>();
builder.Services.AddTransient<ApiAuthenticationHandler>();
builder.Services.AddHttpClient("Api", client =>
{
	client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
}).AddHttpMessageHandler<ApiAuthenticationHandler>();

builder.Services.AddHttpClient<IOTPService, OTPService>(client =>
{
	client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
});

builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
	client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
})
	.AddHttpMessageHandler<ApiAuthenticationHandler>();

builder.Services.AddHttpClient<IUserService, UserService>(client =>
{
	client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
})
	.AddHttpMessageHandler<ApiAuthenticationHandler>();

builder.Services.AddAuthentication("ArMaMelk").AddCookie("ArMaMelk", options =>
    {
        options.Cookie.Name = "auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;

        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";

        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/Auth/Logout", async (HttpContext httpContext, CancellationToken cancellationToken, IAuthService authService) =>
{
	await authService.LogoutAsync(terminateAllSessions: false, cancellationToken);
	await httpContext.SignOutAsync("ArMaMelk");
	return Results.Redirect("/login");
});

app.MapPost("/Auth/Login", async ([FromForm] LoginDTO request, HttpContext httpContext, CancellationToken cancellationToken, IAuthService authService) =>
{
    var result = await authService.LoginAsync(request.PhoneNumber, request.Code, cancellationToken);
    if (result is null)
    {
        return Results.Redirect("/Login?error=invalid");
    }

    var principal = ClaimsPrincipalFactory.Create(result.Token);
    var properties = new AuthenticationProperties();
    properties.StoreTokens(
    [
        new AuthenticationToken
        {
            Name = "access_token",
            Value = result.Token
        },
        new AuthenticationToken
        {
            Name = "refresh_token",
            Value = result.RefreshToken
        }
    ]);

    await httpContext.SignInAsync("ArMaMelk", principal, properties);
    return Results.Redirect("/");
});

app.MapPost("/Auth/RefreshLogin", async (HttpContext httpContext, CancellationToken cancellationToken, IAuthService authService) =>
{
    var result = await authService.LoginAsync(cancellationToken);
    if (result is null)
    {
        return Results.Redirect("/Login?error=invalid");
    }

    var principal = ClaimsPrincipalFactory.Create(result.Token);
    var properties = new AuthenticationProperties();
    properties.StoreTokens(
    [
        new AuthenticationToken
        {
            Name = "access_token",
            Value = result.Token
        },
        new AuthenticationToken
        {
            Name = "refresh_token",
            Value = result.RefreshToken
        }
    ]);

    await httpContext.SignInAsync("ArMaMelk", principal, properties);
    return Results.Ok();
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();
