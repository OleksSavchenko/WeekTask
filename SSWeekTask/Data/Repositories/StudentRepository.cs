using Microsoft.EntityFrameworkCore;

using SSWeekTask.Data.Entities;
using SSWeekTask.Data.Repositories.Interfaces;

namespace SSWeekTask.Data.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;
    public StudentRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Guid> Create(StudentEntity entity)
    {
        var entry = await _context.Students.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entry.Entity.Id;
    }

    public async Task Delete(StudentEntity entity)
    {
        _context.Students.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<StudentEntity>> GetAll()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<StudentEntity> GetById(Guid id)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<StudentEntity> Update(StudentEntity entity)
    {
        _context.Students.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
