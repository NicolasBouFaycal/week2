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
    public class AssignTeacherPerCoursePerSessionTimeHandler : IRequestHandler<AssignTeacherPerCoursePerSessionTimeCommand, ActionResult<TeacherPerCoursePerSessionTime>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public AssignTeacherPerCoursePerSessionTimeHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        

        public async Task<ActionResult<TeacherPerCoursePerSessionTime>> Handle(AssignTeacherPerCoursePerSessionTimeCommand request, CancellationToken cancellationToken)
        {
            return _teachersHelper.AssignTeacherPerCoursePerSessionTime(request._controller, request._teacherPerCourseId,request._sessionTimeId);
        }
    }
}
