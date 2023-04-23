using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using UMS.Application.Abstraction;
using UMS.Application.Commands;
using UMS.Application.DTO;
using UMS.Application.Queries;
using UMS.Common;
using UMS.Common.Abstraction;
using UMS.Domain;
using UMS.Domain.LinqModels;
using UMS.Domain.Models;
using UMS.Persistence;
namespace UMS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeachersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public static IWebHostEnvironment _environment;
        public readonly MyDbContext _context;
        private readonly IUploadImgHelper _uploadImgHelper ;
        private readonly IMapper _mapper;
        public readonly IShemaHelper _shemaService;


        public TeachersController(IShemaHelper shemaService,IMapper mapper,IUploadImgHelper uploadImgHelper, ITeachersHelper teachersHelper, IMediator mediator, IWebHostEnvironment environment, MyDbContext context)
        {
            _mediator = mediator;
            _environment = environment;
            _context = context;
            _uploadImgHelper = uploadImgHelper;
            _mapper = mapper;
            _shemaService = shemaService;
        }

        [HttpGet("AllClassEnrollment")]
        public async Task<ActionResult<List<ClassEnrollmentDTO>>> AllClassEnrollment()
        {
            var result = await _mediator.Send(new AllClassEnrollments());

            return _mapper.Map<List< ClassEnrollmentDTO >>(result);

        }

        [HttpGet("AllCourses")]
        public async Task<ActionResult<List<CourseDTO>>> AllCourses()
        {
            
            var result = await _mediator.Send(new AllCourses());
            var mappedResult=_mapper.Map<List<CourseDTO>>(result);

            return mappedResult;

        }
        [HttpGet("AllTeacherPerCourse")]
        public async Task<ActionResult<List<TeacherPerCourseDTO>>> AllTeacherPerCourse()
        {
            var result = await _mediator.Send(new AllTeacherPerCourse());
            //_mapper.Map<List<TeacherPerCourseDTO>>(result)
            return _mapper.Map<List<TeacherPerCourseDTO>>(result);

        }
        [HttpGet("AllSessionTime")]
        public async Task<ActionResult<List<SessionTimeDTO>>> AllSessionTime()
        {
            var result = await _mediator.Send(new AllSessionTime());
            return _mapper.Map<List<SessionTimeDTO>>(result);

        }
        [HttpPost("SessionTime")]
        public async Task<ActionResult<SessionTime>> SessionTime([FromBody] SessionTimeDTO sT)
        {
            //using AutoMapper Example from SessionTimeDTO to SessionTime
            SessionTime sessionTime = _mapper.Map<SessionTime>(sT);
            var result = await _mediator.Send(new SessionTimeCommand(sessionTime.StartTime, sessionTime.EndTime, sessionTime.Duration));
            return result;
        }

        [HttpPost("TeacherToCourse")]
        public async Task<ActionResult<TeacherPerCourseDTO>> TeacherToCourse([FromBody] StudentTeacherToCourse tc)
        {
            var result = await _mediator.Send(new TeacherToCourseCommand(tc.courseId));
            return _mapper.Map<TeacherPerCourseDTO>(result);
        }
        [HttpPost(template: "Teacher/CourseToSessionTime")]
        public async Task<ActionResult<TeacherPerCoursePerSessionTimeDTO>> TeacherPerCoursePerSessionTime([FromBody] TeacherPerCoursePerSessionTimeDTO teacherPerCoursePerSessionTime)
        {
            var result = await _mediator.Send(new TeacherPerCoursePerSessionTimeCommand(Convert.ToInt32(teacherPerCoursePerSessionTime.TeacherPerCourseId), Convert.ToInt32(teacherPerCoursePerSessionTime.SessionTimeId)));
            return _mapper.Map<TeacherPerCoursePerSessionTimeDTO>(result);
        }
        [HttpPost("Attendance")]
        public async Task<ActionResult<AttendanceDTO>> Attendance([FromBody] AttendanceDTO at)
        {
            Attendance attendance = _mapper.Map<Attendance>(at);
            var result = await _mediator.Send(new AttendanceCommand(attendance));
            return _mapper.Map<AttendanceDTO>(result);
            
        }
        [HttpPost(template: "UploadImage")]
        public ActionResult<string> UploadImage([FromForm] UploadImg obj)
        {
            return _uploadImgHelper.UploadProfileAsync(obj);
        }
    }
}
