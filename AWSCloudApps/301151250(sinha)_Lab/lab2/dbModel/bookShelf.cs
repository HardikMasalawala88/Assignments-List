using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class BookShelf
    {
        public int BookId { get; set; }
        public string Username { get; set; }
        public string Bookname { get; set; }
        public string Authorname { get; set; }
        public bool Islastread { get; set; }
        public string Bookmarkpage { get; set; }
        public string Bookurl { get; set; }
    }
    public class DynamoTableBookShelf
    {
        public IEnumerable<DynamoTableBookShelf> dynamoTableBooks { get; set; }
    }
}
