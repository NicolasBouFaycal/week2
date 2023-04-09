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
    public class AdminsService : IAdminsHelper
    {
        public readonly MyDbContext _context;

        public AdminsService(MyDbContext context)
        {
            _context = context;
        }
        /*public Course Courses( string name,  int maxStudentsNumber,  int startyear,  int startMonth,  int startDay,  int endyear,  int endMonth,  int endDay)
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
                return tm;
            }
            catch (Exception ex)
            {
                throw new Exception("change course id");

            }
        }*/
        public Course Courses(string name, int? maxStudentsNumber, NpgsqlRange<DateOnly>? EnrolmentDateRange)
        {
            try
            {
               

                Course tm = new Course();
                tm.EnrolmentDateRange = EnrolmentDateRange;
                tm.Name = name;
                tm.MaxStudentsNumber = maxStudentsNumber;
                tm.Id = 8;
                if (tm == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(tm);
                _context.SaveChanges();
                return tm;
            }
            catch (Exception ex)
            {
                throw new Exception("change course id");

            }
        }
    }
}
