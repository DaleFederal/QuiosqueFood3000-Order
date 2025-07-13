using System.Reflection;
using QuiosqueFood3000.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = AppStartup.CreateAppBuilder(args);
        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddSwaggerGen(Options => {
            Options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        });
        var app = AppStartup.BuildApp(builder);

        await app.RunAsync();
    }
}