using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RepairTracker.DBModels;
using System.Configuration;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add logging services
builder.Logging.AddConsole();

var connectionString = "";

// Check if the environment is Production
if (builder.Environment.IsProduction())
{
    // Try the azure connection environment variable
    connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING")!;

    if (string.IsNullOrEmpty(connectionString))
    {
        // Obtain the logger factory from the service provider
        using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole());
        var logger = loggerFactory.CreateLogger<Program>();

        logger.LogError("AZURE_SQL_CONNECTIONSTRING environment variable is not set.");
        throw new InvalidOperationException("AZURE_SQL_CONNECTIONSTRING environment variable is not set.");
    }
}
else
{
    connectionString = builder.Configuration["RepairTracker:ConnectionStrings:AzureConnection"];

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

var app = builder.Build();

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
