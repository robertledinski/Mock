using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IAccountService
    {
        AccountSummary GetAccountSummary();
        AccountSummary ProcessAccountSummary(List<AccountViewModel> accounts);
    }
}
