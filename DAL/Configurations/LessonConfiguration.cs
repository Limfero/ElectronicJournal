using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicJournal.DAL.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("lesson");

            builder.Property(lesson => lesson.Id)
                .HasColumnName("id_lesson")
                .ValueGeneratedOnAdd();

            builder.Property(lesson => lesson.ClassRoom)
                .HasColumnName("class_room")
                .IsRequired();

            builder.Property(lesson => lesson.Date)
                .HasColumnName("date")
                .IsRequired();

            builder.Property(lesson => lesson.StartTime)
                .HasColumnName("start_time")
                .IsRequired();

            builder.Property(lesson => lesson.EndTime)
                .HasColumnName("end_time")
                .IsRequired();

            builder.Property(lesson => lesson.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(lesson => lesson.IdClass)
                .HasColumnName("id_class");

            builder.Property(lesson => lesson.IdTeacher)
                .HasColumnName("id_teacher");

            builder.Property(lesson => lesson.IdSubject)
                .HasColumnName("id_subject");

            builder.HasOne(lesson => lesson.Class)
                .WithMany(c => c.Lessons)
                .HasForeignKey(lesson => lesson.IdClass)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lesson => lesson.Teacher)
                .WithMany(t => t.Lessons)
                .HasForeignKey(lesson => lesson.IdTeacher)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lesson => lesson.Subject)
                .WithMany(s => s.Lessons)
                .HasForeignKey(lesson => lesson.IdSubject)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
