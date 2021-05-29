using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Abstractions
{
    /// <summary>
    /// Interface for representing database context with necessary functionality.
    /// </summary>
    public interface IDbContext
    {

        /// <summary>
        /// Categories set.
        /// </summary>
        public DbSet<Category> Categories { get; }

        /// <summary>
        /// Certificates set.
        /// </summary>
        public DbSet<Certificate> Certificates { get; }

        /// <summary>
        /// Images set.
        /// </summary>
        public DbSet<Image> Images { get; }

        /// <summary>
        /// Partners set.
        /// </summary>
        public DbSet<Partner> Partners { get; }

        /// <summary>
        /// Products set.
        /// </summary>
        public DbSet<Product> Products { get; }

        /// <summary>
        /// Leaders set.
        /// </summary>
        public DbSet<SalesLeader> Leaders { get; }

        /// <summary>
        /// Sales representatives set.
        /// </summary>
        public DbSet<SalesRepresentative> SalesRepresentatives { get; }

        /// <summary>
        /// Questions set.
        /// </summary>
        public DbSet<Question> Questions { get; }

        /// <summary>
        /// Returns entity set by passed type.
        /// </summary>
        /// <typeparam name="T">Type of set.</typeparam>
        public DbSet<T> Entity<T>() where T : class, new();

        /// <summary>
        /// Saves changes.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Method for replace one entity with another.
        /// </summary>
        /// <param name="oldEntity">Entity for replacing.</param>
        /// <param name="newEntity">New entity.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        public Task Replace<TEntity>(TEntity oldEntity, TEntity newEntity, CancellationToken cancellationToken) where TEntity : class;
    }
}