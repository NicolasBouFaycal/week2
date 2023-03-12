using MediatR;
using Microsoft.AspNetCore.Mvc;
using UMS.Application.Abstraction;
using UMS.Application.Commands;
using UMS.Domain;

namespace UMS.Application.Handlers
{
    public class AssignTeacherToCourseHandler : IRequestHandler<AssignTeacherToCourseCommand, ActionResult<TeacherPerCourse>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public AssignTeacherToCourseHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<ActionResult<TeacherPerCourse>> Handle(AssignTeacherToCourseCommand request, CancellationToken cancellationToken)
        {
            return _teachersHelper.AssignTeacherToCourse(request._controller,request._courseId);
        }
    }
}
