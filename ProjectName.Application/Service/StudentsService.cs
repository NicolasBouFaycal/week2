using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Common.Abstraction;
using UMS.Domain;
using UMS.Domain.LinqModels;
using UMS.Infrastructure.EmailServiceAbstraction;
using UMS.Persistence;

namespace UMS.Application.Service
{
    public class StudentsService : IStudentsHelper
    {
        public readonly MyDbContext _context;
        private readonly IEmailService _emailService;
        public readonly IShemaHelper _shemaService;

        public StudentsService(IShemaHelper shemaService, MyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            _shemaService = shemaService;
        }


        public List<TeacherPerCourse> AllClassesForStudent(string userId)
        {
            var userid = userId;
            var getStudentId = _context.Users
                                .Where(t => t.KeycloakId == userid)
                                .Select(t => t.Id)
                                .FirstOrDefault();
            var branch = _shemaService.getBranch(userid);

            var conn = _context.Database.GetDbConnection() as NpgsqlConnection;

            _shemaService.setShema(conn, branch);

            var courses2 = _context.TeacherPerCourses
           .Where(course => !course.ClassEnrollments.Any(c => c.StudentId == getStudentId))
           .ToList();
            if (courses2.Count > 0)
            {
                conn.Close();
                return courses2;
            }
            conn.Close();

            throw new Exception("You are Enrolled in All Courses");
        }

        public string StudentEnrollToCourses(string userId, int teacherPerCouseId)
        {
            var branch = _shemaService.getBranch(userId);

            var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
            _shemaService.setShema(conn, branch);
            var classes = _context.TeacherPerCourses.ToList();
            var getUserInfo = _context.Users
                                .Where(u => u.KeycloakId == userId)
                                .FirstOrDefault();
            if (classes.Count == 0)
            {
                throw new Exception("Not Found");
            }
            foreach (var classe in classes)
            {
                if (classe.Id == teacherPerCouseId)
                {

                    ClassEnrollment newclass = new ClassEnrollment();
                    newclass.Id = 4;
                    newclass.ClassId = teacherPerCouseId;
                    newclass.StudentId = _context.Users
                                        .Where(s => s.KeycloakId == userId)
                                        .Select(s => s.Id)
                                        .FirstOrDefault();
                    if (newclass == null)
                        throw new InvalidOperationException("INsert Data");
                    _context.Add(newclass);
                    _context.SaveChanges();

                    var courseId = _context.TeacherPerCourses
                                    .Where(tpc => tpc.Id == teacherPerCouseId)
                                    .Select(tpc => tpc.CourseId)
                                    .FirstOrDefault();

                    var className = _context.Courses
                                    .Where(c => c.Id == courseId)
                                    .Select(c => c.Name)
                                    .FirstOrDefault();

                    var teacherEmail = _context.Users
                                    .Where(us => us.TeacherPerCourses.Any(c => c.Id == teacherPerCouseId)).Select(u => u.Email)
                                    .FirstOrDefault();
                    _emailService.SendEmail(getUserInfo.Email, getUserInfo.Email, "Enrollment to class ", "You have been Suseesfully enrolled in the class :" + className);
                    _emailService.SendEmail(teacherEmail, teacherEmail, "Enrollment to class ", "Student Name :" + getUserInfo.Name + " Has been Enrolled in Class :" + className);

                    conn.Close();
                    return "Inserted succesfully check your email";

                }
            }

            throw new Exception("connection add Class because times up");
        }

        public List<StudentAttendance> AllStudentAttendance()
        {
            var branch = _shemaService.getBranch(Uid.uid);
            var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
            _shemaService.setShema(conn, branch);
            // var attendence = (from a in _context.Attendance select a).ToList();
            var attendance = (from a in _context.Attendance
                              join ce in _context.ClassEnrollments on a.StudentCourseId equals ce.Id
                              join stu in _context.Users on ce.StudentId equals stu.Id
                              join tc in _context.TeacherPerCourses on ce.ClassId equals tc.Id
                              join u in _context.Users on tc.TeacherId equals u.Id
                              join c in _context.Courses on tc.CourseId equals c.Id
                              where stu.KeycloakId == Uid.uid
                              select new StudentAttendance
                              {
                                  courseName = c.Name,
                                  Date = a.Date,
                                  attendance = a.AttendanceStatus,
                                  teacherName = u.Name

                              }).ToList();


            conn.Close();
            return attendance;
        }
    }
}
/* var className= (from tpc in _context.TeacherPerCourses
                                    join c in _context.Courses on tpc.Id equals c.Id
                                    where tpc.Id == teacherPerCouseId
                                        select c.Name).FirstOrDefault();*/

//var className = (from c in _context.Courses where c.Id == courseId select c.Name).FirstOrDefault();

//NpgsqlRange<DateOnly>? range = course.EnrolmentDateRange;
//DateOnly start = range.Value.LowerBound;
// DateOnly end = range.Value.UpperBound;
// DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
//if (start <= currentDate && end >= currentDate || start >= currentDate && end >= currentDate)
//{

/*var classes = _context.TeacherPerCourses
            .Where(c => !_context.ClassEnrollments
                .Any(ce => ce.ClassId == c.Id && ce.StudentId == getStudentId))
            .ToList();*/