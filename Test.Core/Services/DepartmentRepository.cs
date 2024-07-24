using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Interface;
using Test.Domain.Entities;
using Test.Infrastructure;

namespace Test.Core.Services
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DepartmentRepository> _logger;
        public DepartmentRepository(ApplicationDbContext context, ILogger<DepartmentRepository> logger)
        {
            _context = context;
            _logger = logger;
            
        }
        public Department GeDepartmentDetails(int id)
        {
            try
            {
                var department = _context.Departments.Include(d => d.SubDepartments).FirstOrDefault(d => d.DepartmentId == id);
                if (department == null)
                {
                    _logger.LogWarning("Department with ID {DepartmentId} not found", id);
                    return null;
                }

                var parentDepartments = new List<Department>();
                var currentDepartment = department;
                while (currentDepartment.ParentDepartment != null)
                {
                    parentDepartments.Add(currentDepartment.ParentDepartment);
                    currentDepartment = currentDepartment.ParentDepartment;
                }

               // ViewBag.ParentDepartments = parentDepartments;
                _logger.LogInformation("Retrieved details for department with ID {DepartmentId}", id);
                return department;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", id);
                return null;
            }


        }

        public async  Task<List<Department>> GetListOfDepartment()
        {
            try
            {
                var departments = _context.Departments.Include(d => d.SubDepartments).ToList();
                _logger.LogInformation("Retrieved list of departments");
                return departments;
            }
            catch (Exception ex)
            {

                _logger.LogError($"{ex.Message}");
                return null;
            }
        }
    }  
}
