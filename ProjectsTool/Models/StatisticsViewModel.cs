using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectsTool.Models
{
    public class StatisticsViewModel
    {
        public double TotalWorkingHoursAll { get; set; }
        public double AverageHoursPerEmployee { get; set; }

        public List<EmployeePosition> EmployeePositions { get; set; }

        public Nullable<int> ProjectId { get; set; }

        public Nullable<int> EmployeeId { get; set; }
    }
}