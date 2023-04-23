using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using UMS.Application;
using UMS.Application.Commands;
using UMS.Application.DTO;
using UMS.Application.Queries;
using UMS.Common.Abstraction;
using UMS.Domain.LinqModels;
using UMS.Infrastructure.EmailServiceAbstraction;
using UMS.Persistence;

namespace UMS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentsController : ControllerBase
    {
       
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;
        private readonly IUploadImgHelper _uploadImgHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<StudentsController> _logger;
        private readonly IMapper _mapper ;
        private readonly MyDbContext _context;
        public readonly IShemaHelper _shemaService;





        public StudentsController(IShemaHelper shemaService,MyDbContext context,IMapper mapper,ILogger<StudentsController> logger, IConfiguration configuration,IUploadImgHelper uploadImgHelper, IEmailService emailService,  IMediator mediator)
        {
            
            _emailService = emailService;
            _mediator = mediator;   
            _uploadImgHelper = uploadImgHelper;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _shemaService = shemaService;
        }
        [HttpGet("AllClassesForStudent")]
        [EnableQuery]
        public async Task<ActionResult<List<TeacherPerCourseDTO>>> AllClassesForStudent()
        {
            _logger.LogInformation("Get All Courses execute");
            var userid = Uid.uid;
            var query = new AllClassesForStudent(userid);
            var result = await _mediator.Send(query);
            return _mapper.Map< List < TeacherPerCourseDTO >>(result);
        }
        [HttpGet("AllStudentAttendance")]
        [EnableQuery]
        public async Task<ActionResult<List<StudentAttendance>>> AllStudentAttendance()
        {
            var query = new AllStudentAttendance();
            var result = await _mediator.Send(query);

            return result;
        }

        [HttpPost("StudentEnrollToCourses")]
        public async Task<ActionResult<String>> StudentEnrollToCourses([FromBody] StudentToCourseDTO Id)
        {
            //var userid = Request.Headers["Authentication"];

            var userid=Uid.uid;
            var result = await _mediator.Send(new StudentEnrollToCoursesCommand(userid, Id.courseId));
             return result;
           
        }
        [HttpPost("UploadImage")]
        public ActionResult<string> UploadImage([FromForm] UploadImg obj)
        {
            return _uploadImgHelper.UploadProfileAsync(obj);
        }
    }
}

