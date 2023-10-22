using SSWeekTask.Models;
using System.ComponentModel.DataAnnotations;

namespace SSWeekTask.Tests.DtoValidation;

public class CourseDtoTests
{
    [Test]
    public void ValidCourseDto_PassesValidation()
    {
        // Arrange
        var courseDto = new CourseDto
        {
            Id = Guid.NewGuid(),
            Name = "Mathematics",
            Description = "Math course for beginners",
            TeacherId = Guid.NewGuid()
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(courseDto, new ValidationContext(courseDto), validationResults, true);

        // Assert
        Assert.IsTrue(isValid);
        Assert.IsEmpty(validationResults);
    }

    [Test]
    public void InvalidCourseDto_FailsValidation()
    {
        // Arrange
        var courseDto = new CourseDto
        {
            Id = Guid.NewGuid(),
            Name = "",
            Description = new string('A', 501),
            TeacherId = Guid.NewGuid()
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(courseDto, new ValidationContext(courseDto), validationResults, true);

        // Assert
        Assert.IsFalse(isValid);
        Assert.IsNotEmpty(validationResults);
    }
}
