namespace SSWeekTask.Data.Entities;

public class StudentEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public virtual ICollection<CourseEntity> Courses { get; set; }
}
