﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Domain;

namespace UMS.Application.Queries
{
    public class GetAllCourses : IRequest<ActionResult<List<Course>>>
    {
    }

    public class GetAllCoursesHandler : IRequestHandler<GetAllCourses, ActionResult<List<Course>>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public GetAllCoursesHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<ActionResult<List<Course>>> Handle(GetAllCourses request, CancellationToken cancellationToken)
        {
            return _teachersHelper.GetAllCourses();
        }
    }
}
