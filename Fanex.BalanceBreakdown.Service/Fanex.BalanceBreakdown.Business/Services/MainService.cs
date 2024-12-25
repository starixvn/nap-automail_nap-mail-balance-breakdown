using Fanex.BalanceBreakdown.Business.Models;
using Fanex.MailDelivery.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fanex.BalanceBreakdown.Business.Services;

public interface MainService
{
    JobMessage Execute(bool isDaily);
}

public class MainServiceImp(
    Log log,
    MailService mailService,
    ExcelService excelService,
    ZipService zipService)
    : MainService
{
    private static List<DateTime> CounterList { get; } = [];

    public JobMessage Execute(bool isDaily)
    {
        var result = new ExecuteResultModel();
        var jobMessage = new JobMessage();
        try
        {
            CounterList.Add(DateTime.Now);
            result = SendNotification(isDaily);
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.Message;
            log.Log(e);
        }
        finally
        {
            jobMessage.Mail = result.Mail;
            jobMessage.JobStatus = result.Success ? JobStatus.Success : JobStatus.DataIsNotReady;
            jobMessage.TaskLog = new TaskLog
            {
                LogTime = DateTime.Now,
                Message = result.Message
            };

            if (CounterList.Count > 0)
            {
                CounterList.RemoveAt(0);
            }
            if (CounterList.Count == 0)
            {
                AppConfigService.IsDone = true;
            }
        }

        return jobMessage;
    }

    private ExecuteResultModel SendNotification(bool isDaily)
    {
        excelService.Export(isDaily);
        zipService.ZipFile(isDaily);
        var folder = isDaily ? "InternalDaily" : "InternalMonthly";
        Directory.Delete($"{AppConfigService.Config.Path}/{folder}", true);
        var mail = mailService.SendMail(isDaily);
        return new ExecuteResultModel
        {
            Mail = mail,
            Success = true,
            Message = string.Empty
        };
    }
}