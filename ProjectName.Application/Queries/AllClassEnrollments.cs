using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Application.Abstraction;
using UMS.Domain;

namespace UMS.Application.Queries
{
    public class AllClassEnrollments: IRequest<List<ClassEnrollment>>
    {
    }
    public class AllClassEnrollmentsHandler : IRequestHandler<AllClassEnrollments, List<ClassEnrollment>>
    {
        private readonly ITeachersHelper _teachersHelper;

        public AllClassEnrollmentsHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<List<ClassEnrollment>> Handle(AllClassEnrollments request, CancellationToken cancellationToken)
        {
            return _teachersHelper.AllClassEnrollments();
        }

       
    }
}
