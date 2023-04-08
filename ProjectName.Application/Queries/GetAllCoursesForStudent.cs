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
    public class GetAllClassesForStudent : IRequest<List<TeacherPerCourse>>
    {
        public string userid { get; set; }
        public GetAllClassesForStudent(string userid)
        {
            this.userid = userid;
        }
    }
}
