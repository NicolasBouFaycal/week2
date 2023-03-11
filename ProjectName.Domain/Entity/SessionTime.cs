﻿using System;
using System.Collections.Generic;

namespace ProjectName.Domain;

public partial class SessionTime
{
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public long Id { get; set; }

    public int Duration { get; set; }

    public virtual ICollection<TeacherPerCoursePerSessionTime> TeacherPerCoursePerSessionTimes { get; } = new List<TeacherPerCoursePerSessionTime>();
}