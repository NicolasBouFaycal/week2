using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Domain;

namespace UMS.Application.Commands
{
    public class StudentEnrollToCoursesCommand : IRequest<string>
    {
        public string _userid { get; set; }
        public int _courseId { get; set; }
        public StudentEnrollToCoursesCommand(string userId ,  int courseId)
        {
            _userid = userId;
            _courseId = courseId;   
        }
    }
    public class StudentEnrollToCoursesHandler : IRequestHandler<StudentEnrollToCoursesCommand, string>
    {
        private readonly IStudentsHelper _studentsHelper;

        public StudentEnrollToCoursesHandler(IStudentsHelper studentshelper)
        {
            _studentsHelper = studentshelper;
        }
        public async Task<string> Handle(StudentEnrollToCoursesCommand request, CancellationToken cancellationToken)
        {
            return _studentsHelper.StudentEnrollToCourses(request._userid, request._courseId);
        }
    }
}
