using Microsoft.AspNetCore.Mvc;
using SSWeekTask.Models;
using SSWeekTask.Services.Interfaces;

namespace SSWeekTask.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class StudentController : Controller
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAll();
        return students.Any() ? Ok(students) : NoContent();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var student = await _studentService.GetById(id);
        return student != null ? Ok(student) : NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentDto dto)
    {
        var studentId = await _studentService.Create(dto);
        return Ok(studentId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _studentService.Delete(id);
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] StudentDto dto)
    {
        var student = await _studentService.Update(dto);
        return Ok(student);
    }
}
