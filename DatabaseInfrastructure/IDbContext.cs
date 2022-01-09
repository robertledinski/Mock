using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseInfrastructure
{
    public interface IDbContext
    {
        string GetDbName();

        Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker GetChangeTracker();

        IDbContextTransaction CurrentTransaction();

        IDbContextTransaction BeginTransaction();

        void RollBackTransaction();

        void CommitTransaction();

        Task CommitTransactionAsync(CancellationToken cancellationToken);

        Task RollBackTransactionAsync(CancellationToken cancellationToken);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}
