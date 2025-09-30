using LMS.System.Blazor.Authenticate;
using LMS.System.Blazor.Components;
using LMS.System.Domain.DB;
using LMS.System.Domain.Model.Users;
using LMS.System.Domain.Service;
using LMS.System.Migrations.SQLite;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

// Add Entity Framework
builder.Services.AddSingleton(e => new DbContextFactory("DbSystemLMS", TimeProvider.System));
builder.Services.AddScoped(e => e.GetRequiredService<DbContextFactory>().Create());

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

// Register custom services
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CookieAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CookieAuthStateProvider>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

// Add authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Clean up expired tokens on startup
using (var scope = app.Services.CreateScope())
{
    var appDBContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await appDBContext.Database.MigrateAsync();
    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
    await authService.CleanUpExpiredTokensAsync();
}

await app.RunAsync();
