using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Application.Commands;
using UMS.Application.Queries;
using UMS.Domain;

namespace UMS.Application.Handlers
{
    public class InserSessionTimeHandler : IRequestHandler<InserSessionTimeCommand, ActionResult<SessionTime>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public InserSessionTimeHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;   
        }
        public async Task<ActionResult<SessionTime>> Handle(InserSessionTimeCommand request, CancellationToken cancellationToken)
        {
            return _teachersHelper.InserSessionTime(request._controller, request._StartTimeYYYY_MM_DD,request._EndTimeYYYY_MM_DD,request._Duration);
        }
    }
}
