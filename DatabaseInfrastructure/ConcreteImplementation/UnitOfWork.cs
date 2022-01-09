using DatabaseAbstractions;
using System;

namespace DatabaseInfrastructure.ConcreteImplementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MockContext _context;
        /*
         Now here depending about need I would suggest implementing AffterCommitHandler delegate 
         which would have duty to allow providing Action for execution after we sucessfully commit values to the server
         and then for example send an email.

         There is also possibility of adding ability to create transaction over each commit, etc. We probably want 
         to do it explicitly when the operation is delicate.
         */
        public UnitOfWork(MockContext context)
        {
            _context = context;           
        }

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
