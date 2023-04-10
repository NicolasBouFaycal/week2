using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NpgsqlTypes;

namespace UMS.Domain;

public partial class Course
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public int? MaxStudentsNumber { get; set; }


    public NpgsqlRange<DateOnly>? EnrolmentDateRange { get; set; }

    public virtual ICollection<TeacherPerCourse> TeacherPerCourses { get; } = new List<TeacherPerCourse>();
}
public class CreateCourse
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
