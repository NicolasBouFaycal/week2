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
    public class AssignTeacherToCourseCommand : IRequest<ActionResult<TeacherPerCourse>>
    {
        public ControllerBase _controller { get; set; }

        public int _courseId  { get; set; }
        public AssignTeacherToCourseCommand(ControllerBase controller, [FromQuery] int courseId)
        {
            _controller = controller;
            _courseId=courseId;
        }
    }
    public class AssignTeacherToCourseHandler : IRequestHandler<AssignTeacherToCourseCommand, ActionResult<TeacherPerCourse>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public AssignTeacherToCourseHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<ActionResult<TeacherPerCourse>> Handle(AssignTeacherToCourseCommand request, CancellationToken cancellationToken)
        {
            return _teachersHelper.AssignTeacherToCourse(request._controller, request._courseId);
        }
    }
}
