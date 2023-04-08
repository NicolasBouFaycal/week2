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
    public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, ActionResult<Course>>
    {
        private readonly IAdminHelper _adminHelper;

        public CreateCourseHandler(IAdminHelper adminHelper)
        {
            _adminHelper = adminHelper;

        }
        public async Task<ActionResult<Course>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            return _adminHelper.CreateCourse(request._controller,request._name,request._maxStudentsNumber ,request._startyear, request._startMonth, request._startDay, request._endyear, request._endMonth, request._endDay);
        }
    }
}
