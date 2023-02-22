using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable(name: "Questions");

            builder.HasKey(x => x.Id)
                .HasName("PK_Question");

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Text)
                .IsRequired()
                .HasColumnType("NVARCHAR(100)");

            builder.Property(x => x.IsMandatory)
                .HasColumnType("BIT");

            builder.Property(x => x.CreatedOn)
               .IsRequired()
               .HasColumnType("DATETIME")
               .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Quiz)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.QuizId)
                .HasConstraintName("FK_Questions_Quizes")
                .IsRequired();
        }
    }
}
