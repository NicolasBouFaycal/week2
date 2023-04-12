using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UMS.Application.Commands;
using UMS.Common.Abstraction;
using UMS.Domain;

namespace UMS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUploadImgHelper _uploadImgHelper;
        public AdminsController(IUploadImgHelper uploadImgHelper, IMediator mediator)
        {
            _mediator = mediator;   
            _uploadImgHelper = uploadImgHelper; 
        }
       
        [HttpPost("Courses")]
        public void Courses([FromBody] Course course)
        {
            string jsonData;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonData = reader.ReadToEnd();
            }
            CreateCourse myObject = JsonConvert.DeserializeObject<CreateCourse>(jsonData);

           // var result = await _mediator.Send(new CoursesCommand(course.Name, course.MaxStudentsNumber, course.startyear, course.startMonth, course.startDay, course.endyear, course.endMonth, course.endDay));
            //var result = await _mediator.Send(new CoursesCommand(course.Name,course.MaxStudentsNumber, course.EnrolmentDateRange));

            //return result;
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
