using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Domain.LinqModels;

namespace UMS.Application.Queries
{
    public class AttendanceTracking:IRequest<List<AdminAttendanceTracking>>
    {
    }
   
    public class AttendanceTrackingHandler : IRequestHandler<AttendanceTracking, List<AdminAttendanceTracking>>
    {
        private readonly IAdminsHelper _adminsHelper;

        public AttendanceTrackingHandler(IAdminsHelper adminsHelper)
        {
            _adminsHelper = adminsHelper;
        }
        public async Task<List<AdminAttendanceTracking>> Handle(AttendanceTracking request, CancellationToken cancellationToken)
        {
            return _adminsHelper.AttendanceTracking();
        }
    }
}
