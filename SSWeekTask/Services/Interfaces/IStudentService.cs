using SSWeekTask.Models;

namespace SSWeekTask.Services.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAll();
    Task<StudentDto> GetById(Guid id);
    Task<Guid> Create(StudentDto dto);
    Task<StudentDto> Update(StudentDto dto);
    Task Delete(Guid id);
}
