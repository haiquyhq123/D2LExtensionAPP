using D2LExtensionWebAPPSSR.Data;
using D2LExtensionWebAPPSSR.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
// Add services to the container.
builder.Services.AddControllersWithViews();

//Set up the connection string
builder.Services.AddDbContext<D2LDBContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Set up the AddIdentity
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<D2LDBContext>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
