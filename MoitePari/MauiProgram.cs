using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace MoitePari;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton(sp =>
        {
            var connection = new MySqlConnection(ConnectionConfig.Default);
            connection.Open();
            return connection;
        });

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<AddDepositPage>();
        builder.Services.AddTransient<DepositListPage>();

        return builder.Build();
    }
}