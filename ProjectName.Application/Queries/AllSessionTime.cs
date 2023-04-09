using MediatR;
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
    public class AllSessionTime : IRequest<ActionResult<List<SessionTime>>>
    {
        public ControllerBase Controller { get; set; }
        public AllSessionTime(ControllerBase controller)
        {
            Controller = controller;
        }
    }
    public class AllSessionTimeHandler : IRequestHandler<AllSessionTime, ActionResult<List<SessionTime>>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public AllSessionTimeHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<ActionResult<List<SessionTime>>> Handle(AllSessionTime request, CancellationToken cancellationToken)
        {
            return _teachersHelper.AllSessionTime(request.Controller);
        }
    }
}
