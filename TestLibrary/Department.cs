using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary
{
public class Department
    {
        public string DepartmentName;
        private int employeeCount;
public void AddEmployee(Employee emp)
        {
            employeeCount++;
        }

        private void ReportCount()
        {
            Console.WriteLine($"Total Employees: {employeeCount}");
        }
    }
}
