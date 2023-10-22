using System.ComponentModel.DataAnnotations;

namespace SSWeekTask.Models;

public class CourseDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Course name is required")]
    [DataType(DataType.Text)]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [DataType(DataType.Text)]
    [MaxLength(500)]
    public string Description { get; set; }

    public Guid TeacherId { get; set; }
}
