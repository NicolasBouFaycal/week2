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
    public class GetAllTeacherPerCourseHandler : IRequestHandler<GetAllTeacherPerCourse, ActionResult<List<TeacherPerCourse>>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public GetAllTeacherPerCourseHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<ActionResult<List<TeacherPerCourse>>> Handle(GetAllTeacherPerCourse request, CancellationToken cancellationToken)
        {
            return _teachersHelper.GetAllTeacherPerCourse(request.Controller);
        }
    }
}
