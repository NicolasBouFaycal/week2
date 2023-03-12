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
    public class AssignTeacherPerCoursePerSessionTimeCommand : IRequest<ActionResult<TeacherPerCoursePerSessionTime>>
    {
        public ControllerBase _controller { get; set; }
        public int _teacherPerCourseId { get; set; }
        public int _sessionTimeId { get; set; }
        

        public AssignTeacherPerCoursePerSessionTimeCommand(ControllerBase controller, [FromQuery] int teacherPerCourseId,[FromQuery] int sessionTimeId)
        {
            _controller = controller;
            _teacherPerCourseId = teacherPerCourseId;
            _sessionTimeId=sessionTimeId;
        }
    }
}
