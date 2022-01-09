
using DatabaseAbstractions;
using DatabaseInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class BankService : IBankService
    {
        private IRepository<Bank> _banks;
        public BankService(IRepository<Bank> banks)
        {
            _banks = banks;
        }

        public string Test()
        {
            var test = _banks.All.Where(x => x.Id > 0).ToList();
            return "This is test";
        }
            
    }
}
