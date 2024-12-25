using Fanex.BalanceBreakdown.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddRazorPages().AddRazorRuntimeCompilation();
services.AddRazorPages();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddConfiguration();
services.AddFanexLogSentry(configuration);
services.AddFanexSql(configuration);
services.AddInternalService();

services.Configure<HostOptions>(options =>
{
    options.ShutdownTimeout = TimeSpan.FromMinutes(2);
});

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();