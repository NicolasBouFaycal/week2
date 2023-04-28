﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Domain.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public long StudentCourseId { get; set; }

        public bool AttendanceStatus { get; set; }

        public DateTime Date { get; set; }

        public virtual ClassEnrollment StudentCourse { get; set; } = null!;

    }
}
