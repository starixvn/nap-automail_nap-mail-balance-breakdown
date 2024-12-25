using Fanex.MailDelivery.Core.Models;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace Fanex.BalanceBreakdown.Business.Services;

using RazorEngineCore;

public interface MailService
{
    Mail SendMail(bool isDaily);
}

public class MailServiceImp : MailService
{
    private static readonly ConcurrentDictionary<int, IRazorEngineCompiledTemplate> CompiledTemplateCache = new();

    public Mail SendMail(bool isDaily)
    {
        var subject = isDaily
            ? $"Automail Report - Daily Balance Breakdown Report {DateTime.Today:yyyy/MM/dd}"
            : $"Automail Report - Monthly Balance Breakdown Report {DateTime.Today.AddMonths(-1):yyyy/MM}";
        var mail = new Mail
        {
            Title = "Auto Generated Report Administrator",
            Subject = subject,
            Body = LoadBody()
        };

        return mail;
    }

    private static string LoadBody()
    {
        const string templatePath = "Templates/EmailTemplate-URL.cshtml";
        var hashCode = templatePath.GetHashCode();
        var compiledTemplate = CompiledTemplateCache.GetOrAdd(hashCode, _ =>
        {
            var template = File.ReadAllText(templatePath);
            var razorEngine = new RazorEngine();
            return razorEngine.Compile(template);
        });

        var pathDownloadFile = AppConfigService.Config.DownloadFileUrl;
        return compiledTemplate.Run(pathDownloadFile);
    }
}