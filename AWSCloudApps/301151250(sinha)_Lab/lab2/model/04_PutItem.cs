using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace lab2
{
    public static partial class DdbIntro
    {
        public static async Task PutDynamoTableUsersData(string Username, string Password)
        {
            var queryRequest = RequestBuilder(Username, Password);

            await PutItemAsync(queryRequest);
        }

        private static PutItemRequest RequestBuilder(string Username, string Password)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                {"Username", new AttributeValue {N = Username}},
                {"Password", new AttributeValue {N = Password}}
            };

            return new PutItemRequest
            {
                TableName = "Users",
                Item = item
            };
        }

        private static async Task PutItemAsync(PutItemRequest request)
        {
            await DdbIntro.Client.PutItemAsync(request);
        }
    }
}
