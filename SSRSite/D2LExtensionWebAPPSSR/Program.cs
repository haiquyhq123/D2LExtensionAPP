using D2LExtensionWebAPPSSR.Data;
using D2LExtensionWebAPPSSR.Factory;
using D2LExtensionWebAPPSSR.Hubs;
using D2LExtensionWebAPPSSR.Models;
using D2LExtensionWebAPPSSR.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.SqlServer;
using System.Configuration;
using D2LExtensionWebAPPSSR.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();


// Add Email Service
builder.Services.AddTransient<IEmailService, EmailService>();
// Load Email Service data in to MailSettings.cs in configuration model
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
// Add Notification Service
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<NotificationService>();
// Add SignalR Service
builder.Services.AddSignalR();
// Add Assingment Service
builder.Services.AddScoped<IAssignmentOperations,AssingmentOperations>();
// Add course service
builder.Services.AddScoped<ICourseOperations, CourseOperations>();
// Add Course Week Service
builder.Services.AddScoped<ICourseWeekOperations, CourseWeekOperations>();
// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
// Add services to the container.
builder.Services.AddControllersWithViews();

//Set up the connection string
builder.Services.AddDbContext<D2LDBContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Custom Clean Architecture Services
builder.Services.AddScoped<IPlannerDataRepository, PlannerDataRepository>();
builder.Services.AddHttpClient<IAiStudyAdvisorService, OpenAiStudyAdvisorService>();
builder.Services.AddScoped<IStaticReportGeneratorService, StaticReportGeneratorService>();
builder.Services.AddScoped<WeeklyReportJob>();

// Set up the AddIdentity with several defined attribute on password
builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 7;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
    opt.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<D2LDBContext>();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsFactory>(); ;

var app = builder.Build();
//Configure the HTTP request pipeline.
app.UseStaticFiles();
app.UseHangfireDashboard();
using (var scope = app.Services.CreateScope())
{
    //var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    //recurringJobManager.AddOrUpdate<INotificationService>(
    //    "Notification System",
    //    service => service.DailyRemidersDueDateAssignment("lehaiquybui@gmail.com", "Test Daily"),
    //    "0 17 * * 0"
    //);
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJobManager.AddOrUpdate<WeeklyReportJob>(
        "generate-weekly-reports",
        job => job.GenerateAndEmailWeeklyReportsAsync(),
         "0 17 * * 0" // Every Sunday at 5:00 PM

    );
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<NotificationHub>("/Hubs");
app.Run();
