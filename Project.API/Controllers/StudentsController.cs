using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using UMS.Application.Commands;
using UMS.Application.Queries;
using UMS.Common.Abstraction;
using UMS.Domain;
using UMS.Domain.Models;
using UMS.Infrastructure.EmailServiceAbstraction;

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


        public StudentsController(IUploadImgHelper uploadImgHelper, IEmailService emailService,  IMediator mediator)
        {
            
            _emailService = emailService;
            _mediator = mediator;   
            _uploadImgHelper = uploadImgHelper;
        }
        [HttpGet("AllClassesForStudent")]
        [EnableQuery]
        public async Task<ActionResult<List<TeacherPerCourse>>> AllClassesForStudent()
        {
            

            var userid = Uid.uid;
            var query = new AllClassesForStudent(userid);
            var result = await _mediator.Send(query);
            return result;

        }

        [HttpPost(template: "StudentEnrollToCourses")]
        public async Task<ActionResult<String>> StudentEnrollToCourses([FromBody] int courseId)
        {
            var userid = Request.Headers["Authentication"];

            var result = await _mediator.Send(new StudentEnrollToCoursesCommand(userid, courseId));
             return result;
           
        }
        [HttpPost(template: "UploadImage")]
        public ActionResult<string> UploadImage([FromForm] UploadImg obj)
        {
            return _uploadImgHelper.UploadProfileAsync(obj);
        }
    }
}

