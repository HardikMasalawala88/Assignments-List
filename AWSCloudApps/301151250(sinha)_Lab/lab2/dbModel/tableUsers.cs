using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class TableUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class DynamoTableUsers
    {
        public IEnumerable<DynamoTableUsers> dynamoTableUsers { get; set; }
    }
}
