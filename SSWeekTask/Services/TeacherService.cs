using Serilog;

using SSWeekTask.Data.Entities;
using SSWeekTask.Data.Repositories.Interfaces;
using SSWeekTask.Models;
using SSWeekTask.Services.Interfaces;
using SSWeekTask.Services.Mappers;

namespace SSWeekTask.Services;

public class TeacherService : ITeacherService
{
    private readonly IDataMapper _dataMapper;
    private readonly ITeacherRepository _teacherRepository;

    public TeacherService(IDataMapper dataMapper, ITeacherRepository teacherRepository)
    {
        _dataMapper = dataMapper;
        _teacherRepository = teacherRepository;
    }
    public async Task<IEnumerable<TeacherDto>> GetAll()
    {
        Log.Information("Getting all Teachers started.");

        var teachers = await _teacherRepository.GetAll().ConfigureAwait(false);

        Log.Information(!teachers.Any()
            ? "Teachers table is empty."
            : $"All {teachers.Count()} records were successfully received from the Teachers table.");

        return teachers.Select(teacher => _dataMapper.Map<TeacherDto>(teacher)).ToList();
    }

    public async Task<TeacherDto> GetById(Guid id)
    {
        Log.Information($"Getting Teacher by Id started. Looking Id = {id}.");

        var teacher = await _teacherRepository.GetById(id).ConfigureAwait(false);

        _ = teacher ?? throw new ArgumentException($"ID does not exist {id}.");

        Log.Information($"Got a Teacher with Id = {id}.");

        return _dataMapper.Map<TeacherDto>(teacher);
    }

    public async Task<Guid> Create(TeacherDto dto)
    {
        Log.Information("Teacher creating was started.");

        _ = dto ?? throw new ArgumentNullException(nameof(dto));

        var teacher = _dataMapper.Map<TeacherEntity>(dto);
        teacher.Id = default;

        var newTeacherId = await _teacherRepository.Create(teacher).ConfigureAwait(false);

        Log.Information($"Teacher with Id = {newTeacherId} created successfully.");

        return newTeacherId;
    }

    public async Task<TeacherDto> Update(TeacherDto dto)
    {
        _ = dto ?? throw new ArgumentNullException(nameof(dto));

        Log.Information($"Updating Teacher with Id = {dto.Id} started.");

        var exists = await _teacherRepository.GetById(dto.Id).ConfigureAwait(false) != null;

        if (!exists) throw new ArgumentException($"ID does not exist {dto.Id}.");

        var teacher = await _teacherRepository.Update(_dataMapper.Map<TeacherEntity>(dto));

        Log.Information($"Teacher with Id = {teacher.Id} was successfully updated.");

        return _dataMapper.Map<TeacherDto>(teacher);
    }

    public async Task Delete(Guid id)
    {
        Log.Information($"Deleting Teacher with Id = {id} started.");

        var teacher = await _teacherRepository.GetById(id).ConfigureAwait(false);

        _ = teacher ?? throw new ArgumentException($"ID does not exist {id}");

        await _teacherRepository.Delete(teacher);

        Log.Information($"Teacher with Id = {teacher.Id} was successfully deleted.");
    }
}