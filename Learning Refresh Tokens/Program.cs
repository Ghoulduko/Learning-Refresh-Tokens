using Microsoft.EntityFrameworkCore;
using Refresh.Application.Interfaces;
using Refresh.Application.Services;
using Refresh.Core.Database;
using Refresh.Core.Interfaces;
using Refresh.Core.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TokenDbContext>(i => i.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// User Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Token Services

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();