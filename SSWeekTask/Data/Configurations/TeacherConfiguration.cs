using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SSWeekTask.Data.Entities;

namespace SSWeekTask.Data.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<TeacherEntity>
{
    public void Configure(EntityTypeBuilder<TeacherEntity> builder)
    {
        builder.ToTable("Teacher");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName).HasMaxLength(60).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(60).IsRequired();
        builder.Property(e => e.Address).HasMaxLength(150);

        builder.HasMany(e => e.Courses)
            .WithOne(e => e.Teacher)
            .HasForeignKey(e => e.TeacherId);
    }
}
