﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UMS.Domain;

public partial class User
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long RoleId { get; set; }

    public string KeycloakId { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string?  ImagePath { get; set; }
    public string? Branch { get; set; }

    public virtual ICollection<ClassEnrollment> ClassEnrollments { get; } = new List<ClassEnrollment>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<TeacherPerCourse> TeacherPerCourses { get; } = new List<TeacherPerCourse>();

}
