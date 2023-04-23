using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Application.DTO
{
    public class TeacherPerCourseDTO
    {
        public long Id { get; set; }

        public long TeacherId { get; set; }

        public long CourseId { get; set; }
    }
}
