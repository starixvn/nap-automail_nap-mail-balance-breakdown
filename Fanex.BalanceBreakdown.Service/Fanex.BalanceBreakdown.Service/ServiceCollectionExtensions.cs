using Fanex.BalanceBreakdown.Business.Models;
using Fanex.BalanceBreakdown.Business.Services;
using Fanex.Data.Repository;
using Fanex.MailDelivery.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.IO;

namespace Fanex.BalanceBreakdown.Service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Services { get; set; }

    public static void AddConfiguration(this IServiceCollection services)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        var text = File.ReadAllText("appsettings.json");
        AppConfigService.Config = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
    }

    public static void AddInternalService(this IServiceCollection services)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        services.AddSingleton<Log, LogImp>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<DataService, DataServiceImp>();
        services.AddSingleton<MainService, MainServiceImp>();
        services.AddSingleton<ExcelService, ExcelServiceImp>();
        services.AddSingleton<DeleteFilesService, DeleteFilesServiceImp>();
        services.AddSingleton<ZipService, ZipServiceImp>();

        services.AddSingleton<MailService, MailServiceImp>();

        Services = services;
        MailClientHandler.Start();
    }
}