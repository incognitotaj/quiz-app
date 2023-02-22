using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.ToTable(name: "Quizes");

            builder.HasKey(x => x.Id)
                .HasName("PK_Quizes");

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasColumnType("NVARCHAR(50)");

            builder.Property(x => x.Description)
                .IsRequired(false)
                .HasColumnType("NVARCHAR(500)");

            builder.Property(x => x.CreatedOn)
               .IsRequired()
               .HasColumnType("DATETIME")
               .HasDefaultValueSql("GETDATE()");
        }
    }
}
