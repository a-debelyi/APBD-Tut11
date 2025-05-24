using APBD_11.Data;
using APBD_11.Repositories;
using APBD_11.Services;
using APBD_8.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
builder.Services.AddScoped<IPrescriptionsService, PrescriptionsService>();
builder.Services.AddScoped<IPrescriptionsRepository, PrescriptionsRepository>();
builder.Services.AddScoped<IPatientsService, PatientsService>();
builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();