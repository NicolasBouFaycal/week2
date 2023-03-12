using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Domain;
using UMS.Persistence;

namespace UMS.Application.Service
{
    public class AdminsHelper : IAdminHelper
    {
        public readonly MyDbContext _context;

        public AdminsHelper(MyDbContext context)
        {
            _context = context;
        }
        public ActionResult<Course> CreateCourse(ControllerBase controllerBase,[FromQuery] string name, [FromQuery] int maxStudentsNumber, [FromQuery] int startyear, [FromQuery] int startMonth, [FromQuery] int startDay, [FromQuery] int endyear, [FromQuery] int endMonth, [FromQuery] int endDay)
        {
            try
            {
                var startDate = new DateOnly(startyear, startMonth, startDay);
                var endDate = new DateOnly(endyear, endMonth, endDay - 1);
                var range = new NpgsqlRange<DateOnly>(startDate, endDate);

                Course tm = new Course();
                tm.EnrolmentDateRange = range;
                tm.Name = name;
                tm.MaxStudentsNumber = maxStudentsNumber;
                tm.Id = 7;
                if (tm == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(tm);
                _context.SaveChanges();
                return controllerBase.Ok(tm);
            }
            catch (Exception ex)
            {
                throw new Exception("change course id");

            }
        }
    }
}
