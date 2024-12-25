using SharpCompress.Archives;
using SharpCompress.Writers;
using System;
using System.IO;
using ZipArchive = SharpCompress.Archives.Zip.ZipArchive;

namespace Fanex.BalanceBreakdown.Business.Services;

public interface ZipService
{
    string ZipFile(bool isDaily);
}

public class ZipServiceImp(DeleteFilesService deleteFilesService) : ZipService
{
    public string ZipFile(bool isDaily)
    {
        var folder = isDaily ? "Daily_Balance_Breakdown" : "Monthly_Balance_Breakdown";
        var internalFolder = isDaily ? "InternalDaily" : "InternalMonthly";
        var delFilesAfterDays = isDaily ? 7 : 65;
        var fileName = isDaily
            ? $"Daily Balance Breakdown For {DateTime.Today:dd-MM-yyyy}"
            : $"Monthly Balance Breakdown For {DateTime.Today.AddMonths(-1):yyyy-MM}";

        deleteFilesService.Execute($"{AppConfigService.Config.Path}/{folder}", delFilesAfterDays);
        Directory.CreateDirectory($"{AppConfigService.Config.Path}/{folder}/{DateTime.Today:yyyyMM01}");
        var filePath = $"{AppConfigService.Config.Path}/{internalFolder}";
        var zipFile = $"{AppConfigService.Config.Path}/{folder}/{DateTime.Today:yyyyMM01}/{fileName}.zip";
        using var archive = ZipArchive.Create();
        archive.AddAllFromDirectory(filePath);
        archive.SaveTo(zipFile, new WriterOptions(SharpCompress.Common.CompressionType.Deflate));

        return zipFile;
    }
}