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
    public class GetAllCourses : IRequest<ActionResult<List<Course>>>
    {
    }
}
