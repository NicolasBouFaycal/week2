using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMS.Common.Abstraction
{
    public interface IShemaHelper
    {
        public string getBranch(string uid);
        public void setShema(NpgsqlConnection connection, string schema);
    }
}
