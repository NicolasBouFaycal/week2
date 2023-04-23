using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Domain.Models
{
    public class Notes
    {
        public int Id { get; set; }
        public int StudentCourseId { get; set; }
        public double Note { get; set; }
        public DateTime Date { get; set; }
    }
}
