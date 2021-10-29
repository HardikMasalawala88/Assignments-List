using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows.Documents;
using Table = Amazon.DynamoDBv2.DocumentModel.Table;

namespace lab2
{
    public static partial class DdbIntro
    {
        public static async Task<DynamoTableUsers> GetDynamoTableUsersData()
        {
            var queryRequest = RequestBuilder("Users");

            var result = await ScanAsync(queryRequest);

            return new DynamoTableUsers
            {
                dynamoTableUsers = (IEnumerable<DynamoTableUsers>)result.Items.Select(Map).ToList()
            };
        }

        private static TableUser Map(Dictionary<string, AttributeValue> result)
        {
            return new TableUser
            {
                Username = result["Username"].N,
                Password = result["Password"].N,
            };
        }

        public static async Task<BookShelf> GetDynamoTableBooksData(string userMail)
        {
            BookShelf dynamoTableBook = new BookShelf();
            var queryRequest = RequestBuilder("BookShelf");
            var result = await ScanAsync(queryRequest);
            dynamoTableBook = (BookShelf)result.Items.Select(MapBook).Where(x => x.Username == userMail);

            return dynamoTableBook;
        }

        private static BookShelf MapBook(Dictionary<string, AttributeValue> result)
        {
            return new BookShelf
            {
                Authorname = result["Authorname"].S,
                Bookname = result["Bookname"].S
            };
        }

        private static async Task<ScanResponse> ScanAsync(ScanRequest request)
        {
            var response = await  DdbIntro.Client.ScanAsync(request); 

            return response;
        }

        private static ScanRequest RequestBuilder(string TableName)
        {
                return new ScanRequest
                {
                    TableName = TableName 
                };
            
        }
    }
}
