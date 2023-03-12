using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UMS.Application.Abstraction;
using UMS.Application.Commands;
using UMS.Application.Queries;
using UMS.Domain;
using UMS.Persistence;

namespace UMS.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeacherController( ITeachersHelper teachersHelper, IMediator mediator)
        {
           
            _mediator = mediator;
            
        }
        [HttpPost(template: "SessionTime")]
        public async Task<ActionResult<SessionTime>> InserSessionTime([FromQuery] DateTime StartTimeYYYY_MM_DD, [FromQuery] DateTime EndTimeYYYY_MM_DD, [FromQuery] int Duration)
        {
            var result = await _mediator.Send(new InserSessionTimeCommand(this, StartTimeYYYY_MM_DD, EndTimeYYYY_MM_DD, Duration));
            return result;
        }
        [HttpGet("GetAllCourses")]
        public async Task<ActionResult<List<Course>>> GetAllCourses()
        {
            var result = await _mediator.Send(new GetAllCourses());
            return result;

        }
        [HttpGet("GetAllTeacherPerCourse")]
        public async Task<ActionResult<List<TeacherPerCourse>>> GetAllTeacherPerCourse()
        {
            var result = await _mediator.Send(new GetAllTeacherPerCourse(this));
            return result;

        }
        [HttpGet("GetAllSessionTime")]
        public async Task<ActionResult<List<SessionTime>>> GetAllSessionTime()
        {
            var result = await _mediator.Send(new GetAllSessionTime(this));
            return result;

        }

        [HttpPost(template: "AssignTeacherToCourse")]
        public async Task<ActionResult<TeacherPerCourse>> AssignTeacherToCourse([FromQuery] int courseId)
        {
            var result = await _mediator.Send(new AssignTeacherToCourseCommand(this, courseId));
            return result;
        }
        [HttpPost(template: "AssignTeacher/CourseToSessionTime")]
        public async Task<ActionResult<TeacherPerCoursePerSessionTime>> AssignTeacherPerCoursePerSessionTime([FromQuery] int teacherPerCourseId, [FromQuery] int sessionTimeId)
        {
            var result = await _mediator.Send(new AssignTeacherPerCoursePerSessionTimeCommand(this, teacherPerCourseId, sessionTimeId));
            return result;
        }
    }
}
