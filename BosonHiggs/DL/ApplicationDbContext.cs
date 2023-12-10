using BosonHiggsApi.DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace BosonHiggsApi.DL
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<User>().HasKey(x => x.Id);
            builder.Entity<User>().HasIndex(x => x.Token);
            builder.Entity<Level>().HasKey(x => x.Id);
            builder.Entity<Attempt>().HasKey(x => x.Id);

            builder.Entity<UserLevels>().HasKey(x => x.Id);
            builder.Entity<UserLevels>()
                .HasOne(x => x.Level)
                .WithMany(x => x.UserLevels)
                .HasForeignKey(x => x.LevelId);
            builder.Entity<UserLevels>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserLevels)
                .HasForeignKey(x => x.UserId);
            builder.Entity<Message>()
                .HasOne(x => x.User)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.UserId);
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Level> Levels { get; set; } = null!;
        public DbSet<UserLevels> UserLevels { get; set; } = null!;
        public DbSet<Attempt> Attempts { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
    }
}
