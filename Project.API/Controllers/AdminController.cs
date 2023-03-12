using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using ProjectName.Domain;
using ProjectName.Infrastructure.EmailServiceAbstraction;
using ProjectName.Persistence;

namespace Project.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        public readonly MyDbContext _context;
        private readonly IEmailService _emailService;

        public AdminController(MyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        [HttpPost(template: "SessionTime")]
        [Authorize(Roles = "NICOLAS")]
        public async Task<ActionResult<SessionTime>> InsertTime([FromQuery] DateTime StartTimeYYYY_MM_DD, [FromQuery] DateTime EndTimeYYYY_MM_DD, [FromQuery] int Duration)
        {
            try
            {
                SessionTime tm = new SessionTime();
                tm.StartTime = StartTimeYYYY_MM_DD;
                tm.EndTime = EndTimeYYYY_MM_DD;
                tm.Duration = Duration;
                tm.Id = 4;
                if (tm == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(tm);
                _context.SaveChanges();
                return Ok(tm);
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }
        [HttpPost(template: "CreateCourse")]
        public async Task<ActionResult<Course>> CreateCourse([FromQuery] string Name, [FromQuery] int MaxStudentsNumber, [FromQuery] int startyear, [FromQuery] int startMonth, [FromQuery] int startDay, [FromQuery] int endyear, [FromQuery] int endMonth, [FromQuery] int endDay)
        {
            try
            {
                var startDate = new DateOnly(startyear, startMonth, startDay);
                var endDate = new DateOnly(endyear, endMonth, endDay - 1);
                var range = new NpgsqlRange<DateOnly>(startDate, endDate);

                Course tm = new Course();
                tm.EnrolmentDateRange = range;
                tm.Name = Name;
                tm.MaxStudentsNumber = MaxStudentsNumber;
                tm.Id = 6;
                if (tm == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(tm);
                _context.SaveChanges();
                return Ok(tm);
            }
            catch (Exception ex)
            {
                throw new Exception("change course id");

            }
        }
            [HttpPost(template: "CreateRole")]
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
            }
        [HttpPost(template: "CreateUser")]
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
        }
    }
}
