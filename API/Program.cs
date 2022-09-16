using API.Data;
using API.Extensions;
using API.Middleware;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//seed data to databse
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DatabaseContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    Seed seed = new(context);
    await seed.SeedUsers(userManager, roleManager);
    await seed.SeedBookStatus();
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
//app.UseHttpsRedirection();
app.UseCors(options => options
               .WithOrigins(new[] { "http://localhost:4200", "https://localhost:4200" })
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials()
           );
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
