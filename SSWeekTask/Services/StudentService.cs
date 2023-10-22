using Serilog;

using SSWeekTask.Data.Entities;
using SSWeekTask.Data.Repositories.Interfaces;
using SSWeekTask.Models;
using SSWeekTask.Services.Interfaces;
using SSWeekTask.Services.Mappers;

namespace SSWeekTask.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IDataMapper _dataMapper;

    public StudentService(IStudentRepository studentRepository, IDataMapper dataMapper)
    {
        _studentRepository = studentRepository;
        _dataMapper = dataMapper;
    }
    public async Task<Guid> Create(StudentDto dto)
    {
        Log.Information("Student creating was started.");

        _ = dto ?? throw new ArgumentNullException(nameof(dto));

        var student = _dataMapper.Map<StudentEntity>(dto);
        student.Id = default;

        var newStudentId = await _studentRepository.Create(student).ConfigureAwait(false);

        Log.Information($"Student with Id = {newStudentId} created successfully.");

        return newStudentId;
    }

    public async Task Delete(Guid id)
    {
        Log.Information($"Deleting Student with Id = {id} started.");

        var student = await _studentRepository.GetById(id).ConfigureAwait(false);

        _ = student ?? throw new ArgumentException($"ID does not exist {id}");

        await _studentRepository.Delete(student);

        Log.Information($"Student with Id = {student.Id} was successfully deleted.");
    }

    public async Task<IEnumerable<StudentDto>> GetAll()
    {
        Log.Information("Getting all Students started.");

        var students = await _studentRepository.GetAll().ConfigureAwait(false);

        Log.Information(!students.Any()
            ? "Students table is empty."
            : $"All {students.Count()} records were successfully received from the Students table.");

        return students.Select(student => _dataMapper.Map<StudentDto>(student)).ToList();

    }

    public async Task<StudentDto> GetById(Guid id)
    {
        Log.Information($"Getting Student by Id started. Looking Id = {id}.");

        var student = await _studentRepository.GetById(id).ConfigureAwait(false);

        _ = student ?? throw new ArgumentException($"ID does not exist {id}");

        Log.Information($"Got a Student with Id = {id}.");

        return _dataMapper.Map<StudentDto>(student);
    }

    public async Task<StudentDto> Update(StudentDto dto)
    {
        _ = dto ?? throw new ArgumentNullException(nameof(dto));

        Log.Information($"Updating Student with Id = {dto.Id} started.");

        var exists = await _studentRepository.GetById(dto.Id).ConfigureAwait(false) != null;

        if (!exists) throw new ArgumentException($"ID does not exist {dto.Id}");

        var student = await _studentRepository.Update(_dataMapper.Map<StudentEntity>(dto));

        Log.Information($"Student with Id = {student.Id} was successfully updated.");

        return _dataMapper.Map<StudentDto>(student);
    }
}
