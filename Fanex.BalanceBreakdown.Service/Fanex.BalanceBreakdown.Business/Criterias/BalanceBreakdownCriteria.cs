using Fanex.Data.Repository;

namespace Fanex.BalanceBreakdown.Business.Criterias;

public class BalanceBreakdownCriteria : CriteriaBase
{
    private string SpName { get; set; }

    public BalanceBreakdownCriteria(string spName)
    {
        this.SpName = spName;
    }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public override string GetSettingKey() => SpName;

    public override bool IsValid() => true;
}