namespace SSWeekTask.Data.Entities;

public class CourseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid TeacherId { get; set; }
    public ICollection<StudentEntity> Students { get; set; }
    public virtual TeacherEntity Teacher { get; set; }
} 