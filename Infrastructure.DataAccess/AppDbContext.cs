using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess
{
    public sealed class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>, IDbContext
    {
        /// <inheritdoc/>
        public DbSet<Category> Categories { get; private set; }

        /// <inheritdoc/>
        public DbSet<Certificate> Certificates { get; private set; }

        /// <inheritdoc/>
        public DbSet<Image> Images { get; private set; }

        /// <inheritdoc/>
        public DbSet<Partner> Partners { get; private set; }

        /// <inheritdoc/>
        public DbSet<Product> Products { get; private set; }

        /// <inheritdoc/>
        public DbSet<SalesLeader> Leaders { get; private set; }

        /// <inheritdoc/>
        public DbSet<SalesRepresentative> SalesRepresentatives { get; private set; }

        /// <inheritdoc/>
        public DbSet<Question> Questions { get; private set; }

        /// <inheritdoc/>
        public DbSet<Stock> Stocks { get; private set; }

        /// <inheritdoc/>
        public DbSet<CooperationRequest> CooperationRequests { get; private set; }

        /// <inheritdoc/>
        public DbSet<User> Users { get; private set; }

        /// <inheritdoc/>

        public override DbSet<IdentityUserClaim<int>> UserClaims { get; set; }

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