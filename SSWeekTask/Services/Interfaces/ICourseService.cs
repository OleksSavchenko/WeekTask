using SSWeekTask.Models;

namespace SSWeekTask.Services.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAll();
    
    Task<CourseDto> GetById(Guid id);
    
    Task<Guid> Create(CourseDto dto);
    
    Task<CourseDto> Update(CourseDto dto);
    
    Task Delete(Guid id);
}