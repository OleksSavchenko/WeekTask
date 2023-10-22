using SSWeekTask.Models;
using System.ComponentModel.DataAnnotations;

namespace SSWeekTask.Tests.DtoValidation;

public class TeacherDtoTests
{
    [Test]
    public void ValidTeacherDto_PassesValidation()
    {
        // Arrange
        var teacherDto = new TeacherDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Grisha",
            LastName = "Sherlok",
            Address = "Zhytomyrska 12"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(teacherDto, new ValidationContext(teacherDto), validationResults, true);

        // Assert
        Assert.IsTrue(isValid);
        Assert.IsEmpty(validationResults);
    }

    [Test]
    public void InvalidTeacherDto_FailsValidation()
    {
        // Arrange
        var teacherDto = new TeacherDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Grisha88",
            LastName = "Sherlok7",
            Address = "Zhytomyrska 12"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(teacherDto, new ValidationContext(teacherDto), validationResults, true);

        // Assert
        Assert.IsFalse(isValid);
        Assert.IsNotEmpty(validationResults);
    }
}
