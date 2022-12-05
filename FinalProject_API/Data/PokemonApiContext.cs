using FinalProject_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinalProject_API.Data
{
    public class PokemonApiContext : DbContext
    {
        //To give access to IHttpContextAccessor for Audit Data with IAuditable
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }

        public PokemonApiContext(DbContextOptions<PokemonApiContext> options)
        : base(options)
        {
            UserName = "SeedData";
        }

        public PokemonApiContext(DbContextOptions<PokemonApiContext> options, IHttpContextAccessor httpContextAccessor)
       : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
            UserName ??= "Unknown";
        }

        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<PrimaryType> PrimaryTypes { get; set; }
        //public DbSet<Move> Moves { get; set; }
        //public DbSet<PokemonType> PokemonTypes { get; set; }
        //public DbSet<PokemonPokemonType> PokemonPokemonTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Add a unique index to the Pokemon ID
            modelBuilder.Entity<Pokemon>()
            .HasIndex(p => p.Pokedex)
            .IsUnique();
            
            //Restrict Cascade Delete
            //Trainer
            modelBuilder.Entity<Pokemon>()
                .HasOne(pc => pc.Trainer)
                .WithMany(c => c.Pokemons)
                .HasForeignKey(pc => pc.TrainerID)
                .OnDelete(DeleteBehavior.Restrict);

            //PrimaryType
            modelBuilder.Entity<Pokemon>()
                .HasOne(pc => pc.PrimaryType)
                .WithMany(c => c.Pokemons)
                .HasForeignKey(pc => pc.PrimaryTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            //Move
            /*modelBuilder.Entity<Move>()
                .HasOne(pc => pc.PrimaryType)
                .WithMany(c => c.Moves)
                .HasForeignKey(pc => pc.TypeID)
                .OnDelete(DeleteBehavior.Restrict);*/
            
            /*modelBuilder.Entity<Move>()
                .HasOne(pc => pc.Pokemon)
                .WithMany(c => c.Moves)
                .HasForeignKey(pc => pc.PokemonID)
                .OnDelete(DeleteBehavior.Restrict);

            //PokemonPokemonType          
            /*modelBuilder.Entity<PokemonPokemonType>()
                .HasOne(oi => oi.PokemonType)
                .WithMany(i => i.PokemonPokemonTypes)
                .HasForeignKey(oi => oi.PokemonTypeID)
                .OnDelete(DeleteBehavior.Restrict);*/


        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }
    }
}

