using Business;
using Shared.Abstractions;
using DAL;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using Microsoft.Data.SqlClient;
using System.Data;
using LibraryManagement;
using DAL.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Shared.Models.Identity;
using Project.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PhysicalPersonsDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
    b => b.MigrationsAssembly("Project")));
builder.Services.AddDbContext<IdentityDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
    b => b.MigrationsAssembly("Project")));


builder.Services.AddScoped<IPhysicalPersonRepository, PhysicalPersonRepository>();
builder.Services.AddScoped<IRelationRepository, RelationRepository>();
builder.Services.AddScoped<IProfilePictureRepository, ProfilePictureRepository>();
builder.Services.AddScoped<IPersonInfoRepository, PersonInfoRepository>();
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<CustomActionFilter>();
builder.Services.AddIdentity<Users, IdentityRole>(
    o =>
    {
        o.Password.RequireNonAlphanumeric = false;
        o.Password.RequiredLength = 8;
        o.Password.RequireUppercase = false;
        o.Password.RequireLowercase = false;
        o.User.RequireUniqueEmail = true;
        o.SignIn.RequireConfirmedAccount = false;
        o.SignIn.RequireConfirmedEmail = false;
        o.SignIn.RequireConfirmedPhoneNumber = false;
    }
)
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/Resources"
});
app.UseRouting();

app.UseMiddleware<ErrorLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
