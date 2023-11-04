using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicJournal.DAL.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("student");

            builder.Property(student => student.Id)
                .HasColumnName("id_student")
                .ValueGeneratedOnAdd();

            builder.Property(student => student.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(student => student.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(student => student.MiddleName)
                .HasColumnName("middle_name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(student => student.Login)
                .HasColumnName("login")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(student => student.Password)
                .HasColumnName("password")
                .IsRequired();

            builder.Property(student => student.IdClass)
                .HasColumnName("id_class");

            builder.HasOne(student => student.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(student => student.IdClass)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
