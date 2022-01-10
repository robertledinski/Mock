using DatabaseInfrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class DataSourceInput
    {
        [JsonProperty("brandName")]
        public string BankName { get; set; }
        [JsonProperty("accounts")]
        public List<AccountViewModel> Accounts { get; set; }
    }

    public class AccountViewModel
    {
        [JsonProperty("accountId")]
        public string Id { get; set; }
        [JsonProperty("displayName")]
        public string Name { get; set; }    
        [JsonProperty("currencyCode")]
        public string Currency { get; set; }
        public string AccountType { get; set; } // Parse to enum
        public string AccountSubType { get; set; } // Parse to enum
        [JsonProperty("identifiers")]
        public IdentifierViewModel Identifier { get; set; }
        public List<object> Parties { get; set; }
        public List<object> StandingOrders { get; set; }
        public List<object> DirectDebits { get; set; }
        [JsonProperty("balances")]
        public BalanceViewModel Balance { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }

        public Account ConvertToAccountDTO(int UserId)
           => new Account
           {
               AccountId = Id,
               CurrencyId = (int)Currency.ParseEnum<CurrencyType>(), // Matching initial currency
               Name = Name,
               Type = AccountType.ParseEnum<AccountType>(),
               SubType = AccountSubType.ParseEnum<AccountSubType>(),
               SortCode = Identifier.SortCode,
               AccountNumber = Identifier.AccountNumber,
               SecondaryIdentification = Identifier.SecondaryIdentification,
               AvailableBalance = Balance.Available.Amount,
               CurrentBalance = Balance.Available.Amount,
               CreditCard = new CreditCard()
               {
                   Name = Id,
                   TypeId = Balance.Current.CreditCardType.ParseEnum<CreditCardType>() // Matching inital cc
               },
               Transactions = Transactions.Select(x => x.ConvertToTransactionDTO()).ToList(),
               UserId = UserId
           };


    }

    public class TransactionViewModel
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        [JsonProperty("creditDebitIndicator")]
        public string CreditCardType { get; set; } // Convert to enum
        public string Status { get; set; } // Convert to enum
        [JsonProperty("bookingDate")]
        public DateTime Date { get; set; }
        [JsonProperty("merchantDetails")]
        public string Details { get; set; }

        public Transaction ConvertToTransactionDTO()
        {
            return new Transaction() 
            { 
                Amount = Amount,
                CardType = CreditCardType.ParseEnum<CreditCardType>(),
                Status = Status.ParseEnum<Status>(),
                Date = Date,
                MerchantDetails = Details,
                Description = Description
            };
        }
    }

    public class BalanceViewModel
    {
        public BalanceCreditViewModel Current { get; set; }
        public BalanceCreditViewModel Available { get; set; }
    }

    public class BalanceCreditViewModel
    {
        public decimal Amount { get; set; }
        [JsonProperty("creditDebitIndicator")]
        public string CreditCardType { get; set; } // Convert to enum
        [JsonProperty("creditLines")]
        public string Lines { get; set; }
    }


    public class IdentifierViewModel
    {
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
        public string SecondaryIdentification { get; set; }
    }
}
