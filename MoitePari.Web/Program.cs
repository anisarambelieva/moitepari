using MySqlConnector;
using MoitePari.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("Default");

builder.Services.AddScoped(sp =>
{
    var conn = new MySqlConnection(connStr);
    conn.Open();
    return conn;
});

builder.Services.AddSingleton<DepositCalculator>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Deposits}/{action=Index}/{id?}");

app.Run();
