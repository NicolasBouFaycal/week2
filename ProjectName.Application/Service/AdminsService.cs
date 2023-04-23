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
using UMS.Domain.LinqModels;
using UMS.Common.Abstraction;
using UMS.Domain;
using UMS.Persistence;

namespace UMS.Application.Service
{
    public class AdminsService : IAdminsHelper
    {
        public readonly MyDbContext _context;
        public readonly IShemaHelper _shemaService;

        public AdminsService(MyDbContext context, IShemaHelper shemaService)
        {
            _context = context;
            _shemaService = shemaService;
        }

        public List<AdminAttendanceTracking> AttendanceTracking()
        {
            var branch = _shemaService.getBranch(Uid.uid);
            var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
            _shemaService.setShema(conn, branch);
            var attendance = (from a in _context.Attendance
                              join ce in _context.ClassEnrollments on a.StudentCourseId equals ce.Id
                              join stu in _context.Users on ce.StudentId equals stu.Id
                              join tc in _context.TeacherPerCourses on ce.ClassId equals tc.Id
                              join u in _context.Users on tc.TeacherId equals u.Id
                              join c in _context.Courses on tc.CourseId equals c.Id
                              select new AdminAttendanceTracking
                              {
                                  courseName = c.Name,
                                  Date = a.Date,
                                  attendance = a.AttendanceStatus,
                                  teacherName = u.Name,
                                  studentName = stu.Name

                              }).ToList();


            conn.Close();
            return attendance;
        }

        public Course Courses(string name, int? maxStudentsNumber, int startyear, int startMonth, int startDay, int endyear, int endMonth, int endDay)
        {
            try
            {

                var userid = Uid.uid;
                var branch = _shemaService.getBranch(userid);
                var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
                _shemaService.setShema(conn, branch);
                var startDate = new DateOnly(startyear, startMonth, startDay);
                var endDate = new DateOnly(endyear, endMonth, endDay - 1);
                var range = new NpgsqlRange<DateOnly>(startDate, endDate);

                Course tm = new Course();
                tm.EnrolmentDateRange = range;
                tm.Name = name;
                tm.MaxStudentsNumber = maxStudentsNumber;
                tm.Id = 8;
                if (tm == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(tm);
                _context.SaveChanges();
                conn.Close();
                return tm;
            }
            catch (Exception ex)
            {
                throw new Exception("change course id");

            }
        }
    }
}

/*public Course Courses( string name,  int maxStudentsNumber,  int startyear,  int startMonth,  int startDay,  int endyear,  int endMonth,  int endDay)
     {
         try
         {
             var startDate = new DateOnly(startyear, startMonth, startDay);
             var endDate = new DateOnly(endyear, endMonth, endDay - 1);
             var range = new NpgsqlRange<DateOnly>(startDate, endDate);

             Course tm = new Course();
             tm.EnrolmentDateRange = range;
             tm.Name = name;
             tm.MaxStudentsNumber = maxStudentsNumber;
             tm.Id = 7;
             if (tm == null)
                 throw new InvalidOperationException("INsert Data");
             _context.Add(tm);
             _context.SaveChanges();
             return tm;
         }
         catch (Exception ex)
         {
             throw new Exception("change course id");

         }
     }*/