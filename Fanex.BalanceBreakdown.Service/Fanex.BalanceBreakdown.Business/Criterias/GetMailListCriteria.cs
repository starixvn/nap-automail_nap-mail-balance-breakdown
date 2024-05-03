using Fanex.Data.Repository;

namespace Fanex.BalanceBreakdown.Business.Criterias;

public class GetMailListCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Mail_MailManagement_GetSuggesionEmailList";

    public override bool IsValid() => true;
}