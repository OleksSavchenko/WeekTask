using Moq;
using SSWeekTask.Data.Entities;
using SSWeekTask.Data.Repositories.Interfaces;
using SSWeekTask.Models;
using SSWeekTask.Services.Mappers;
using SSWeekTask.Services;
using SSWeekTask.Services.Interfaces;

namespace SSWeekTask.Tests.ServicesTests;

public class TeacherServiceTests
{
    private TeacherService teacherService;
    private Mock<IDataMapper> dataMapperMock;
    private Mock<ITeacherRepository> teacherRepositoryMock;

    [SetUp]
    public void Setup()
    {
        dataMapperMock = new Mock<IDataMapper>();
        teacherRepositoryMock = new Mock<ITeacherRepository>();
        teacherService = new TeacherService(dataMapperMock.Object, teacherRepositoryMock.Object);
    }

    [Test]
    public async Task Create_ValidTeacherDto_ReturnsGuid()
    {
        // Arrange
        var teacherDto = new TeacherDto();
        var expectedGuid = Guid.NewGuid();
        dataMapperMock.Setup(dm => dm.Map<TeacherEntity>(It.IsAny<TeacherDto>()))
                .Returns(new TeacherEntity());

        teacherRepositoryMock.Setup(repo => repo.Create(It.IsAny<TeacherEntity>()))
            .ReturnsAsync(expectedGuid);

        // Act
        var result = await teacherService.Create(teacherDto);

        // Assert
        Assert.AreEqual(expectedGuid, result);
    }

    [Test]
    public void Create_NullTeacherDto_ThrowsArgumentNullException()
    {
        // Arrange
        TeacherDto teacherDto = null;

        // Act and Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await teacherService.Create(teacherDto));
    }

    [Test]
    public async Task Delete_ExistingTeacherId_CallsRepositoryDelete()
    {
        // Arrange
        var teacherId = Guid.NewGuid();
        var teacherEntity = new TeacherEntity { Id = teacherId };
        teacherRepositoryMock.Setup(repo => repo.GetById(teacherId)).ReturnsAsync(teacherEntity);

        // Act
        await teacherService.Delete(teacherId);

        // Assert
        teacherRepositoryMock.Verify(repo => repo.Delete(teacherEntity), Times.Once);
    }

    [Test]
    public void Delete_NonExistentTeacherId_ThrowsInvalidDataException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        teacherRepositoryMock.Setup(repo => repo.GetById(nonExistentId)).ReturnsAsync((TeacherEntity)null);

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await teacherService.Delete(nonExistentId));
    }

    [Test]
    public async Task GetAll_ReturnsListOfTeacherDtos()
    {
        // Arrange
        var teachers = new List<TeacherEntity>
        {
            new TeacherEntity { Id = Guid.NewGuid() },
            new TeacherEntity { Id = Guid.NewGuid() }
        };
        var teacherDtos = teachers.Select(s => new TeacherDto()).ToList();
        teacherRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(teachers);

        int callCount = 0;
        dataMapperMock.Setup(dm => dm.Map<TeacherDto>(It.IsAny<TeacherEntity>()))
            .Returns(() => teacherDtos[callCount++]);

        // Act
        var result = await teacherService.GetAll();

        // Assert
        CollectionAssert.AreEqual(teacherDtos, result);
    }

    [Test]
    public async Task GetById_ExistingTeacherId_ReturnsTeacherDto()
    {
        // Arrange
        var teacherId = Guid.NewGuid();
        var teacherEntity = new TeacherEntity { Id = teacherId };
        var teacherDto = new TeacherDto();
        teacherRepositoryMock.Setup(repo => repo.GetById(teacherId)).ReturnsAsync(teacherEntity);
        dataMapperMock.Setup(dm => dm.Map<TeacherDto>(teacherEntity)).Returns(teacherDto);

        // Act
        var result = await teacherService.GetById(teacherId);

        // Assert
        Assert.AreEqual(teacherDto, result);
    }

    [Test]
    public void GetById_NonExistentTeacherId_ThrowsInvalidDataException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        teacherRepositoryMock.Setup(repo => repo.GetById(nonExistentId)).ReturnsAsync((TeacherEntity)null);

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await teacherService.GetById(nonExistentId));
    }

    [Test]
    public async Task Update_ExistingTeacherDto_ReturnsUpdatedStudentDto()
    {
        // Arrange
        var teacherDto = new TeacherDto { Id = Guid.NewGuid() };
        var teacherEntity = new TeacherEntity();
        teacherRepositoryMock.Setup(repo => repo.GetById(teacherDto.Id)).ReturnsAsync(teacherEntity);
        teacherRepositoryMock.Setup(repo => repo.Update(It.IsAny<TeacherEntity>())).ReturnsAsync(teacherEntity);
        dataMapperMock.Setup(dm => dm.Map<TeacherEntity>(teacherDto)).Returns(teacherEntity);
        dataMapperMock.Setup(dm => dm.Map<TeacherDto>(teacherEntity)).Returns(teacherDto);

        // Act
        var result = await teacherService.Update(teacherDto);

        // Assert
        Assert.AreEqual(teacherDto, result);
    }

    [Test]
    public void Update_NonExistentTeacherId_ThrowsInvalidDataException()
    {
        // Arrange
        var teacherDto = new TeacherDto { Id = Guid.NewGuid() };
        teacherRepositoryMock.Setup(repo => repo.GetById(teacherDto.Id)).ReturnsAsync((TeacherEntity)null);

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await teacherService.Update(teacherDto));
    }


}
