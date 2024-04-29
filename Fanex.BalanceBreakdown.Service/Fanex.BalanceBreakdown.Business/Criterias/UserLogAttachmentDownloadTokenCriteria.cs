using Fanex.Data.Repository;

namespace Fanex.BalanceBreakdown.Business.Criterias;

public class UserLogAttachmentDownloadTokenCriteria : NonQueryCommand
{
    public string Token { get; set; }

    public int ReportId { get; set; }

    public string UserEmail { get; set; }

    public string FileName { get; set; }

    public int Action { get; set; } = 2;

    public override string GetSettingKey() => "Acc_UserLog_AttachmentDownload_Token";

    public override bool IsValid() => true;
}