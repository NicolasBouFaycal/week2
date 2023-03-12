using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain;

namespace UMS.Application.Commands
{
    public class InserSessionTimeCommand:IRequest<ActionResult<SessionTime>>
    {
        public ControllerBase _controller { get; set; }
        public DateTime _StartTimeYYYY_MM_DD { get; set; }
        public DateTime _EndTimeYYYY_MM_DD { get; set; }

        public int _Duration { get; set; }
        public InserSessionTimeCommand(ControllerBase controller, [FromQuery] DateTime StartTimeYYYY_MM_DD, [FromQuery] DateTime EndTimeYYYY_MM_DD, [FromQuery] int Duration)
        {
            _controller = controller;
            _StartTimeYYYY_MM_DD = StartTimeYYYY_MM_DD;
            _EndTimeYYYY_MM_DD = EndTimeYYYY_MM_DD;
            _Duration=Duration;

        }
    }
}
