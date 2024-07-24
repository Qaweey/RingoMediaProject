using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Test.Core.Services;
using Test.Domain.Entities;
using Test.Infrastructure;
using Xunit;

public class DepartmentRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly Mock<ILogger<DepartmentRepository>> _mockLogger;
    private readonly DepartmentRepository _repository;

    public DepartmentRepositoryTests()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _mockLogger = new Mock<ILogger<DepartmentRepository>>();
        _repository = new DepartmentRepository(_mockContext.Object, _mockLogger.Object);
    }

    [Fact]
    public void GeDepartmentDetails_ReturnsDepartment_WhenDepartmentExists()
    {
        // Arrange
        var departmentId = 1;
        var department = new Department
        {
            DepartmentId = departmentId,
            SubDepartments = new List<Department>(),
            ParentDepartment = null
        };

        var departments = new List<Department> { department }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(c => c.Departments).Returns(departments.Object);

        // Act
        var result = _repository.GeDepartmentDetails(departmentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(departmentId, result.DepartmentId);
        _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>(), departmentId), Times.Once);
    }

    [Fact]
    public void GeDepartmentDetails_ReturnsNull_WhenDepartmentDoesNotExist()
    {
        // Arrange
        var departmentId = 1;
        var departments = new List<Department>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(c => c.Departments).Returns(departments.Object);

        // Act
        var result = _repository.GeDepartmentDetails(departmentId);

        // Assert
        Assert.Null(result);
        _mockLogger.Verify(l => l.LogWarning(It.IsAny<string>(), departmentId), Times.Once);
    }

    [Fact]
    public async Task GetListOfDepartment_ReturnsListOfDepartments()
    {
        // Arrange
        var departments = new List<Department>
        {
            new Department { DepartmentId = 1, SubDepartments = new List<Department>() },
            new Department { DepartmentId = 2, SubDepartments = new List<Department>() }
        }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(c => c.Departments).Returns(departments.Object);

        // Act
        var result = await _repository.GetListOfDepartment();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetListOfDepartment_ReturnsNull_WhenExceptionOccurs()
    {
        // Arrange
        _mockContext.Setup(c => c.Departments).Throws(new Exception("Database error"));

        // Act
        var result = await _repository.GetListOfDepartment();

        // Assert
        Assert.Null(result);
        _mockLogger.Verify(l => l.LogError(It.IsAny<string>()), Times.Once);
    }
}

public static class IQueryableExtensions
{
    public static Mock<DbSet<T>> BuildMockDbSet<T>(this IQueryable<T> source) where T : class
    {
        var mockDbSet = new Mock<DbSet<T>>();
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(source.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(source.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(source.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(source.GetEnumerator());
        return mockDbSet;
    }
}
