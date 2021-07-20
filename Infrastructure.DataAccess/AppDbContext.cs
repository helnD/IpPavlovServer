using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess
{
    public sealed class AppDbContext : DbContext, IDbContext
    {
        /// <inheritdoc/>
        public DbSet<Category> Categories { get; init; }

        /// <inheritdoc/>
        public DbSet<Certificate> Certificates { get; init; }

        /// <inheritdoc/>
        public DbSet<Image> Images { get; init; }

        /// <inheritdoc/>
        public DbSet<Partner> Partners { get; init; }

        /// <inheritdoc/>
        public DbSet<Product> Products { get; init; }

        /// <inheritdoc/>
        public DbSet<SalesLeader> Leaders { get; init; }

        /// <inheritdoc/>
        public DbSet<SalesRepresentative> SalesRepresentatives { get; init; }

        /// <inheritdoc/>
        public DbSet<Question> Questions { get; init; }

        /// <inheritdoc/>
        public DbSet<Stock> Stocks { get; init; }

        /// <inheritdoc/>
        public DbSet<CooperationRequest> CooperationRequests { get; init; }

        /// <summary>
        /// Initializes <see cref="AppDbContext"/>.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /// <inheritdoc/>
        public DbSet<T> Entity<T>() where T : class, new() => Set<T>();

        /// <inheritdoc cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)" />
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task Replace<TEntity>(TEntity oldEntity, TEntity newEntity, CancellationToken cancellationToken) where TEntity : class
        {
            Entry(oldEntity).CurrentValues.SetValues(newEntity);

            var references = Entry(oldEntity).References;
            foreach (var reference in references)
            {
                var referenceName = reference.Metadata.Name;
                var newReference = Entry(newEntity).Reference(referenceName).CurrentValue;
                Entry(oldEntity).Reference(referenceName).CurrentValue = newReference;
            }

            var collections = Entry(oldEntity).Collections;
            foreach (var collection in collections)
            {
                var collectionName = collection.Metadata.Name;
                var newCollection = Entry(newEntity).Collection(collectionName).CurrentValue;
                if (newCollection != null)
                {
                    Entry(oldEntity).Collection(collectionName).CurrentValue = newCollection;
                }
            }
        }

    }
}