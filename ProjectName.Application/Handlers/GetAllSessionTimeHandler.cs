using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Application.Queries;
using UMS.Domain;

namespace UMS.Application.Handlers
{
    public class GetAllSessionTimeHandler : IRequestHandler<GetAllSessionTime, ActionResult<List<SessionTime>>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public GetAllSessionTimeHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<ActionResult<List<SessionTime>>> Handle(GetAllSessionTime request, CancellationToken cancellationToken)
        {
            return _teachersHelper.GetAllSessionTime(request.Controller);
        }
    }
}
