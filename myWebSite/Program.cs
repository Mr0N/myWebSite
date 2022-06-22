using myWebSite.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.EntityFrameworkCore;
using myWebSite.Service;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("New Version");
// Add services to the container.
builder.Services.AddControllersWithViews();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<PointDbContext>(a =>
{
    a.UseNpgsql(builder.Configuration.GetConnectionString("Point"));
},ServiceLifetime.Singleton);
builder.Services.AddHostedService<TelegramWorkerService>();
builder.Services.AddSingleton<TelegramBotClient>(a =>
{

    return new TelegramBotClient(builder.Configuration.GetSection("Token").Value);
});
builder.Services.AddSingleton<MessageToTelegramService>();
var app = builder.Build();

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
    pattern: "{controller=Page}/{action=Index}/{id?}");

app.Run();
