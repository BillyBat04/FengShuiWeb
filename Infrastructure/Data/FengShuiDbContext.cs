using Microsoft.EntityFrameworkCore;
using FengShuiWeb.Domain.Models;

namespace FengShuiWeb.Infrastructure.Data
{
    public class FengShuiDbContext : DbContext
    {
        public FengShuiDbContext(DbContextOptions<FengShuiDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<FengShuiAnalysis> FengShuiAnalyses { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleHistory> ArticleHistory { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Warning> Warnings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<Article>()
                .HasIndex(a => new { a.UserID, a.Status });
            modelBuilder.Entity<ArticleHistory>()
                .HasIndex(ah => ah.ArticleId);
            modelBuilder.Entity<Report>()
                .HasIndex(r => r.ArticleId);
            modelBuilder.Entity<Warning>()
                .HasIndex(w => w.UserID);
            modelBuilder.Entity<Token>()
                .HasIndex(t => t.TokenValue);
            modelBuilder.Entity<FengShuiAnalysis>()
                .HasOne(a => a.User)
                .WithMany(u => u.FengShuiAnalyses)
                .HasForeignKey(a => a.UserID);
            modelBuilder.Entity<Token>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserID);
            modelBuilder.Entity<Article>()
                .HasOne(a => a.User)
                .WithMany(u => u.Articles)
                .HasForeignKey(a => a.UserID);
            modelBuilder.Entity<ArticleHistory>()
                .HasOne(ah => ah.Article)
                .WithMany(a => a.History)
                .HasForeignKey(ah => ah.ArticleId);
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Article)
                .WithMany(a => a.Reports)
                .HasForeignKey(r => r.ArticleId);
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.ReporterId);
            modelBuilder.Entity<Warning>()
                .HasOne(w => w.User)
                .WithMany(u => u.Warnings)
                .HasForeignKey(w => w.UserID);
        }
    }
}