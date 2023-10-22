using SSWeekTask.Models;

namespace SSWeekTask.Services.Interfaces;

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetAll();
    
    Task<TeacherDto> GetById(Guid id);
    
    Task<Guid> Create(TeacherDto dto);
    
    Task<TeacherDto> Update(TeacherDto dto);
    
    Task Delete(Guid id);
}