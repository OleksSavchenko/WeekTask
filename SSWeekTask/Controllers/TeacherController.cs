using Microsoft.AspNetCore.Mvc;
using SSWeekTask.Models;
using SSWeekTask.Services.Interfaces;

namespace SSWeekTask.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TeacherController : Controller
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var teachers = await _teacherService.GetAll();
        return teachers.Any() ? Ok(teachers) : NoContent();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var teacher = await _teacherService.GetById(id);
        return teacher != null ? Ok(teacher) : NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TeacherDto dto)
    {
        var teacherId = await _teacherService.Create(dto);
        return Ok(teacherId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _teacherService.Delete(id);
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TeacherDto dto)
    {
        var teacher = await _teacherService.Update(dto);
        return Ok(teacher);
    }
}