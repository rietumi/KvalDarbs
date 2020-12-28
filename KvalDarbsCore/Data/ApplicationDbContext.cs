using LogicCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KvalDarbsCore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Simple entity tables
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Task> Tasks{ get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamTraining> TeamTrainings { get; set; }
        public DbSet<Training> Trainings { get; set; }

        // ManyToMany tables
        public DbSet<UserTeam> UserTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers");
            builder.Entity<Comment>();
            builder.Entity<Competition>();
            builder.Entity<Example>();
            builder.Entity<Exercise>();
            builder.Entity<Goal>();
            builder.Entity<Result>();
            builder.Entity<Task>();
            builder.Entity<Team>();
            builder.Entity<TeamTraining>();
            builder.Entity<Training>();

            // ManyToMany tables
            builder.Entity<UserTeam>().HasKey(ut => new { ut.UserId, ut.TeamId });
        }
    }
}
