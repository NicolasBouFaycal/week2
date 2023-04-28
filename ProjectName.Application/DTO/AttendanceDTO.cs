using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Application.DTO
{
    public class AttendanceDTO
    {
        public long StudentCourseId { get; set; }
        public bool AttendanceStatus { get; set; }
        public DateTime Date { get; set; }
    }
}
