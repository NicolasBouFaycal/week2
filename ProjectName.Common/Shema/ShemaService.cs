using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Common.Abstraction;
using UMS.Persistence;

namespace UMS.Common.Shema
{

    public class ShemaService : IShemaHelper
    {
        public readonly MyDbContext _context;

        public ShemaService(MyDbContext context)
        {
            _context = context;
        }
        public string getBranch(string uid)
        {
            var branch=(from u in _context.Users where u.KeycloakId==uid select u.Branch).FirstOrDefault();
            return branch;
        }

        public void setShema(NpgsqlConnection connection, string schema)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            using (var command = new NpgsqlCommand("SET search_path TO '" + schema + "',public;", connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
