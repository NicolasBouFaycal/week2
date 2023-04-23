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
    public class AllStudentAttendance:IRequest<List<StudentAttendance>>
    {
    }
    public class AllStudentAttendanceHandler : IRequestHandler<AllStudentAttendance, List<StudentAttendance>>
    {
        private readonly IStudentsHelper _studentHelper;

        public AllStudentAttendanceHandler(IStudentsHelper studentHelper)
        {
            _studentHelper = studentHelper;
        }
        public async Task<List<StudentAttendance>> Handle(AllStudentAttendance request, CancellationToken cancellationToken)
        {
            return _studentHelper.AllStudentAttendance();
        }
    }
}
