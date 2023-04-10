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
    public class AllTeacherPerCourse : IRequest<List<TeacherPerCourse>>
    {
        public AllTeacherPerCourse()
        {
        }

    }
    public class AllTeacherPerCourseHandler : IRequestHandler<AllTeacherPerCourse, List<TeacherPerCourse>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public AllTeacherPerCourseHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<List<TeacherPerCourse>> Handle(AllTeacherPerCourse request, CancellationToken cancellationToken)
        {
            return _teachersHelper.AllTeacherPerCourse();
        }
    }
}
