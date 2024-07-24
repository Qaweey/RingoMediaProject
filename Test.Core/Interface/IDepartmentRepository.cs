using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Entities;

namespace Test.Core.Interface
{
    public  interface IDepartmentRepository
    {
        Task<List<Department>> GetListOfDepartment();
        Department GeDepartmentDetails( int id);
    }
}
