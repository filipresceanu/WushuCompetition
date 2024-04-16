using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WushuCompetition.Models;

namespace WushuCompetition.Data
{
    public class DataContext: IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Competition> Competitions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Round> Rounds { get; set; }

        public DbSet<AgeCategory> AgeCategories { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Match>()
                .HasOne(m => m.CompetitorFirst)
                .WithMany(p => p.MatchesAsFirstCompetitor)
                .HasForeignKey(m => m.CompetitorFirstId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.CompetitorSecond)
                .WithMany(p => p.MatchesAsSecondCompetitor)
                .HasForeignKey(m => m.CompetitorSecondId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.ParticipantWinner)
                .WithMany(p => p.MatchesAsWinner)
                .HasForeignKey(m => m.ParticipantWinnerId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);

        }
       

    }
}
