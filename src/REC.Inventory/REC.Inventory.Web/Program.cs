using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using REC.Inventory.Infrastructure;
using REC.Inventory.Web;
using REC.Inventory.Web.Data;
using Serilog;
using System.Reflection;

#region Bootstrap Logger Configuration
var configuration = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json")
	.Build();

Log.Logger = new LoggerConfiguration()
			.ReadFrom.Configuration(configuration)
			.CreateBootstrapLogger();
#endregion

try
{
    Log.Information("Application Starting...");

    var builder = WebApplication.CreateBuilder(args);

    var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

    #region AutoFac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new WebModule(connectionString, migrationAssembly));
});
#endregion

    #region Automapper Configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

    builder.WebHost.UseUrls("http://*:80");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(connectionString, (x) =>
            x.MigrationsAssembly(migrationAssembly)));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddIdentity();

    builder.Services.AddControllersWithViews();

    builder.Services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = 104857600;
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
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
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=IndexSP}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start application.");
}
finally
{
    Log.CloseAndFlush();
}