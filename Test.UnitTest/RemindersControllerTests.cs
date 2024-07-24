using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Test.Domain.Entities;
using Test.Infrastructure;
using Test.UnitTest.Utility;
using Test.Web.Controllers;
using Xunit;

public class RemindersControllerTests
{
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly RemindersController _controller;
    private readonly ILogger<RemindersController> _logger;

    public RemindersControllerTests()
    {
        _contextMock = new Mock<ApplicationDbContext>();
        _controller = new RemindersController(_contextMock.Object,_logger);
    }


    [Fact]
    public void Index_ReturnsViewResult_WithListOfReminders()
    {
        var reminders = new List<Reminder>
        {
            new Reminder { ReminderId = 1, Title = "Reminder 1", ReminderDateTime = DateTime.Now.AddDays(1) },
            new Reminder { ReminderId = 2, Title = "Reminder 2", ReminderDateTime = DateTime.Now.AddDays(2) }
        }.AsQueryable();

        var mockSet = DbSetMock.GetMockDbSet(reminders);
        _contextMock.Setup(c => c.Reminders).Returns(mockSet.Object);

        var result = _controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Reminder>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Create_ValidReminder_RedirectsToIndex()
    {
        var reminder = new Reminder { Title = "New Reminder", ReminderDateTime = DateTime.Now.AddDays(1) };

        var result = await _controller.Create(reminder);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        _contextMock.Verify(c => c.Add(It.IsAny<Reminder>()), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
