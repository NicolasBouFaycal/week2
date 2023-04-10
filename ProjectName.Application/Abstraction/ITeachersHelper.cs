using UMS.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UMS.Application.Abstraction
{
    public interface ITeachersHelper
    {
        public  List<Course> AllCourses();
        public List<TeacherPerCourse> AllTeacherPerCourse();
        public List<SessionTime> AllSessionTime();
        public TeacherPerCourse TeacherToCourse(int courseId);
        public TeacherPerCoursePerSessionTime TeacherPerCoursePerSessionTime(int TeacherPerCourseId, int SessionTimeId);
        public SessionTime SessionTime(DateTime StartTimeYYYY_MM_DD, DateTime EndTimeYYYY_MM_DD, int Duration);

    }
}
