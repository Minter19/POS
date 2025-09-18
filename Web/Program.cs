using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MudBlazor.Services;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Web.Components;

var builder = WebApplication.CreateBuilder(args);

//Konfigurasi Opentelemetry
// clear these providers.
builder.Logging.ClearProviders();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
    {
        resource.AddService(serviceName: builder.Environment.ApplicationName);
    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddConsoleExporter();
    })
    //.WithMetrics(metrics => metrics
    //    .AddAspNetCoreInstrumentation()
    //    .AddConsoleExporter((exporterOptions, metricReaderOptions) =>
    //    {
    //        metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;
    //    }))
    .WithLogging(logging =>
    {
        logging.AddConsoleExporter();
    });

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Tambahkan layanan otentikasi dan otorisasi
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "MyBlazorApp.Auth"; // Nama cookie untuk sesi
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Waktu kedaluwarsa
    options.SlidingExpiration = true; // Perpanjang masa kedaluwarsa saat aktif
})
.AddOpenIdConnect(options =>
{
    // Konfigurasi ini harus cocok dengan klien Keycloak 'my-blazor-client'
    options.Authority = builder.Configuration["Keycloak:Authority"];
    options.ClientId = builder.Configuration["Keycloak:ClientId"];
    options.ResponseType = "code";
    options.SaveTokens = true; // Simpan token ke dalam cookie otentikasi
    options.RequireHttpsMetadata = false; // Untuk lingkungan pengembangan
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("api-access"); // Meminta scope untuk memanggil API
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor(); // Akses ke HttpContext untuk mendapatkan token
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Middleware otentikasi dan otorisasi
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/login", async (HttpContext context) =>
{
    await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "/"
    });
});

app.MapGet("/signout", async (HttpContext context) =>
{
    // Mengambil token dan menyiapkan properti autentikasi
    var idToken = await context.GetTokenAsync("id_token");
    var authProperties = new AuthenticationProperties
    {
        RedirectUri = "/"
    };

    // Tambahkan id_token_hint ke properti autentikasi
    if (!string.IsNullOrEmpty(idToken))
    {
        authProperties.Parameters.Add("id_token_hint", idToken);
    }

    // Memicu alur logout OpenID Connect secara resmi
    // Ini akan menghapus cookie lokal dan mengarahkan ke Keycloak secara otomatis
    await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, authProperties);
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

});

app.Run();
