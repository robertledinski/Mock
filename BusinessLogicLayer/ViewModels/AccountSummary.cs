using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class AccountSummary
    {
        public AccountSummary()
        {
            EndOfDayBalances = new List<BalanceSummary>();
        }

        public decimal TotalCredits { get; set; }
        public decimal TotalDebits { get; set; }
        public List<BalanceSummary> EndOfDayBalances { get; set; }
    }

    public class BalanceSummary
    {
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }
    }
}
