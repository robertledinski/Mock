using DatabaseAbstractions;
using DatabaseAbstructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInfrastructure.ConcreteImplementation
{
    public class UnitOfWork<T> : IUnitOfWork where T : class
    {
        private readonly ApplicationContext _context;
        /*
         Now here depending about need I would suggest implementing AffterCommitHandler delegate 
         which would have duty to allow providing Action for execution after we sucessfully commit values to the server
         and then for example send an email.

         There is also possibility of adding ability to create transaction over each commit, etc. We probably want 
         to do it explicitly when the operation is delicate.
         */
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;           
        }

        public IRepository<T> Repository { get; private set; }
        public int Commit()
        {
            int saveChanges;
            try
            {
                saveChanges = _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return saveChanges;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
