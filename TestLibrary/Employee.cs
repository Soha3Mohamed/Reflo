using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary
{
public class Employee{
        private string _name;
        internal int _age;
        public double _Salary;
internal void DisplayInfo()
        {
            Console.WriteLine($"{_name}, {_age}, {_Salary}");
        }

        internal void CalculateBonus()
        {
            // TODO: Implement bonus logic later
        }
    }
}
