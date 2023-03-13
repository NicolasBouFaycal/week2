using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using UMS.Application.Commands;
using UMS.Application.Queries;
using UMS.Common.Abstraction;
using UMS.Domain;
using UMS.Infrastructure.EmailServiceAbstraction;

namespace UMS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Student")]
    public class StudentController : ControllerBase
    {
       
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;
        private readonly IUploadImgHelper _uploadImgHelper;


        public StudentController(IUploadImgHelper uploadImgHelper, IEmailService emailService,  IMediator mediator)
        {
            
            _emailService = emailService;
            _mediator = mediator;   
            _uploadImgHelper = uploadImgHelper;
        }
        [HttpGet("GetAllClassesForStudent")]
        [EnableQuery]
        public async Task<ActionResult<List<TeacherPerCourse>>> GetAllClassesForStudent()
        {
            var query = new GetAllClassesForStudent(this);
            var result = await _mediator.Send(query);
            return result;

        }
        [HttpPost(template: "StudentEnrollToCourse")]
        public async Task<ActionResult<ClassEnrollment>> StudentEnrollToCourse([FromQuery] int courseId)
        {
             var result = await _mediator.Send(new StudentEnrollToCourseCommand(this, courseId));
             return result;
           
        }
        [HttpPost(template: "UploadImage")]
        public ActionResult<string> UploadImage([FromForm] UploadImg obj)
        {
            return _uploadImgHelper.UploadProfile(this, obj);
        }
    }
}

