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
    public class StudentsHelper: IStudentsHelper
    {
        public readonly MyDbContext _context;
        private readonly IEmailService _emailService;

        public StudentsHelper(MyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;   
        }

        public ActionResult<List<TeacherPerCourse>> GetAllClassesForStudent(ControllerBase controllerBase)
        {
            var userid = controllerBase.Request.Cookies["UserId"];
            var getStudentId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();
            var classes = _context.TeacherPerCourses
            .Where(c => !_context.ClassEnrollments
                .Any(ce => ce.ClassId == c.Id && ce.StudentId == getStudentId))
            .ToList();

            if (classes.Count > 0)
            {
                return classes;
            }
            throw new Exception("NUll");
        }

        public ActionResult<ClassEnrollment> StudentEnrollToCourse(ControllerBase controllerBase,[FromQuery] int courseId)
        {
            var classes = (from c in _context.TeacherPerCourses select c).ToList();
            var userid = controllerBase.Request.Cookies["UserId"];
            var getUserEmail = (from u in _context.Users where u.KeycloakId == userid select u.Email).FirstOrDefault();
            if (classes.Count == 0)
            {
                return controllerBase.NotFound();
            }
            foreach (var classe in classes)
            {
                if (classe.Id == courseId)
                {
                    //NpgsqlRange<DateOnly>? range = course.EnrolmentDateRange;
                    //DateOnly start = range.Value.LowerBound;
                    // DateOnly end = range.Value.UpperBound;
                    // DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
                    //if (start <= currentDate && end >= currentDate || start >= currentDate && end >= currentDate)
                    //{
                    ClassEnrollment newclass = new ClassEnrollment();
                    newclass.Id = 3;
                    newclass.ClassId = courseId;
                    newclass.StudentId = (from s in _context.Users where s.KeycloakId == userid select s.Id).FirstOrDefault();
                    if (newclass == null)
                        throw new InvalidOperationException("INsert Data");
                    _context.Add(newclass);
                    _context.SaveChanges();
                    var className = (from c in _context.Courses where c.Id == courseId select c.Name).FirstOrDefault();
                    _emailService.SendEmail(getUserEmail, getUserEmail, "Enrollment to class ", "You have been Suseesfully enrolled in the class :" + className);

                    return controllerBase.Ok("Inserted succesfully check your email");
                    //};
                }
            }
            throw new Exception("connection add Class because times up");
        }
       
    }
}
