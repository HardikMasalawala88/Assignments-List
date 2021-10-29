using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace lab2
{
    /// <summary>
    /// Interaction logic for BookList.xaml
    /// </summary>
    public partial class BookList : Window
    {
        private MainWindow m_parent;
        public BookList()
        {
            InitializeComponent();
        }
        public BookList(MainWindow parent,string userEmail = "")
        {
            InitializeComponent();
            m_parent = parent;
            lblUserName.Content = userEmail;

            RetriveBookData(userEmail);

            //var response = DdbIntro.GetDynamoTableBooksData(userEmail);
        }

        private async Task RetriveBookData(string userEmail)
        {
            var bookrequest = new GetItemRequest
            {
                TableName = "BookShelf",
                Key = new Dictionary<string, AttributeValue>
                {
                    //{ "BookId", new AttributeValue { N = "1" } },
                    //{ "Authorname", new AttributeValue { S = "Maddie Stigller" } },
                    //{ "Bookmarkpage", new AttributeValue { N = "20" } },
                    //{ "Bookname", new AttributeValue { S = "Beginning Serverless Computing" } },
                    //{ "Bookurl", new AttributeValue { S = "" } },
                    //{ "Islastread", new AttributeValue { BOOL = false } },
                    { "Username", new AttributeValue { N = userEmail } }
                },
                ProjectionExpression = "BookId, Authorname, Bookmarkpage, Bookname, Bookurl, Islastread, Username",
                ConsistentRead = true
            };
            //var response = DBContext.awsDbClient.GetItemAsync(bookrequest).Result;
            try
            {
                var response = DBContext.awsDbClient.GetItemAsync(bookrequest).Result;
            }
            catch (AggregateException ex)
            {

                throw;
            }

            //foreach(var item in response.Item)
            //{
            //    lblAuthorName.Content = item.Value.S;
            //}
        }
    }
}
