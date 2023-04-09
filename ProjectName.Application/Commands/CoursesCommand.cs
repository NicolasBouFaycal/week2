using MediatR;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Domain;

namespace UMS.Application.Commands
{
    public class CoursesCommand : IRequest<ActionResult<Course>>
    {
        public string _name { get; set; }
        public int? _maxStudentsNumber { get; set; }
        public  NpgsqlRange<DateOnly>? _enrolmentDateRange { get; set; }
       /* public int _startyear { get; set; }
        public int _startMonth { get; set; }
        public int _startDay { get; set; }
        public int _endyear { get; set; }
        public int _endMonth { get; set; }
        public int _endDay { get; set; }*/

        public CoursesCommand(string name,int? maxStudentsNumber, NpgsqlRange<DateOnly>? enrolmentDateRange)
        {
            
            _name = name;
            _maxStudentsNumber = maxStudentsNumber;
            _enrolmentDateRange = enrolmentDateRange;
           
        }
    }
    public class CreateCourseHandler : IRequestHandler<CoursesCommand, ActionResult<Course>>
    {
        private readonly IAdminsHelper _adminHelper;

        public CreateCourseHandler(IAdminsHelper adminHelper)
        {
            _adminHelper = adminHelper;

        }
        public async Task<ActionResult<Course>> Handle(CoursesCommand request, CancellationToken cancellationToken)
        {
            return _adminHelper.Courses(request._name, request._maxStudentsNumber, request._enrolmentDateRange);

            //return _adminHelper.Courses( request._name, request._maxStudentsNumber, request._startyear, request._startMonth, request._startDay, request._endyear, request._endMonth, request._endDay);
        }
    }
}
