using Common;
using Microsoft.EntityFrameworkCore;

namespace ApiWithEntityFramework
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<MailLog> MailLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.ToTable("CrudTest");
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<MailLog>(entity =>
            {
                entity.ToTable("MailLog");
                entity.HasKey(x => x.Id);
            });
        }
    }
}