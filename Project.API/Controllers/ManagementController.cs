using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using ProjectName.Domain;
using ProjectName.Persistence;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        public readonly MyDbContext _context;
        public ManagementController(MyDbContext context)
        {
            _context = context;
        }
        [HttpPost(template: "SessionTime")]
        public async Task<ActionResult<SessionTime>> InsertTime([FromQuery] DateTime StartTime, [FromQuery] DateTime EndTime, [FromQuery] int Duration)
        {
            try
            {
                SessionTime tm = new SessionTime();
                tm.StartTime = StartTime;
                tm.EndTime = EndTime;
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
                tm.Id = 2;
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
        public async Task<ActionResult<Role>> CreateUser([FromQuery] string Name)
        {
            try
            {


                Role r = new Role();
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
    }
}
