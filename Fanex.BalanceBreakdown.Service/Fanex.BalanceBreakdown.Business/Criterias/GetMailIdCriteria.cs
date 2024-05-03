using Fanex.Data.Repository;

namespace Fanex.BalanceBreakdown.Business.Criterias;

public class GetMailIdCriteria : CriteriaBase
{
    public string MailId { get; set; }

    public override string GetSettingKey() => "Mail_MailManagement_GetMailByID";

    public override bool IsValid() => true;
}