using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services;
using TheBugTracker.Services.Factories;
using TheBugTracker.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(DataUtility.GetConnectionString(builder.Configuration),o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<BTUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddClaimsPrincipalFactory<BTUserClaimsPrincipalFactory>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

//Custom Services
builder.Services.AddScoped<IBTRolesService, BTRolesService>();
builder.Services.AddScoped<IBTCompanyInfoService, BTCompanyInfoService>();
builder.Services.AddScoped<IBTProjectService, BTProjectService>();
builder.Services.AddScoped<IBTTicketService, BTTicketService>();
builder.Services.AddScoped<IBTTicketHistoryService, BTTicketHistoryService>();
builder.Services.AddScoped<IBTNotificationService, BTNotificationService>();
builder.Services.AddScoped<IBTInviteService, BTInviteService>();
builder.Services.AddScoped<IBTFileService, BTFileService > ();
//Custom Email Service using IEmailSender Interface
builder.Services.AddScoped<IEmailSender, BTEmailService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();


//allow legacy date time behaviour
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//Not sure if this is right
await DataUtility.ManageDataAsync(app, builder.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();
