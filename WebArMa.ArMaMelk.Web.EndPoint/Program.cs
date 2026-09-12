using WebArMa.ArMaMelk.Web.Application.Auth.Services;
using WebArMa.ArMaMelk.Web.Application.OTP.Services;
using WebArMa.ArMaMelk.Web.Application.Toast.Services;
using WebArMa.ArMaMelk.Web.EndPoint.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ToastService>();
builder.Services.AddHttpClient<IOTPService, OTPService>(client =>
{
	client.BaseAddress = new Uri(
		builder.Configuration["ApiSettings:BaseUrl"]!);
});

builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
	client.BaseAddress = new Uri(
		builder.Configuration["ApiSettings:BaseUrl"]!);
});

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

app.Run();
