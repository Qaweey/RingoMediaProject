using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Core.Interface;
using Test.Domain.Entities;
using Test.Infrastructure;



namespace Test.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        
        private readonly IDepartmentRepository _department;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(IDepartmentRepository department, ILogger<DepartmentsController> logger)
        {
            
            _department = department;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var departments =   _department.GetListOfDepartment();
                _logger.LogInformation("Retrieved list of departments");
                return View(departments);
            }
            catch (Exception ex)
            {

                _logger.LogError($"{ex.Message}");
                return BadRequest();
            }
         
        }

        public   IActionResult Details(int id)
        {
            try
            {
                var department =    _department.GeDepartmentDetails(id);
                if (department == null)
                {
                    _logger.LogWarning("Department with ID {DepartmentId} not found", id);
                    return NotFound();
                }

                var parentDepartments = new List<Department>();
                var currentDepartment = department;
                while (currentDepartment.ParentDepartment != null)
                {
                    parentDepartments.Add(currentDepartment.ParentDepartment);
                    currentDepartment = currentDepartment.ParentDepartment;
                }

                ViewBag.ParentDepartments = parentDepartments;
                _logger.LogInformation("Retrieved details for department with ID {DepartmentId}", id);
                return View(department);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", id);
                return BadRequest();
            }
           
        }
    }
}
