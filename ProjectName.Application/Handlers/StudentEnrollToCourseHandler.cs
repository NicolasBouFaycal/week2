using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Application.Commands;
using UMS.Domain;


namespace UMS.Application.Handlers
{
    public class StudentEnrollToCourseHandler : IRequestHandler<StudentEnrollToCourseCommand, ActionResult<ClassEnrollment>>
    {
        private readonly IStudentsHelper _studentsHelper;

        public StudentEnrollToCourseHandler(IStudentsHelper studentshelper)
        {
            _studentsHelper = studentshelper;
        }
        public async Task<ActionResult<ClassEnrollment>> Handle(StudentEnrollToCourseCommand request, CancellationToken cancellationToken)
        {
            return _studentsHelper.StudentEnrollToCourse(request._controller,request._courseId);
        }
    }
}
