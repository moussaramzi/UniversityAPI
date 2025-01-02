using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.DAL.Models;

namespace University.DAL.Data
{
    public class UniversityContext: DbContext
    {      
            public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

            public DbSet<Course> Courses { get; set; }
            public DbSet<Student> Students { get; set; }
            public DbSet<Instructor> Instructors { get; set; }
            public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany(c => c.EnrolledStudents)
                .WithMany(s => s.EnrolledCourses)
                .UsingEntity(j => j.ToTable("StudentCourses"));

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors)
                .WithMany(i => i.CoursesTaught)
                .UsingEntity(j => j.ToTable("CourseInstructors"));
        }
    }

    
}
