using Fanex.BalanceBreakdown.Business.Models;

namespace Fanex.BalanceBreakdown.Business.Services;

public static class AppConfigService
{
    public static ConfigSettingModel Config { get; set; }

    public static bool IsDone { get; set; }
}