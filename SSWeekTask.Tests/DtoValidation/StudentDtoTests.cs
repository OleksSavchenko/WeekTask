using System.ComponentModel.DataAnnotations;

using SSWeekTask.Models;

namespace SSWeekTask.Tests.DtoValidation;

public class StudentDtoTests
{
    [Test]
    public void ValidStudentDto_PassesValidation()
    {
        // Arrange
        var studentDto = new StudentDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Grisha",
            LastName = "Sherlok",
            Address = "Kyivska street 1"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(studentDto, new ValidationContext(studentDto), validationResults, true);

        // Assert
        Assert.IsTrue(isValid);
        Assert.IsEmpty(validationResults);
    }

    [Test]
    public void InvalidStudentDtoName_FailsValidation()
    {
        // Arrange
        var studentDto = new StudentDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Grisha11",
            LastName = "Sherlok123",
            Address = "Kyivska street 1"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(studentDto, new ValidationContext(studentDto), validationResults, true);

        // Assert
        Assert.IsFalse(isValid);
        Assert.IsNotEmpty(validationResults);
    }
}
