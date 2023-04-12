using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Domain;
using UMS.Infrastructure.EmailServiceAbstraction;
using UMS.Persistence;

namespace UMS.Application.Service
{
    public class StudentsService: IStudentsHelper
    {
        public readonly MyDbContext _context;
        private readonly IEmailService _emailService;

        public StudentsService(MyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;   
        }

        public List<TeacherPerCourse> AllClassesForStudent(string userId)
        {
            var userid = userId;
            var getStudentId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();

            var courses2 = _context.TeacherPerCourses
               .Where(course => !course.ClassEnrollments.Any(c=>c.StudentId==getStudentId))
               .ToList();



            /*var classes = _context.TeacherPerCourses
            .Where(c => !_context.ClassEnrollments
                .Any(ce => ce.ClassId == c.Id && ce.StudentId == getStudentId))
            .ToList();*/

            if (courses2.Count > 0)
            {
                return courses2;
            }
            throw new Exception("You are Enrolled in All Courses");
        }

        public string StudentEnrollToCourses(string userId,int teacherPerCouseId)
        {
            var classes = (from c in _context.TeacherPerCourses select c).ToList();
            var userid = userId;
            var getUserEmail = (from u in _context.Users where u.KeycloakId == userid select u.Email).FirstOrDefault();
            if (classes.Count == 0)
            {
                throw new Exception("Not Found");
            }
            foreach (var classe in classes)
            {
                if (classe.Id == teacherPerCouseId)
                {
                    //NpgsqlRange<DateOnly>? range = course.EnrolmentDateRange;
                    //DateOnly start = range.Value.LowerBound;
                    // DateOnly end = range.Value.UpperBound;
                    // DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
                    //if (start <= currentDate && end >= currentDate || start >= currentDate && end >= currentDate)
                    //{
                    ClassEnrollment newclass = new ClassEnrollment();
                    newclass.Id = 4;
                    newclass.ClassId = teacherPerCouseId;
                    newclass.StudentId = (from s in _context.Users where s.KeycloakId == userid select s.Id).FirstOrDefault();
                    if (newclass == null)
                        throw new InvalidOperationException("INsert Data");
                    _context.Add(newclass);
                    _context.SaveChanges();

                    var courseId = (from tpc in _context.TeacherPerCourses where tpc.Id == teacherPerCouseId select tpc.CourseId).FirstOrDefault();

                    var className = (from c in _context.Courses where c.Id == courseId select c.Name).FirstOrDefault();
                    /* var className= (from tpc in _context.TeacherPerCourses
                                    join c in _context.Courses on tpc.Id equals c.Id
                                    where tpc.Id == teacherPerCouseId
                                     select c.Name).FirstOrDefault();*/

                    //var className = (from c in _context.Courses where c.Id == courseId select c.Name).FirstOrDefault();
                    _emailService.SendEmail(getUserEmail, getUserEmail, "Enrollment to class ", "You have been Suseesfully enrolled in the class :" + className);

                    return "Inserted succesfully check your email";
                    //};
                }
            }
            throw new Exception("connection add Class because times up");
        }
       
    }
}
