using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.DTO;
using UMS.Domain;
using UMS.Domain.Models;

namespace UMS.API.Pfl
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SessionTimeDTO, SessionTime>();
            CreateMap<SessionTime, SessionTimeDTO>();
            CreateMap<Course, CourseDTO>();
            CreateMap<TeacherPerCourse, TeacherPerCourseDTO>();
            CreateMap<TeacherPerCourseDTO, TeacherPerCourse>();
            CreateMap<TeacherPerCoursePerSessionTimeDTO, TeacherPerCoursePerSessionTime>();
            CreateMap<AttendanceDTO, Attendance>();
            CreateMap<Attendance, AttendanceDTO>();
            CreateMap<ClassEnrollmentDTO, ClassEnrollment>();
            CreateMap<ClassEnrollment, ClassEnrollmentDTO>();
        }
    }
}
