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
    public class GetAllSessionTime : IRequest<ActionResult<List<SessionTime>>>
    {
        public ControllerBase Controller { get; set; }
        public GetAllSessionTime(ControllerBase controller)
        {
            Controller = controller;
        }
    }
}
