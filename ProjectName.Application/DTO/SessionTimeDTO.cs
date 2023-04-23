using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Application.DTO
{
    public class SessionTimeDTO
    {
        public long Id { get; set; }
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
       
    }
}
