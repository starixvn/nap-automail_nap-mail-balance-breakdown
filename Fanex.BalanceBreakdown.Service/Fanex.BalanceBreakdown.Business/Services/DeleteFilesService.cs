using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace Fanex.BalanceBreakdown.Business.Services
{
    public interface DeleteFilesService
    {
        void Execute(string path, int delFilesAfterDays);
    }

    public class DeleteFilesServiceImp(ILoggerFactory loggerFactory)
        : DeleteFilesService
    {
        private readonly ILogger logger = loggerFactory.CreateLogger("Fanex");

        private int fileCount;
        private int directoryCount;

        public void Execute(string path, int delFilesAfterDays)
        {
            fileCount = 0;
            directoryCount = 0;
            var logMessage = $"Fanex.BalanceBreakdown.Service: Start at {DateTime.Now}";
            logger.LogInformation(logMessage);
            try
            {
                var directoryInfo = new DirectoryInfo(path);
                if (!directoryInfo.Exists)
                {
                    return;
                }

                DeleteFiles(directoryInfo, delFilesAfterDays);
            }
            catch (Exception e)
            {
                Logging.Logger.Log.Error(e.Message, e);
            }

            logMessage = $"Fanex.BalanceBreakdown.Service: Finish the task at {DateTime.Now}. File count = {fileCount}. Directory count = {directoryCount}";
            logger.LogInformation(logMessage);
        }

        private void DeleteFiles(DirectoryInfo directoryInfo, int delFilesAfterDays)
        {
            if (IgnoreDirectoryBecauseOfTheDateInDirectoryName(directoryInfo, delFilesAfterDays))
            {
                logger.LogInformation("Ignore directory {FullName} at {Now}", directoryInfo.FullName, DateTime.Now);
                return;
            }

            logger.LogInformation("Checking files and directories {FullName} at {Now}", directoryInfo.FullName, DateTime.Now);

            var files = directoryInfo.EnumerateFiles();
            foreach (var file in files)
            {
                DeleteFile(file, delFilesAfterDays);
            }

            var subDirectories = directoryInfo.EnumerateDirectories();
            foreach (var subDirectory in subDirectories)
            {
                DeleteFiles(subDirectory, delFilesAfterDays);
            }

            DeleteEmptyDirectory(directoryInfo);
        }

        private void DeleteFile(FileInfo fileInfo, int delFilesAfterDays)
        {
            try
            {
                fileCount++;

                var days = DateTime.Today.Subtract(fileInfo.CreationTime).TotalDays;
                if (days <= delFilesAfterDays)
                {
                    return;
                }

                fileInfo.Delete();
                logger.LogInformation("Deleted file {FullName} at {Now}", fileInfo.FullName, DateTime.Now);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error when delete file {FullName} at {Now}", fileInfo.FullName, DateTime.Now);
            }
        }

        private void DeleteEmptyDirectory(DirectoryInfo directoryInfo)
        {
            try
            {
                directoryCount++;
                var hasFile = directoryInfo.EnumerateFileSystemInfos().Any();
                if (hasFile)
                {
                    return;
                }

                var isOld = IsDirectoryOld(directoryInfo);
                if (!isOld)
                {
                    return;
                }

                directoryInfo.Delete();
                logger.LogInformation("Deleted directory {FullName} at {Now}", directoryInfo.FullName, DateTime.Now);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error when delete directory {FullName} at {Now}", directoryInfo.FullName, DateTime.Now);
            }
        }

        private static bool IsDirectoryOld(DirectoryInfo directoryInfo)
        {
            var lastAccessTime = directoryInfo.LastAccessTimeUtc;
            var isOld = DateTime.Today.Subtract(lastAccessTime).TotalDays > 120;
            return isOld;
        }

        private bool IgnoreDirectoryBecauseOfTheDateInDirectoryName(DirectoryInfo directoryInfo, int delFilesAfterDays)
        {
            var date = ExtractDateInDirectoryName(directoryInfo);

            return DateTime.Today.Subtract(date).TotalDays <= delFilesAfterDays;
        }

        private static DateTime ExtractDateInDirectoryName(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                return DateTime.MaxValue;
            }

            try
            {
                return DateExtractor.ExtractDateTimeFromFileName(directoryInfo.FullName);
            }
            catch (Exception e)
            {
                Logging.Logger.Log.Info($"Fanex.BalanceBreakdown.Service: ExtractDateInDirectoryName exception: {e.Message}");
            }

            return new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local);
        }
    }
}