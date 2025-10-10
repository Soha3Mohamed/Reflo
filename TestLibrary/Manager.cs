using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary
{
public class Manager : Employee
    {
        private string level;
public void ApproveLeave()
        {
            Console.WriteLine("Leave approved.");
        }

        private void PlanMeeting()
        {
            Console.WriteLine("Meeting scheduled.");
        }
    }
}
