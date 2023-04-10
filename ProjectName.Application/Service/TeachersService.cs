
using Microsoft.AspNetCore.Mvc;
using UMS.Application.Abstraction;
using UMS.Domain;
using UMS.Persistence;
using UMS.Common;
using UMS.Domain.Models;

namespace UMS.Application.Service
{
    public class TeachersService:ITeachersHelper
    {
        public readonly MyDbContext _context;
        public TeachersService(MyDbContext context)
        {
            _context = context;
        }

        public TeacherPerCoursePerSessionTime TeacherPerCoursePerSessionTime( int teacherPerCourseId, int sessionTimeId)
        {
            try
            {
                var userid = Uid.uid;
                TeacherPerCoursePerSessionTime SessionCourse = new TeacherPerCoursePerSessionTime();
                SessionCourse.Id = 2;
                SessionCourse.TeacherPerCourseId = teacherPerCourseId;
                SessionCourse.SessionTimeId = sessionTimeId;

                if (SessionCourse == null)
                    throw new InvalidOperationException("Insert Data");
                _context.Add(SessionCourse);
                _context.SaveChanges();
                return SessionCourse;
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }

        public TeacherPerCourse TeacherToCourse(int courseId)
        {
            try
            {
                var userid = Uid.uid;
                TeacherPerCourse course = new TeacherPerCourse();
                course.Id = 3;
                course.CourseId = courseId;
                course.TeacherId = (from s in _context.Users where (s.KeycloakId == userid) select s.Id).FirstOrDefault();

                if (course == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(course);
                _context.SaveChanges();
                return course;
            }
            catch (Exception ex)
            {
                throw new Exception("connection not fond");

            }
        }

        public List<Course> AllCourses()
        {
            var courses = _context.Courses
            .Where(c => !_context.TeacherPerCourses
                .Any(tpc => tpc.CourseId == c.Id))
            .ToList();

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var courses2 = _context.Courses
                .Where(course => !course.TeacherPerCourses.Any())
                .ToList();

            if (courses2.Count > 0)
            {
                return courses2;
            }
            throw new Exception("NUll");
        }

        public List<SessionTime>AllSessionTime()
        {
            var userid = Uid.uid;
            var getTeacherId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();

            var Scourses = _context.SessionTimes
                .Where(st => !st.TeacherPerCoursePerSessionTimes.Any())
                .ToList();

           /* var Scourses = _context.SessionTimes
            .Where(st => !_context.TeacherPerCoursePerSessionTimes
                .Any(tpcpst => tpcpst.SessionTimeId == st.Id))
            .ToList();*/
            if (Scourses.Count > 0)
            {
                return Scourses;
            }
            throw new Exception("NUll");

        }

        public List<TeacherPerCourse> AllTeacherPerCourse()
        {
            var userid = Uid.uid;
            var getTeacherId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();

            var thcourses = _context.TeacherPerCourses
                .Where(tp =>tp.TeacherId == getTeacherId && !tp.TeacherPerCoursePerSessionTimes.Any())
                .ToList();

            /*var thcourses = _context.TeacherPerCourses
            .Where(tpc => !_context.TeacherPerCoursePerSessionTimes
                .Any(tpcpst => tpcpst.TeacherPerCourseId == tpc.Id && tpc.TeacherId != getTeacherId))
            .ToList();*/

            if (thcourses.Count > 0)
            {
                return thcourses;
            }
            throw new Exception("NUll");
        }

        public SessionTime SessionTime(DateTime StartTimeYYYY_MM_DD,DateTime EndTimeYYYY_MM_DD,int Duration)
        {
            try
            {
                SessionTime tm = new SessionTime();
                tm.StartTime = StartTimeYYYY_MM_DD;
                tm.EndTime = EndTimeYYYY_MM_DD;
                tm.Duration = Duration;
                tm.Id = 6;
                if (tm == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(tm);
                _context.SaveChanges();
                return tm;
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }
    }
}
