using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using StorageUI.Data;
using StorageUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>(); // Supondo que este seja um servi�o de exemplo

// Registra o CustomAuthenticationStateProvider e o mapeia para AuthenticationStateProvider
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

// Registra o servi�o de autentica��o com a implementa��o concreta
builder.Services.AddScoped<IAppAuthenticationService, AuthenticationService>();

// Registra o IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configura��o do Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura��o da autentica��o por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/home"; // Atualize conforme necess�rio
        options.LogoutPath = "/"; // Atualize conforme necess�rio
    });

var app = builder.Build();

// Configura o pipeline de requisi��o HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Garante que o middleware de autentica��o e autoriza��o seja chamado
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
