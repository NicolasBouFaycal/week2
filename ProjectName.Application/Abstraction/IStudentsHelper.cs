using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Domain;

namespace UMS.Application.Abstraction
{
    public interface IStudentsHelper
    {
        public string StudentEnrollToCourse(string userId,int courseId);
        public List<TeacherPerCourse> GetAllClassesForStudent(string userId);

    }
}
