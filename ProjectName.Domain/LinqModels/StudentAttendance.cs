using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Domain.LinqModels
{
    public class StudentAttendance
    {
        public string? courseName { get; set; }
        public DateTime? Date { get; set; }  
        public bool attendance { get; set; }
        public string? teacherName { get; set; }

    }
}
