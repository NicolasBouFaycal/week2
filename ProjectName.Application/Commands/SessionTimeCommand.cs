using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Domain;

namespace UMS.Application.Commands
{
    public class SessionTimeCommand:IRequest<SessionTime>
    {
        public DateTime _StartTimeYYYY_MM_DD { get; set; }
        public DateTime _EndTimeYYYY_MM_DD { get; set; }

        public int _Duration { get; set; }
        public SessionTimeCommand( DateTime StartTimeYYYY_MM_DD,DateTime EndTimeYYYY_MM_DD, int Duration)
        {
            _StartTimeYYYY_MM_DD = StartTimeYYYY_MM_DD;
            _EndTimeYYYY_MM_DD = EndTimeYYYY_MM_DD;
            _Duration=Duration;

        }
    }
    public class SessionTimeHandler : IRequestHandler<SessionTimeCommand, SessionTime>
    {
        private readonly ITeachersHelper _teachersHelper;

        public SessionTimeHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<SessionTime> Handle(SessionTimeCommand request, CancellationToken cancellationToken)
        {
            return _teachersHelper.SessionTime(request._StartTimeYYYY_MM_DD, request._EndTimeYYYY_MM_DD, request._Duration);
        }
    }
}
