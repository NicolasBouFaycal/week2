using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Domain.Models;

namespace UMS.Application.Commands
{
    public class AttendanceCommand : IRequest<Attendance>
    {
        public Attendance _attendance;
        public AttendanceCommand(Attendance attendance)
        {
            _attendance = attendance;
        }
    }
    public class AttendanceHandler : IRequestHandler<AttendanceCommand, Attendance>
    {
        private readonly ITeachersHelper _teachersHelper;

        public AttendanceHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;

        }
        public async Task<Attendance> Handle(AttendanceCommand request, CancellationToken cancellationToken)
        {
            //return _adminHelper.Courses(request._name, request._maxStudentsNumber, request._enrolmentDateRange);

            return _teachersHelper.Attendance(request._attendance);
        }
    }
}
