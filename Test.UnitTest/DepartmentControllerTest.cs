using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Test.Domain.Entities;
using Test.Infrastructure;
using Test.Web.Controllers;
using Xunit;

public class DepartmentsControllerTests
{
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly DepartmentsController _controller;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsControllerTests()
    {
        _contextMock = new Mock<ApplicationDbContext>();
        _controller = new DepartmentsController(_contextMock.Object,_logger);
    }

    [Fact]
    public void Index_ReturnsViewResult_WithListOfDepartments()
    {

        var departments = new List<Department>
        {
            new Department { DepartmentId = 1, DepartmentName = "HR" },
            new Department { DepartmentId = 2, DepartmentName = "IT" }
        };
        //Act
        _contextMock.Setup(c => c.Departments.Include(It.IsAny<string>())).Returns(departments.AsQueryable());

        var result = _controller.Index();
        //Assert
        var viewResult = Assert.IsType<System.Web.Mvc.ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Department>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count());
    }
}
