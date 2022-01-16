using DatabaseAbstractions;
using DatabaseInfrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class AccountService : IAccountService
    {
        IRepository<Account> _accountRepo;
        public AccountService(IRepository<Account> accountRepo)
        {
            _accountRepo = accountRepo;
        }

        AccountSummary ProcessAccount(AccountViewModel account)
        {
            var accountSummary = new AccountSummary();
            var balanceSummary = new Dictionary<DateTime, BalanceSummary>();
            BalanceSummary currentSummary;

            if (account == null || account.Transactions == null)
                return accountSummary;

            foreach (var transaction in account.Transactions)
            {
                currentSummary = new BalanceSummary() { Date = transaction.Date };

                if (!balanceSummary.ContainsKey(transaction.Date))
                    balanceSummary.Add(transaction.Date, currentSummary);
                else
                    currentSummary = balanceSummary[transaction.Date];

                // Got
                if (transaction.CreditCardTypeEnum.HasFlag(CreditCardType.Debit))
                {
                    accountSummary.TotalDebits += transaction.Amount;
                    currentSummary.Balance += transaction.Amount;
                }

                // Payed
                if (transaction.CreditCardTypeEnum.HasFlag(CreditCardType.Credit))
                {
                    accountSummary.TotalCredits -= transaction.Amount;
                    currentSummary.Balance -= transaction.Amount;
                }
            }

            accountSummary.EndOfDayBalances.AddRange(balanceSummary.Values);


            return accountSummary;
        }

        public AccountSummary ProcessAccountSummary(List<AccountViewModel> accounts)
        {
            var accountSummary = new AccountSummary();

            if (accounts == null || accounts.Count == 0)
                return accountSummary;


            foreach (var account in accounts)
            {
                if (account != null)
                {
                    var summary = ProcessAccount(account);

                    accountSummary.TotalDebits += summary.TotalDebits;
                    accountSummary.TotalCredits += summary.TotalCredits;

                    if (accountSummary.EndOfDayBalances.Count > 0)
                    {
                        // Join all balances for particular date of other accounts
                        accountSummary.EndOfDayBalances.ForEach(_ => {
                            var currentDate = summary.EndOfDayBalances.Where(x => x.Date == _.Date).FirstOrDefault();
                            _.Balance += currentDate.Balance;
                        });
                    }
                    else
                    {
                        accountSummary.EndOfDayBalances.AddRange(summary.EndOfDayBalances);
                    }
                }
            }

            return accountSummary;
        }

        public AccountSummary GetAccountSummary()
        {
            var accountSummary = new AccountSummary();
            var account = _accountRepo.All.Include(x => x.Transactions).Where(x => x.AccountId == "10473770").FirstOrDefault();

            if (account == null)
                return accountSummary; // Error code


            var balanceSummary = new Dictionary<DateTime, BalanceSummary>();
            // I could separete same logic to one reusable method or use
            // some kind of pattern (Strategy, etc., however this will take me
            // least amount of time. The previous method above wasn't reused because
            // I didn't want spent some time to do class transformations or adding automapper.
            BalanceSummary currentSummary = new BalanceSummary();
            foreach (var transaction in account.Transactions)
            {
                currentSummary = new BalanceSummary() { Date = transaction.Date };

                if (!balanceSummary.ContainsKey(transaction.Date))
                    balanceSummary.Add(transaction.Date, currentSummary);
                else
                    currentSummary = balanceSummary[transaction.Date];

                // Got
                if (transaction.CardType.HasFlag(CreditCardType.Debit))
                {
                    accountSummary.TotalDebits += transaction.Amount;
                    currentSummary.Balance += transaction.Amount;
                }

                // Payed
                if (transaction.CardType.HasFlag(CreditCardType.Credit))
                {
                    accountSummary.TotalCredits -= transaction.Amount;
                    currentSummary.Balance -= transaction.Amount;
                }   
            }

            accountSummary.EndOfDayBalances.AddRange(balanceSummary.Values);


            return accountSummary;
        }
    }
}
