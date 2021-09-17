using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class ReportingStructure
    {
        public Employee employee { get; set; }
        public int numberOfReports { get; set; }

        public ReportingStructure(Employee e)
        {
            this.employee = e;
            this.numberOfReports = 0;
        }
    }
}
