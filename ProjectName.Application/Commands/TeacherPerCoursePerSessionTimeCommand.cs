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
    public class TeacherPerCoursePerSessionTimeCommand : IRequest<TeacherPerCoursePerSessionTime>
    {
        public int _teacherPerCourseId { get; set; }
        public int _sessionTimeId { get; set; }
        

        public TeacherPerCoursePerSessionTimeCommand( int teacherPerCourseId,int sessionTimeId)
        {
            _teacherPerCourseId = teacherPerCourseId;
            _sessionTimeId=sessionTimeId;
        }
    }
    public class TeacherPerCoursePerSessionTimeHandler : IRequestHandler<TeacherPerCoursePerSessionTimeCommand, TeacherPerCoursePerSessionTime>
    {
        private readonly ITeachersHelper _teachersHelper;

        public TeacherPerCoursePerSessionTimeHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }


        public async Task<TeacherPerCoursePerSessionTime> Handle(TeacherPerCoursePerSessionTimeCommand request, CancellationToken cancellationToken)
        {
            return _teachersHelper.TeacherPerCoursePerSessionTime( request._teacherPerCourseId, request._sessionTimeId);
        }
    }
}
