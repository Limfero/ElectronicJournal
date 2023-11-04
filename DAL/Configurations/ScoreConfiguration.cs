using ElectronicJournal.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicJournal.DAL.Configurations
{
    public class ScoreConfiguration : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.ToTable("score");

            builder.Property(score => score.Id)
                .HasColumnName("id_score")
                .ValueGeneratedOnAdd();

            builder.Property(score => score.Grade)
                .HasColumnName("grade")
                .IsRequired();

            builder.Property(score => score.Description)
                .HasColumnName("description")
                .IsRequired(false);


            builder.Property(score => score.IdLesson)
                .HasColumnName("id_lesson");

            builder.Property(score => score.IdStudent)
                .HasColumnName("id_student");

            builder.HasOne(score => score.Lesson)
                .WithMany(lesson => lesson.Scores)
                .HasForeignKey(score => score.IdLesson)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(score => score.Student)
                .WithMany(student => student.Scores)
                .HasForeignKey(score => score.IdStudent)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
