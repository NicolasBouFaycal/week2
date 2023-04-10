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
    public class TeacherToCourseCommand : IRequest<TeacherPerCourse>
    {

        public int _courseId  { get; set; }
        public TeacherToCourseCommand( int courseId)
        {
            _courseId=courseId;
        }
    }
    public class TeacherToCourseHandler : IRequestHandler<TeacherToCourseCommand, TeacherPerCourse>
    {
        private readonly ITeachersHelper _teachersHelper;

        public TeacherToCourseHandler(ITeachersHelper teachersHelper)
        {
            _teachersHelper = teachersHelper;
        }
        public async Task<TeacherPerCourse> Handle(TeacherToCourseCommand request, CancellationToken cancellationToken)
        {
            return _teachersHelper.TeacherToCourse( request._courseId);
        }
    }
}
