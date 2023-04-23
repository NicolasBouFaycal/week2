using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain;
using UMS.Domain.LinqModels;

namespace UMS.Application.Abstraction
{
    public interface IAdminsHelper
    {
        public Course Courses( string Name, int? MaxStudentsNumber,  int startyear,  int startMonth,  int startDay,  int endyear,  int endMonth,  int endDay);
        //public Course Courses(string Name, int? MaxStudentsNumber, NpgsqlRange<DateOnly>? EnrolmentDateRange);
        public List<AdminAttendanceTracking> AttendanceTracking();

    }
}
