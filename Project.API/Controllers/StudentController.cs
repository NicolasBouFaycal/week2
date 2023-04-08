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
            var userid="";
            if (HttpContext.Items.TryGetValue("userId", out var userId))
            {
                userid = userId.ToString();
            }

            var nicolas = Uid.uid;
            var query = new GetAllClassesForStudent(userid);
            var result = await _mediator.Send(query);
            return result;

        }

        [HttpPost(template: "StudentEnrollToCourse")]
        public async Task<ActionResult<String>> StudentEnrollToCourse([FromBody] int courseId)
        {
            var userid = Request.Headers["Authentication"];

            var result = await _mediator.Send(new StudentEnrollToCourseCommand(userid, courseId));
             return result;
           
        }
        [HttpPost(template: "UploadImage")]
        public ActionResult<string> UploadImage([FromForm] UploadImg obj)
        {
            return _uploadImgHelper.UploadProfile(this, obj);
        }
    }
}

