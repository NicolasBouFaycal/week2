using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Application.Queries;
using UMS.Domain;

namespace UMS.Application.Handlers
{
    public class GetAllCoursesForStudentHandler:IRequestHandler<GetAllClassesForStudent, List<TeacherPerCourse>>
    {
        private readonly IStudentsHelper _studentsHelper;
        public GetAllCoursesForStudentHandler(IStudentsHelper studentshelper)
        {
            _studentsHelper = studentshelper;

        }
        public async Task<List<TeacherPerCourse>>Handle(GetAllClassesForStudent request, CancellationToken cancellationToken)
        {
            
            return _studentsHelper.GetAllClassesForStudent(request.userid);
        }

    }
}
