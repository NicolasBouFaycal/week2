using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
}
