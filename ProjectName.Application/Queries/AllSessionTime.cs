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
    public class AllSessionTime : IRequest<List<SessionTime>>
    {
        public AllSessionTime()
        {
        }
    }
    public class AllSessionTimeHandler : IRequestHandler<AllSessionTime, List<SessionTime>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public AllSessionTimeHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<List<SessionTime>> Handle(AllSessionTime request, CancellationToken cancellationToken)
        {
            return _teachersHelper.AllSessionTime();
        }
    }
}
