using Microsoft.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetMembershipDbContext : DbContext
    {
        public AspNetMembershipDbContext()
            : base()
        {

        }

        public AspNetMembershipDbContext(DbContextOptions<AspNetMembershipDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>().ToTable("aspnet_Users", "dbo");
            modelBuilder.Entity<AspNetUser>().HasKey(u => u.UserId);
            modelBuilder.Entity<AspNetUser>().HasOne(u => u.AspNetMembership).WithOne(m => m.AspNetUser).IsRequired();
            modelBuilder.Entity<AspNetUser>().HasOne(u => u.AspNetApplication).WithMany(a => a.AspNetUsers).IsRequired();

            modelBuilder.Entity<AspNetMembership>().ToTable("aspnet_Membership", "dbo");
            modelBuilder.Entity<AspNetMembership>().HasKey(m => m.UserId);
            modelBuilder.Entity<AspNetMembership>().HasOne(m => m.AspNetUser).WithOne(u => u.AspNetMembership).IsRequired();
            modelBuilder.Entity<AspNetMembership>().HasOne(m => m.AspNetApplication).WithMany(a => a.AspNetMemberships).IsRequired();

            modelBuilder.Entity<AspNetApplication>().ToTable("aspnet_Applications", "dbo");
            modelBuilder.Entity<AspNetApplication>().HasKey(a => a.ApplicationId);
            modelBuilder.Entity<AspNetApplication>().HasMany(a => a.AspNetUsers).WithOne(u => u.AspNetApplication).IsRequired();
            modelBuilder.Entity<AspNetApplication>().HasMany(a => a.AspNetMemberships).WithOne(m => m.AspNetApplication).IsRequired();
        }

        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetMembership> AspNetMemberships { get; set; }
        public DbSet<AspNetApplication> AspNetApplications { get; set; }
    }
}
