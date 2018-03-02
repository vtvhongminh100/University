namespace UniversityDao.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UniversityDbContext : DbContext
    {
        public UniversityDbContext()
            : base("name=UniversityDbContext")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<CategoryGroupIdea> CategoryGroupIdeas { get; set; }
        public virtual DbSet<Idea> Ideas { get; set; }
        public virtual DbSet<IdeaCategory> IdeaCategories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Role)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.IdeaEmotion)
                .IsUnicode(false);
        }
    }
}
