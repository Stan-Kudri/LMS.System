using System.IO;
using LMS.System.BlazorApp.Components;
using LMS.System.Domain.DB;
using LMS.System.Domain.Model.Users;
using LMS.System.Infrastructure.Service;
using LMS.System.Infrastructure.Service.Validation;
using LMS.System.Migrations.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

var connectionName = "LMSDbConnection";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient();

builder.Services.AddFluentUIComponents();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddSingleton(e => new DbContextFactory(connectionName, e.GetRequiredService<TimeProvider>()));
builder.Services.AddScoped(e => e.GetRequiredService<DbContextFactory>().Create());

builder.Services.AddScoped<PasswordHasher>();

builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddHostedService<RefreshTokenCleanupService>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

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
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
