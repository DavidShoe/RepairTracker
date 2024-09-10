using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RepairTracker.DBModels;
using System.Collections;
using System.Configuration;
using System.Diagnostics;

internal class Program
{

    private async Task DumpAllEnvVariables(HttpContext context, IDictionary envVariables)
    {
        foreach (var envVar in envVariables.Cast<DictionaryEntry>())
        {
            await context.Response.WriteAsync($"{envVar.Key}"); // : {envVar.Value}
            await context.Response.WriteAsync($"\r\n");
        }
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddEnvironmentVariables();
    
        // Add logging services
        builder.Logging.AddConsole();

        var connectionString = "";

        var app = builder.Build();

        // Check if the environment is Production
        if (app.Environment.IsProduction())
        {
            // Try the azure connection environment variable
            connectionString = Environment.GetEnvironmentVariable("AZURESQLCONNECTIONSTRING")!;
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = app.Configuration["AZURESQLCONNECTIONSTRING"];
            }
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = app.Configuration.GetConnectionString("AZURESQLCONNECTIONSTRING");
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                // Obtain the logger factory from the service provider
                using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole());
                var logger = loggerFactory.CreateLogger<Program>();

                logger.LogError("AZURE_SQL_CONNECTIONSTRING environment variable is not set.");

                //await context.Response.WriteAsync("# Environmental Variables \r\n");
                //await DumpAllEnvVariables(context, Environment.GetEnvironmentVariables());

                throw new InvalidOperationException("AZURE_SQL_CONNECTIONSTRING environment variable is not set.");
            }
        }
        else
        {
            connectionString = app.Configuration["RepairTracker:ConnectionStrings:AzureConnection"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Local secret is not set.");
            }
        }

        builder.Services.AddDbContext<GameRepairContext>(options =>
            options.UseSqlServer(connectionString));

        // Add services to the container.
        builder.Services.AddControllersWithViews(
            options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
        );

        // Test our data connection context
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            using (var context = new GameRepairContext(
                    services.GetRequiredService<DbContextOptions<GameRepairContext>>()))
            {
                // Look for any data
                if (context.Parts.Any())
                {
                    Debug.WriteLine("Found data");
                }
                else
                {
                    Debug.WriteLine("Not finding data");
                }
            }
        }


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}