using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Test.Core.Dtos;
using Test.Core.Interface;
using Test.Core.Services;
using Test.Domain.Entities;
using Test.Infrastructure;
using Xunit;

public class ReminderRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly Mock<ILogger<ReminderRepository>> _mockLogger;
    private readonly ReminderRepository _repository;

    public ReminderRepositoryTests()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _mockLogger = new Mock<ILogger<ReminderRepository>>();
        _repository = new ReminderRepository(_mockContext.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateReminder_SavesReminder_WhenCalled()
    {
        // Arrange
        var reminderDto = new ReminderDto
        {
            ReminderDateTime = DateTime.Now,
            Title = "Test Reminder"
        };

        var reminders = new List<Reminder>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(c => c.Reminders).Returns(reminders.Object);

        // Act
        await _repository.CreateReminder(reminderDto);

        // Assert
        _mockContext.Verify(c => c.Add(It.IsAny<Reminder>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetListOfReminder_ReturnsListOfReminders()
    {
        // Arrange
        var reminders = new List<Reminder>
        {
            new Reminder { ReminderId = 1, Title = "Reminder 1", ReminderDateTime = DateTime.Now },
            new Reminder { ReminderId = 2, Title = "Reminder 2", ReminderDateTime = DateTime.Now }
        }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(c => c.Reminders).Returns(reminders.Object);

        // Act
        var result = _repository.GetListOfReminder();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _mockLogger.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetListOfReminder_ReturnsNull_WhenExceptionOccurs()
    {
        // Arrange
        _mockContext.Setup(c => c.Reminders).Throws(new Exception("Database error"));

        // Act
        var result = _repository.GetListOfReminder();

        // Assert
        Assert.Null(result);
        _mockLogger.Verify(l => l.LogError(It.IsAny<string>()), Times.Once);
    }
}



