using DatabaseInfrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class DataSourceInput
    {
        [JsonPropertyName("brandName")]
        [JsonProperty("brandName")]
        public string BankName { get; set; }
        [JsonPropertyName("accounts")]
        public List<AccountViewModel> Accounts { get; set; }
    }

    public class AccountViewModel
    {
        [JsonPropertyName("accountId")]
        [JsonProperty("accountId")]
        public string Id { get; set; }
        [JsonPropertyName("displayName")]
        [JsonProperty("displayName")]
        public string Name { get; set; }
        [JsonPropertyName("currencyCode")]
        [JsonProperty("currencyCode")]
        public string Currency { get; set; }
        public string AccountType { get; set; } // Parse to enum
        public string AccountSubType { get; set; } // Parse to enum
        [JsonPropertyName("identifiers")]
        [JsonProperty("identifiers")]
        public IdentifierViewModel Identifier { get; set; }
        public List<object> Parties { get; set; }
        public List<object> StandingOrders { get; set; }
        public List<object> DirectDebits { get; set; }
        [JsonPropertyName("balances")]
        [JsonProperty("balances")]
        public BalanceViewModel Balance { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }

        public Account ConvertToAccountDTO(User user)
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
               User = user
           };


    }

    public class TransactionViewModel
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("creditDebitIndicator")]
        [JsonProperty("creditDebitIndicator")]
        public string CreditCardType { get; set; } // Convert to enum
        public string Status { get; set; } // Convert to enum
        [JsonPropertyName("bookingDate")]
        public string BookingDate { get; set; }
        public DateTime Date => BookingDate.ToUTCDateTime();
        [JsonPropertyName("merchantDetails")]
        public string Details { get; set; }

        public CreditCardType CreditCardTypeEnum => CreditCardType.ParseEnum<CreditCardType>();
    

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
        [JsonPropertyName("creditDebitIndicator")]
        [JsonProperty("creditDebitIndicator")]
        public string CreditCardType { get; set; } // Convert to enum
        [JsonPropertyName("creditLines")]
        public string Lines { get; set; }
    }


    public class IdentifierViewModel
    {
        [JsonPropertyName("sortCode")]
        public string SortCode { get; set; }
        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; }
        [JsonPropertyName("secondaryIdentification")]
        public string SecondaryIdentification { get; set; }
    }
}
