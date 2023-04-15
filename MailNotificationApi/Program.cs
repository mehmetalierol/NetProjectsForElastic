using Common;
using Elastic.Apm.NetCoreAll;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
