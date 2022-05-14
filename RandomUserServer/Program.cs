using RandomUserServer.Models;
using RandomUserServer.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

//DbAccessors
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<Context>(option => option.UseInMemoryDatabase("Country"));

builder.Services.AddControllers();
//Singlteton JobTask
builder.Services.AddHostedService<TimedHostedService>();

//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();

builder.Services.AddMvc();
var app = builder.Build();

app.UseAuthorization();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"Assets")),
    RequestPath = new PathString("/Assets")
});
app.MapControllers();

app.UseRouting();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
