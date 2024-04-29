using Fanex.BalanceBreakdown.Business.Criterias;
using System.Data;

namespace Fanex.BalanceBreakdown.Business.Services;

public interface DataService
{
    IDataReader QueryData(bool isDaily, int pageIndex, int pageSize);
}

public class DataServiceImp : DataService
{
    public IDataReader QueryData(bool isDaily, int pageIndex, int pageSize)
    {
        var spName = isDaily ? "Acc_Rpt_Automail_BalanceBreakdown_Daily" : "Acc_Rpt_Automail_BalanceBreakdown_monthly";
        var criteria = new BalanceBreakdownCriteria(spName) { PageIndex = pageIndex, PageSize = pageSize };
        using var db = Data.ObjectDbFactory.CreateInstance(spName, 0);
        return db.ExecuteReader(criteria);
    }
}