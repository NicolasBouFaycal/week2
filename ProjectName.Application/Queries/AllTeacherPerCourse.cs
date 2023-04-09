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
    public class AllTeacherPerCourse : IRequest<ActionResult<List<TeacherPerCourse>>>
    {
        public ControllerBase Controller { get; set; }
        public AllTeacherPerCourse(ControllerBase controller)
        {
            Controller = controller;
        }

    }
    public class AllTeacherPerCourseHandler : IRequestHandler<AllTeacherPerCourse, ActionResult<List<TeacherPerCourse>>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public AllTeacherPerCourseHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<ActionResult<List<TeacherPerCourse>>> Handle(AllTeacherPerCourse request, CancellationToken cancellationToken)
        {
            return _teachersHelper.AllTeacherPerCourse(request.Controller);
        }
    }
}
