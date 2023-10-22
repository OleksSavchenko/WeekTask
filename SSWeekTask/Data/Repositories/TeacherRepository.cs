using Microsoft.EntityFrameworkCore;

using SSWeekTask.Data.Entities;
using SSWeekTask.Data.Repositories.Interfaces;

namespace SSWeekTask.Data.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly ApplicationDbContext _context;
    public TeacherRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task<Guid> Create(TeacherEntity entity)
    {
        var entry = await _context.Teachers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity.Id;
    }

    public async Task Delete(TeacherEntity entity)
    {
        _context.Teachers.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<TeacherEntity>> GetAll()
    {
        return await _context.Teachers.ToListAsync();

    }

    public async Task<TeacherEntity> GetById(Guid id)
    {
        return await _context.Teachers.FirstOrDefaultAsync(s => s.Id == id);

    }

    public async Task<TeacherEntity> Update(TeacherEntity entity)
    {
        _context.Teachers.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
