using D2LExtensionWebAPPSSR.Data;
using D2LExtensionWebAPPSSR.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//Set up the connection string
builder.Services.AddDbContext<D2LDBContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Set up the AddIdentity
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<D2LDBContext>();
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
