using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UMS.Application.Commands;
using UMS.Domain;

namespace UMS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController( IMediator mediator)
        {
            _mediator = mediator;   
        }
       
        [HttpPost(template: "CreateCourse")]
        public async Task<ActionResult<Course>> CreateCourse([FromQuery] string name, [FromQuery] int maxStudentsNumber, [FromQuery] int startyear, [FromQuery] int startMonth, [FromQuery] int startDay, [FromQuery] int endyear, [FromQuery] int endMonth, [FromQuery] int endDay)
        {
            var result = await _mediator.Send(new CreateCourseCommand(this,name,maxStudentsNumber,startyear,startMonth,startDay,endyear,endMonth,endDay));
            return result;
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
