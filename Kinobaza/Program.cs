using Microsoft.EntityFrameworkCore;
using Kinobaza.BLL.Interfaces;
using Kinobaza.BLL.Services;
using Kinobaza.BLL.Infrastructure;


//builder
var builder = WebApplication.CreateBuilder(args);

//mvc service
builder.Services.AddControllersWithViews();

//session provider
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//connection provider
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if(connectionString is not null) builder.Services.AddKinobazaDBContext(connectionString);

//repository provider
builder.Services.AddUnitOfWorkService();

//application services provider
builder.Services.AddApplicationServicesService();

//application
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
