using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepairTracker.Models;
using RepairTracker.Data;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameRepairContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RepairTrackerConnection") ?? throw new InvalidOperationException("Connection string 'RepairTrackerConnection' not found.")));


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
            services.GetRequiredService<
                DbContextOptions<GameRepairContext>>()))
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
