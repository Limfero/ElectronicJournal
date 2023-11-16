using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicJournal.DAL.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("subject");

            builder.Property(subject => subject.Id)
                .HasColumnName("id_subject")
                .ValueGeneratedOnAdd();

            builder.Property(subject => subject.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(subject => subject.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.HasMany(subject => subject.Teachers)
                .WithMany(teacher => teacher.Subjects)
                .UsingEntity(j => j.ToTable("teacher_has_subject"));
        }
    }
}
