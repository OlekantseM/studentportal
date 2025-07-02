using Microsoft.EntityFrameworkCore;
using StudentPortal.Models;
using System.Diagnostics;

namespace StudentPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<CourseSubject> CourseSubjects { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public DbSet<Term> Terms { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Mark> Marks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Set precision for decimal to avoid truncation warnings
            modelBuilder.Entity<Grade>()
                .Property(g => g.Score)
                .HasColumnType("decimal(18,2)");
        }
    }
}
