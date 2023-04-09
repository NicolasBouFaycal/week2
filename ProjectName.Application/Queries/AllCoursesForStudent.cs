using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Domain;

namespace UMS.Application.Queries
{
    public class AllClassesForStudent : IRequest<List<TeacherPerCourse>>
    {
        public string userid { get; set; }
        public AllClassesForStudent(string userid)
        {
            this.userid = userid;
        }
    }
    public class AllCoursesForStudentHandler : IRequestHandler<AllClassesForStudent, List<TeacherPerCourse>>
    {
        private readonly IStudentsHelper _studentsHelper;
        public AllCoursesForStudentHandler(IStudentsHelper studentshelper)
        {
            _studentsHelper = studentshelper;

        }
        public async Task<List<TeacherPerCourse>> Handle(AllClassesForStudent request, CancellationToken cancellationToken)
        {

            return _studentsHelper.AllClassesForStudent(request.userid);
        }

    }
}
