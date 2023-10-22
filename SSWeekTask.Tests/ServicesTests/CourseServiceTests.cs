using Moq;

using SSWeekTask.Data.Entities;
using SSWeekTask.Data.Repositories.Interfaces;
using SSWeekTask.Models;
using SSWeekTask.Services;
using SSWeekTask.Services.Mappers;

namespace SSWeekTask.Tests.ServicesTests;
public  class CourseServiceTests
{
    private CourseService courseService;
    private Mock<ICourseRepository> courseRepositoryMock;
    private Mock<IDataMapper> dataMapperMock;
    private Mock<ITeacherRepository> teacherRepositoryMock;

    [SetUp]
    public void Setup()
    {
        courseRepositoryMock = new Mock<ICourseRepository>();
        dataMapperMock = new Mock<IDataMapper>();
        teacherRepositoryMock = new Mock<ITeacherRepository>();
        courseService = new CourseService(courseRepositoryMock.Object, dataMapperMock.Object, teacherRepositoryMock.Object);
    }

    [Test]
    public async Task Create_ValidCourseDto_ReturnsGuid()
    {
        // Arrange
        var teacherId = Guid.NewGuid(); // Creating a valid teacherId

        var courseDto = new CourseDto
        {
            TeacherId = teacherId
        };

        var expectedGuid = Guid.NewGuid();

        // Ensure that dataMapperMock is set up to return a valid CourseEntity
        dataMapperMock.Setup(dm => dm.Map<CourseEntity>(It.IsAny<CourseDto>()))
                .Returns(new CourseEntity());

        // Ensure that teacherRepositoryMock is set up to return a valid teacher entity
        teacherRepositoryMock.Setup(repo => repo.GetById(teacherId))
            .ReturnsAsync(new TeacherEntity { Id = teacherId });

        courseRepositoryMock.Setup(repo => repo.Create(It.IsAny<CourseEntity>()))
            .ReturnsAsync(expectedGuid);

        // Act
        var result = await courseService.Create(courseDto);

        // Assert
        Assert.AreEqual(expectedGuid, result);
    }

    [Test]
    public void Create_NullCourseDto_ThrowsArgumentNullException()
    {
        // Arrange
        CourseDto courseDto = null;

        // Act and Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => courseService.Create(courseDto));
    }

    [Test]
    public async Task Delete_ExistingCourseId_CallsRepositoryDelete()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var courseEntity = new CourseEntity { Id = courseId };
        courseRepositoryMock.Setup(repo => repo.GetById(courseId)).ReturnsAsync(courseEntity);

        // Act
        await courseService.Delete(courseId);

        // Assert
        courseRepositoryMock.Verify(repo => repo.Delete(courseEntity), Times.Once);
    }

    [Test]
    public void Delete_NonExistentCourseId_ThrowsInvalidDataException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        courseRepositoryMock.Setup(repo => repo.GetById(nonExistentId)).ReturnsAsync((CourseEntity)null);

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(() => courseService.Delete(nonExistentId));
    }

    [Test]
    public async Task GetAll_ReturnsListOfCourseDtos()
    {
        // Arrange
        var courses = new List<CourseEntity>
        {
            new CourseEntity { Id = Guid.NewGuid() },
            new CourseEntity { Id = Guid.NewGuid() }
        };
        var courseDtos = courses.Select(c => new CourseDto()).ToList();
        courseRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(courses);

        int callCount = 0;
        dataMapperMock.Setup(dm => dm.Map<CourseDto>(It.IsAny<CourseEntity>()))
            .Returns(() => courseDtos[callCount++]);

        // Act
        var result = await courseService.GetAll();

        // Assert
        CollectionAssert.AreEqual(courseDtos, result);
    }

    [Test]
    public async Task GetById_ExistingCourseId_ReturnsCourseDto()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var courseEntity = new CourseEntity { Id = courseId };
        var courseDto = new CourseDto { Id = courseEntity.Id };
        courseRepositoryMock.Setup(repo => repo.GetById(courseId)).ReturnsAsync(courseEntity);
        dataMapperMock.Setup(dm => dm.Map<CourseDto>(courseEntity)).Returns(courseDto);

        // Act
        var result = await courseService.GetById(courseId);

        // Assert
        Assert.AreEqual(courseDto, result);
    }

    [Test]
    public void GetById_NonExistentCourseId_ThrowsInvalidDataException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        courseRepositoryMock.Setup(repo => repo.GetById(nonExistentId)).ReturnsAsync((CourseEntity)null);

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(() => courseService.GetById(nonExistentId));
    }

    [Test]
    public async Task Update_ExistingCourseDto_ReturnsUpdatedStudentDto()
    {
        // Arrange
        var courseDto = new CourseDto
        {
            Id = Guid.NewGuid(),
            TeacherId = Guid.NewGuid()
        };

        var courseEntity = new CourseEntity();

        courseRepositoryMock.Setup(repo => repo.GetById(courseDto.Id)).ReturnsAsync(courseEntity);
        courseRepositoryMock.Setup(repo => repo.Update(It.IsAny<CourseEntity>())).ReturnsAsync(courseEntity);

        teacherRepositoryMock.Setup(repo => repo.GetById(courseDto.TeacherId)).ReturnsAsync(new TeacherEntity());

        dataMapperMock.Setup(dm => dm.Map<CourseEntity>(courseDto)).Returns(courseEntity);
        dataMapperMock.Setup(dm => dm.Map<CourseDto>(courseEntity)).Returns(courseDto);

        // Act
        var result = await courseService.Update(courseDto);

        // Assert
        Assert.AreEqual(courseDto, result);
    }

    [Test]
    public void Update_NonExistentCourseId_ThrowsInvalidDataException()
    {
        // Arrange
        var courseDto = new CourseDto { Id = Guid.NewGuid() };
        courseRepositoryMock.Setup(repo => repo.GetById(courseDto.Id)).ReturnsAsync((CourseEntity)null);

        // Act and Assert
        Assert.ThrowsAsync<ArgumentException>(() => courseService.Update(courseDto));
    }

    
}
