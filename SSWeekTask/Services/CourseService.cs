using Serilog;

using SSWeekTask.Data.Entities;
using SSWeekTask.Data.Repositories.Interfaces;
using SSWeekTask.Models;
using SSWeekTask.Services.Interfaces;
using SSWeekTask.Services.Mappers;

namespace SSWeekTask.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IDataMapper _dataMapper;
    private readonly ITeacherRepository _teacherRepository;

    public CourseService(ICourseRepository courseRepository, IDataMapper dataMapper, ITeacherRepository teacherRepository)
    {
        _courseRepository = courseRepository;
        _dataMapper = dataMapper;
        _teacherRepository = teacherRepository;
    }
    public async Task<IEnumerable<CourseDto>> GetAll()
    {
        Log.Information("Getting all Courses started.");

        var courses = await _courseRepository.GetAll().ConfigureAwait(false);

        Log.Information(!courses.Any()
            ? "Courses table is empty."
            : $"All {courses.Count()} records were successfully received from the Courses table.");

        return courses.Select(course => _dataMapper.Map<CourseDto>(course)).ToList();
    }

    public async Task<CourseDto> GetById(Guid id)
    {
        Log.Information($"Getting Course by Id started. Looking Id = {id}.");

        var course = await _courseRepository.GetById(id).ConfigureAwait(false);

        _ = course ?? throw new ArgumentException($"ID does not exist {id}");

        Log.Information($"Got a Course with Id = {id}.");

        return _dataMapper.Map<CourseDto>(course);
    }

    public async Task<Guid> Create(CourseDto dto)
    {
        Log.Information("Course creating was started.");

        _ = dto ?? throw new ArgumentNullException(nameof(dto));

        _ = await _teacherRepository.GetById(dto.TeacherId).ConfigureAwait(false) ?? throw new ArgumentException($"Invalid teacher Id: {dto.TeacherId}");

        var course = _dataMapper.Map<CourseEntity>(dto);
        course.Id = default;

        var newCourseId = await _courseRepository.Create(course).ConfigureAwait(false);

        Log.Information($"Course with Id = {newCourseId} created successfully.");

        return newCourseId;
    }

    public async Task<CourseDto> Update(CourseDto dto)
    {
        _ = dto ?? throw new ArgumentNullException(nameof(dto));

        Log.Information($"Updating Course with Id = {dto.Id} started.");

        var courseExists = await _courseRepository.GetById(dto.Id).ConfigureAwait(false) != null;

        if (!courseExists) throw new ArgumentException($"Course ID does not exist {dto.Id}");

        var teacherExists = await _teacherRepository.GetById(dto.TeacherId) != null;

        if (!teacherExists) throw new ArgumentException($"Teacher ID does not exist {dto.TeacherId}");

        var course = await _courseRepository.Update(_dataMapper.Map<CourseEntity>(dto));

        Log.Information($"Course with Id = {course.Id} was successfully updated.");

        return _dataMapper.Map<CourseDto>(course);
    }

    public async Task Delete(Guid id)
    {
        Log.Information($"Deleting Course with Id = {id} started.");

        var course = await _courseRepository.GetById(id).ConfigureAwait(false);

        _ = course ?? throw new ArgumentException($"ID does not exist {id}");

        await _courseRepository.Delete(course);

        Log.Information($"Course with Id = {course.Id} was successfully deleted.");
    }
}