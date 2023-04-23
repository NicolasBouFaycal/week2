using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Application.DTO
{
    public class CreateCourseDTO
    {
        public string Name { get; set; }
        public int? MaxStudentsNumber { get; set; }
        public int startyear { get; set; }

        public int startMonth { get; set; }
        public int startDay { get; set; }
        public int endyear { get; set; }
        public int endMonth { get; set; }
        public int endDay { get; set; }
    }
}
