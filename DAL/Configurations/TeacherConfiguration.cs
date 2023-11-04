using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicJournal.DAL.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("teacher");

            builder.Property(teacher => teacher.Id)
                .HasColumnName("id_teacher")
                .ValueGeneratedOnAdd();

            builder.Property(teacher => teacher.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(teacher => teacher.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(teacher => teacher.MiddleName)
                .HasColumnName("middle_name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(teacher => teacher.Login)
                .HasColumnName("login")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(teacher => teacher.Password)
                .HasColumnName("password")
                .IsRequired();

            builder.Property(teacher => teacher.Role)
                .HasColumnName("role")
                .IsRequired();

            builder.HasMany(teacher => teacher.Subjects)
                .WithMany(subject => subject.Teachers);
        }
    }
}
