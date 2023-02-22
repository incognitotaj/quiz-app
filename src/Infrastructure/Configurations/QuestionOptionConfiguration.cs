using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Configurations
{
    public class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
    {
        public void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            builder.ToTable(name: "QuestionOptions");

            builder.HasKey(x => x.Id)
                .HasName("PK_QuestionOption");

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Text)
                .IsRequired()
                .HasColumnType("NVARCHAR(100)");

            builder.Property(x => x.IsAnswer)
                .HasColumnType("BIT");

            builder.Property(x => x.CreatedOn)
               .IsRequired()
               .HasColumnType("DATETIME")
               .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Question)
                .WithMany(x => x.QuestionOptions)
                .HasForeignKey(x => x.QuestionId)
                .HasConstraintName("FK_QuestionOptions_Questions")
                .IsRequired();
        }
    }
}
