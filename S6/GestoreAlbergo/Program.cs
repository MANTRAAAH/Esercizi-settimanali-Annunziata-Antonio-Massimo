using GestoreAlbergo.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the database connection using ADO.NET
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DatabaseHelper(connectionString));

// Register services
builder.Services.AddTransient<IClienteService, ClienteService>();
builder.Services.AddTransient<ICameraService>(provider => new CameraService(connectionString));

builder.Services.AddScoped<IPrenotazioneService>(provider =>
{
    var clienteService = provider.GetRequiredService<IClienteService>();
    var cameraService = provider.GetRequiredService<ICameraService>();
    return new PrenotazioneService(connectionString, clienteService, cameraService);
});

// Add cookie-based authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Path to your login page
        options.LogoutPath = "/Account/Logout"; // Path to your logout action
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Expiry time for the authentication cookie
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
