using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain;

namespace UMS.Application.Queries
{
    public class GetAllClassesForStudent : IRequest<ActionResult<List<TeacherPerCourse>>>
    {
        public ControllerBase Controller { get; set; }
        public GetAllClassesForStudent(ControllerBase controller)
        {
            Controller = controller;
        }
    }
}
