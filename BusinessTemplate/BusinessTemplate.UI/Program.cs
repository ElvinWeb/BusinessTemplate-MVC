using BusinessTemplate.Business;
using BusinessTemplate.Business.Services.Implementations;
using BusinessTemplate.Business.Services.Service;
using BusinessTemplate.Core.Entities;
using BusinessTemplate.Core.Repositories;
using BusinessTemplate.Data;
using BusinessTemplate.Data.DAL;
using BusinessTemplate.Data.Repositories.Implementations;
using BusinessTemplate.UI.ViewService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Logging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<LayoutService>();
builder.Services.ServicesRegistration();
builder.Services.RepositoryRegistration();

builder.Services.AddDbContext<ProjectDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("myDb1"));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;

    options.User.RequireUniqueEmail = false;
}).AddEntityFrameworkStores<ProjectDbContext>().AddDefaultTokenProviders();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
