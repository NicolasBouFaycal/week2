using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using UMS.Application.Commands;
using UMS.Application.DTO;
using UMS.Application.Queries;
using UMS.Common.Abstraction;
using UMS.Domain;
using UMS.Domain.LinqModels;
using UMS.Domain.Models;
using UMS.Persistence;

namespace UMS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUploadImgHelper _uploadImgHelper;
        public readonly IShemaHelper _shemaHelper;
        private readonly MyDbContext _context;


        public AdminsController(MyDbContext context,IShemaHelper shemaHelper,IUploadImgHelper uploadImgHelper, IMediator mediator)
        {
            _mediator = mediator;   
            _uploadImgHelper = uploadImgHelper;
            _shemaHelper = shemaHelper;
            _context = context; 

        }
        [HttpGet("AttendanceTracking")]
        public async Task<ActionResult<List<AdminAttendanceTracking>>> AttendanceTracking()
        {
            //var result = await _mediator.Send(new CoursesCommand(course.Name,course.MaxStudentsNumber, course.EnrolmentDateRange));
            var query = new AttendanceTracking();
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpPost("Courses")]
        public async Task<ActionResult<Course>> Courses([FromBody] CreateCourseDTO course)
        {
            var result = await _mediator.Send(new CoursesCommand(course.Name, course.MaxStudentsNumber, course.startyear, course.startMonth, course.startDay, course.endyear, course.endMonth, course.endDay));
            //var result = await _mediator.Send(new CoursesCommand(course.Name,course.MaxStudentsNumber, course.EnrolmentDateRange));

            return result;
        }
        [HttpPost("UploadImage")]
        public ActionResult<string> UploadImage([FromForm] UploadImg obj)
        {
            return _uploadImgHelper.UploadProfileAsync(obj);
        }
        /*[HttpPost(template: "CreateRole")]
        public async Task<ActionResult<Role>> CreateRole([FromQuery] string Name)
        {
            try
            {


                Role r=new Role();
                r.Name = Name;
                r.Id = 3;
                if (r == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(r);
                _context.SaveChanges();
                return Ok(r);
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }*/
        /* [HttpPost(template: "CreateUser")]
         public async Task<ActionResult<Role>> CreateUser([FromQuery] string Name, [FromQuery] string Email, [FromQuery] string FireBaseId)
         {
             try
             {
                 User r = new User();
                 r.Name = Name;
                 r.Email=Email;
                 r.KeycloakId=FireBaseId;
                 r.Id = 5;
                 r.RoleId = 3;
                 if (r == null)
                     throw new InvalidOperationException("INsert Data");
                 _context.Add(r);
                 _context.SaveChanges();
                 return Ok(r);
             }
             catch (Exception ex)
             {
                 throw new Exception("connection not find");

             }
         }*/
    }
}
