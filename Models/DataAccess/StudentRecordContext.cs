using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace lab4.DataAccess
{
    public partial class StudentRecordContext : DbContext
    {
        public StudentRecordContext()
        {
        }

        public StudentRecordContext(DbContextOptions<StudentRecordContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AcademicRecord> AcademicRecords { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicRecord>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.CourseCode })
                    .HasName("PK__Academic__3D052599CA81688F");

                entity.ToTable("AcademicRecord");

                entity.Property(e => e.StudentId).HasMaxLength(16);

                entity.Property(e => e.CourseCode).HasMaxLength(16);

                entity.HasOne(d => d.CourseCodeNavigation)
                    .WithMany(p => p.AcademicRecords)
                    .HasForeignKey(d => d.CourseCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AcademicRecord_Course");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.AcademicRecords)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AcademicRecord_Student");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__Course__A25C5AA6502681CB");

                entity.ToTable("Course");

                entity.Property(e => e.Code).HasMaxLength(16);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.FeeBase).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasMany(d => d.StudentStudentNums)
                    .WithMany(p => p.CourseCourses)
                    .UsingEntity<Dictionary<string, object>>(
                        "Registration",
                        l => l.HasOne<Student>()
                            .WithMany()
                            .HasForeignKey("StudentStudentNum")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_Registration_ToStudent"),
                        r => r.HasOne<Course>()
                            .WithMany()
                            .HasForeignKey("CourseCourseId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_Registration_ToCourse"),
                        j =>
                        {
                            j.HasKey("CourseCourseId", "StudentStudentNum")
                                .HasName("PK__Registra__92ECCCE9B9BA1538");

                            j.ToTable("Registration");

                            j.IndexerProperty<string>("CourseCourseId")
                                .HasMaxLength(16)
                                .HasColumnName("Course_CourseID");

                            j.IndexerProperty<string>("StudentStudentNum")
                                .HasMaxLength(16)
                                .HasColumnName("Student_StudentNum");
                        });
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Employees)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeRole",
                        l => l.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ToRole"),
                        r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ToEmployee"),
                        j =>
                        {
                            j.HasKey("EmployeeId", "RoleId");

                            j.ToTable("Employee_Role");

                            j.IndexerProperty<int>("EmployeeId").HasColumnName("Employee_Id");

                            j.IndexerProperty<int>("RoleId").HasColumnName("Role_Id");
                        });
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Role1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.Id).HasMaxLength(16);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
