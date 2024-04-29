using Fanex.BalanceBreakdown.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.LoadConfiguration();
builder.Services.SentryRegister();
builder.Services.FanexDataRegister(builder.Configuration);
builder.Services.InternalServiceRegister();

builder.Services.Configure<HostOptions>(options =>
{
    options.ShutdownTimeout = TimeSpan.FromMinutes(2);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
var serviceProvider = app.Services;
var life = serviceProvider.GetService<IHostApplicationLifetime>();
life.ApplicationStopped.Register(() =>
{
    Console.WriteLine($"Application is stopped at {DateTimeOffset.Now}");
});
life.ApplicationStopping.Register(() =>
{
    Console.WriteLine($"Application is being stopped at {DateTimeOffset.Now}");
});

app.Run();