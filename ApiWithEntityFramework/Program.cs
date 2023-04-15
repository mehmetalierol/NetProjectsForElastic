using ApiWithEntityFramework;
using Common;
using Elastic.Apm.NetCoreAll;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApiDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("SampleDbConnection")));

LoggingExtension.ConfigureLogging();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseAllElasticApm();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();