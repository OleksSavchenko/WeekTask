using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SSWeekTask.Data.Entities;

namespace SSWeekTask.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<StudentEntity>
{
    public void Configure(EntityTypeBuilder<StudentEntity> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName).HasMaxLength(60).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(60).IsRequired();
        builder.Property(e => e.Address).HasMaxLength(150);

        builder.HasMany(e => e.Courses)
            .WithMany(e => e.Students);
    }
}
