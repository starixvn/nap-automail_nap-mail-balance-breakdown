using Fanex.BalanceBreakdown.Business.Models;
using Fanex.MailDelivery.Core.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;

namespace Fanex.BalanceBreakdown.Business.Services;

public interface MailService
{
    Mail SendMail(bool isDaily);
}

public class MailServiceImp : MailService
{
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

    private string LoadBody()
    {
        var template = File.ReadAllText(@"Templates/EmailTemplate-URL.cshtml");
        var pathDownloadFile = AppConfigService.Config.DownloadFileUrl;
        var mailDto = new MailDto
        {
            PathDownloadFile = pathDownloadFile
        };
        var tuple = new Tuple<MailDto>(mailDto);
        const string tplKey = "BalanceBreakdown-Template-URL";
        var result = Engine.Razor.IsTemplateCached(tplKey, null)
            ? Engine.Razor.Run(tplKey, null, tuple)
            : Engine.Razor.RunCompile(template, tplKey, model: tuple);

        return result;
    }
}