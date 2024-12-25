using Fanex.Data.Repository;

namespace Fanex.BalanceBreakdown.Business.Criterias;

public class BalanceBreakdownCriteria(string spName) : CriteriaBase
{
    private string SpName { get; } = spName;

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public override string GetSettingKey() => SpName;

    public override bool IsValid() => true;
}