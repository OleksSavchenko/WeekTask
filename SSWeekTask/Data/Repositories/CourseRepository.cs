using Microsoft.EntityFrameworkCore;
using SSWeekTask.Data.Entities;
using SSWeekTask.Data.Repositories.Interfaces;

namespace SSWeekTask.Data.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;
    public CourseRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task<Guid> Create(CourseEntity entity)
    {
        var entry = await _context.Courses.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity.Id;
    }

    public async Task Delete(CourseEntity entity)
    {
        _context.Courses.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<CourseEntity>> GetAll()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<CourseEntity> GetById(Guid id)
    {
        return await _context.Courses.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<CourseEntity> Update(CourseEntity entity)
    {
        _context.Courses.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
