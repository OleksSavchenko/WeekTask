using Microsoft.AspNetCore.Mvc;
using SSWeekTask.Models;
using SSWeekTask.Services.Interfaces;

namespace SSWeekTask.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CourseController : Controller
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _courseService.GetAll();
        return courses.Any() ? Ok(courses) : NoContent();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var course = await _courseService.GetById(id);
        return course != null ? Ok(course) : NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CourseDto dto)
    {
        var courseId = await _courseService.Create(dto);
        return Ok(courseId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _courseService.Delete(id);
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CourseDto dto)
    {
        var course = await _courseService.Update(dto);
        return Ok(course);
    }
}