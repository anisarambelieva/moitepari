using MySqlConnector;
using MoitePari.BusinessLogic;

/// <summary>
/// Entry point for the MoitePari ASP.NET Core web application.
/// Configures services, database connection, and routing.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Retrieves the default connection string from appsettings.json.
/// </summary>
var connStr = builder.Configuration.GetConnectionString("Default");

/// <summary>
/// Registers a scoped MySQL connection for use in controllers.
/// The connection is opened immediately and disposed automatically at the end of each request.
/// </summary>
builder.Services.AddScoped(sp =>
{
    var conn = new MySqlConnection(connStr);
    conn.Open();
    return conn;
});

/// <summary>
/// Registers the deposit calculator as a singleton service, shared across the application.
/// </summary>
builder.Services.AddSingleton<DepositCalculator>();

/// <summary>
/// Adds support for MVC controllers and Razor views.
/// </summary>
builder.Services.AddControllersWithViews();

var app = builder.Build();

/// <summary>
/// Configures global error handling for non-development environments.
/// </summary>
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();

/// <summary>
/// Defines the default route mapping: /Deposits/Index
/// </summary>
app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Deposits}/{action=Index}/{id?}");

app.Run();
