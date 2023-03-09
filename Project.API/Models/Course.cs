﻿using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Project.API.Models;

public partial class Course
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public int? MaxStudentsNumber { get; set; }

    public NpgsqlRange<DateOnly>? EnrolmentDateRange { get; set; }

    public virtual ICollection<TeacherPerCourse> TeacherPerCourses { get; } = new List<TeacherPerCourse>();
}
