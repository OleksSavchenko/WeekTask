using System.ComponentModel.DataAnnotations;

namespace SSWeekTask.Models;

public class StudentDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [DataType(DataType.Text)]
    [MaxLength(60)]
    [RegularExpression(@"^([^0-9]*)$", ErrorMessage = "First name cannot contains digits")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [DataType(DataType.Text)]
    [MaxLength(60)]
    [RegularExpression(@"^([^0-9]*)$", ErrorMessage = "Last name cannot contains digits")]
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    [MaxLength(150)]
    public string Address { get; set; } = string.Empty;
}
