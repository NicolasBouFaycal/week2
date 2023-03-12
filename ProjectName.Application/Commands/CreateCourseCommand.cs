using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain;

namespace UMS.Application.Commands
{
    public class CreateCourseCommand : IRequest<ActionResult<Course>>
    {
        public ControllerBase _controller { get; set; }
        public string _name { get; set; }
        public int _maxStudentsNumber { get; set; }
        public int _startyear { get; set; }
        public int _startMonth { get; set; }
        public int _startDay { get; set; }
        public int _endyear { get; set; }
        public int _endMonth { get; set; }
        public int _endDay { get; set; }

        public CreateCourseCommand(ControllerBase controller,string name,int maxStudentsNumber,int startyear,int startMonth, int startDay, int endyear, int endMonth, int endDay)
        {
            _controller = controller;
            _name = name;
            _maxStudentsNumber = maxStudentsNumber;
            _startyear = startyear;
            _startMonth = startMonth;
            _startDay = startDay;
            _endyear = endyear;
            _endMonth = endMonth;
            _endDay = endDay;

        }
    }
}
