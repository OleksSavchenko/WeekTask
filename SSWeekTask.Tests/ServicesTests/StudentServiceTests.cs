using Moq;

using SSWeekTask.Data.Entities;
using SSWeekTask.Data.Repositories.Interfaces;
using SSWeekTask.Models;
using SSWeekTask.Services;
using SSWeekTask.Services.Interfaces;
using SSWeekTask.Services.Mappers;

namespace SSWeekTask.Tests.ServicesTests;

public class StudentServiceTests
{
    private Mock<IStudentRepository> studentRepositoryMock;
    private Mock<IDataMapper> dataMapperMock;
    private IStudentService studentService;

    [SetUp]
    public void SetUp()
    {
        studentRepositoryMock = new Mock<IStudentRepository>();
        dataMapperMock = new Mock<IDataMapper>();
        studentService = new StudentService(studentRepositoryMock.Object, dataMapperMock.Object);
    }

    [Test]
    public async Task Create_ValidStudentDto_ReturnsGuid()
    {
        // Arrange
        var studentDto = new StudentDto();
        var expectedGuid = Guid.NewGuid();

        dataMapperMock.Setup(dm => dm.Map<StudentEntity>(It.IsAny<StudentDto>()))
            .Returns(new StudentEntity());

        studentRepositoryMock.Setup(repo => repo.Create(It.IsAny<StudentEntity>()))
            .ReturnsAsync(expectedGuid);

        // Act
        var result = await studentService.Create(studentDto);

        // Assert
        Assert.AreEqual(expectedGuid, result);
    }

    [Test]
    public void Create_NullStudentDto_ThrowsArgumentNullException()
    {
        // Arrange
        StudentDto studentDto = null;

        // Act and Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => studentService.Create(studentDto));
    }

    [Test]
    public async Task Delete_ExistingStudentId_CallsRepositoryDelete()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var studentEntity = new StudentEntity { Id = studentId };
        studentRepositoryMock.Setup(repo => repo.GetById(studentId)).ReturnsAsync(studentEntity);

        // Act
        await studentService.Delete(studentId);

        // Assert
        studentRepositoryMock.Verify(repo => repo.Delete(studentEntity), Times.Once);
    }

    [Test]
    public void Delete_NonExistentStudentId_ThrowsInvalidDataException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        studentRepositoryMock.Setup(repo => repo.GetById(nonExistentId)).ReturnsAsync((StudentEntity)null);

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await studentService.Delete(nonExistentId));
    }

    [Test]
    public async Task GetAll_ReturnsListOfStudentDtos()
    {
        // Arrange
        var students = new List<StudentEntity>
        {
            new StudentEntity { Id = Guid.NewGuid() },
            new StudentEntity { Id = Guid.NewGuid() }
        };
        var studentDtos = students.Select(s => new StudentDto()).ToList();
        studentRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(students);

        int callCount = 0;
        dataMapperMock.Setup(dm => dm.Map<StudentDto>(It.IsAny<StudentEntity>()))
            .Returns(() => studentDtos[callCount++]);

        // Act
        var result = await studentService.GetAll();

        // Assert
        CollectionAssert.AreEqual(studentDtos, result);
    }

    [Test]
    public async Task GetById_ExistingStudentId_ReturnsStudentDto()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var studentEntity = new StudentEntity { Id = studentId };
        var studentDto = new StudentDto();
        studentRepositoryMock.Setup(repo => repo.GetById(studentId)).ReturnsAsync(studentEntity);
        dataMapperMock.Setup(dm => dm.Map<StudentDto>(It.IsAny<StudentEntity>())).Returns(studentDto);

        // Act
        var result = await studentService.GetById(studentId);

        // Assert
        Assert.AreEqual(studentDto, result);
    }

    [Test]
    public void GetById_NonExistentStudentId_ThrowsInvalidDataException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        studentRepositoryMock.Setup(repo => repo.GetById(nonExistentId)).ReturnsAsync((StudentEntity)null);

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await studentService.GetById(nonExistentId));
    }

    [Test]
    public async Task Update_ExistingStudentDto_ReturnsUpdatedStudentDto()
    {
        // Arrange
        var studentDto = new StudentDto { Id = Guid.NewGuid() };
        var studentEntity = new StudentEntity();
        studentRepositoryMock.Setup(repo => repo.GetById(studentDto.Id)).ReturnsAsync(studentEntity);
        studentRepositoryMock.Setup(repo => repo.Update(It.IsAny<StudentEntity>())).ReturnsAsync(studentEntity);
        dataMapperMock.Setup(dm => dm.Map<StudentEntity>(studentDto)).Returns(studentEntity);
        dataMapperMock.Setup(dm => dm.Map<StudentDto>(studentEntity)).Returns(studentDto);

        // Act
        var result = await studentService.Update(studentDto);

        // Assert
        Assert.AreEqual(studentDto, result);
    }

    [Test]
    public void Update_NonExistentStudentId_ThrowsInvalidDataException()
    {
        // Arrange
        var studentDto = new StudentDto { Id = Guid.NewGuid() };
        studentRepositoryMock.Setup(repo => repo.GetById(studentDto.Id)).ReturnsAsync((StudentEntity)null);

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await studentService.Update(studentDto));
    }

}
