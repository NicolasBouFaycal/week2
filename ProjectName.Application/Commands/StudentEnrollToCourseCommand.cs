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
    public class StudentEnrollToCourseCommand : IRequest<string>
    {
        public string _userid { get; set; }
        public int _courseId { get; set; }
        public StudentEnrollToCourseCommand(string userId ,  int courseId)
        {
            _userid = userId;
            _courseId = courseId;   
        }
    }
}
