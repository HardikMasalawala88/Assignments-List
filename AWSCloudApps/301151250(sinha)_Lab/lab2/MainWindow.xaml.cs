using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using lab2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Table = Amazon.DynamoDBv2.DocumentModel.Table;

namespace lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
 
        private static string tableUsers= "Users";
        
        BookList bookList;
        public MainWindow()
        {
            InitializeComponent();
            DdbIntro.createClient(false);
            if (!DdbIntro.CheckingTableExistence_async(tableUsers).Result)
            {
                //table not found
                var tableAttribute = new List<AttributeDefinition>(){
                new AttributeDefinition
                    {
                        AttributeName = "Username",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "Password",
                        AttributeType = "N"
                    }
                };
                var tableKeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Username",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Password",
                        KeyType = "RANGE" //Sort key
                    }
                };
                var provisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 6
                };
                if (DdbIntro.CreateTable_async(tableUsers, tableAttribute, tableKeySchema, provisionedThroughput).Result)
                {
                    //Add Record in
                    //Table table = Table.LoadTable(client, tableUsers);

                    var user = new Document();
                    user["Username"] = "20211001";
                    user["Password"] = "20211001";

                    DdbIntro.PutDynamoTableUsersData(user["Username"], user["Password"]);
                    
                    //GET DATA OF TABLE 
                    var tableData = DdbIntro.GetDynamoTableUsersData();
                }
            }
            else {
                //Add Record in
                //Table table = Table.LoadTable(DdbIntro.createClient, tableUsers);

                var user = new Document();
                user["Username"] = "20211002";
                user["Password"] = "20211002";

                DdbIntro.PutDynamoTableUsersData(user["Username"], user["Password"]);
                var tableData = DdbIntro.GetDynamoTableUsersData();
            }

        }

        private async Task btnLogin_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Enter username");
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Enter password");
            }
            else
            {
                MessageBox.Show("System is validating your credentials");
                bool isUserFound = false;
                //CALL AWS DynamoDB for checking data is valid 
                isUserFound = await IsValidUserAsync(txtUsername.Text, txtPassword.Text);
                if (isUserFound)
                {
                    this.Hide();
                    MessageBox.Show("User Found");
                    bookList = new BookList(this, txtUsername.Text);
                    bookList.Show();
                    
                }
                else { MessageBox.Show("User not found"); }

            }

        }

        private async Task<bool> IsValidUserAsync(string username, string password)
        {
            var request = new GetItemRequest
            {
                TableName = "Users",
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Username", new AttributeValue {N = username}},
                    {"Password", new AttributeValue {N = password}}
                },
                ProjectionExpression = "Username, Password",
                ConsistentRead = true
            };
            var response = DBContext.awsDbClient.GetItemAsync(request).Result;
            bool checkresponse = false;
            foreach (var item in response.Item)
            {
                //Match user here
                if (item.Key == "Username" && item.Value.N == username)
                {
                    checkresponse = true;
                }
                else if (item.Key == "Password" && item.Value.N == password)
                {
                    checkresponse = true;
                }
                else
                    checkresponse = false;
            }

            return checkresponse;
            //
            //var credentials = new BasicAWSCredentials("<ACCESS_KEY>", "<SECRET_KEY>");
            //var config = new AmazonDynamoDBConfig()
            //{
            //    RegionEndpoint = RegionEndpoint.USEast1
            //};
            //var client = new AmazonDynamoDBClient(credentials, config);

            //services.AddSingleton<IAmazonDynamoDB>(client);
            //services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
            //
            //AWSCredentials credentials = new AWSCredentials();
            //AmazonDynamoDBClient client = new AmazonDynamoDBClient(,);

            //var request = new QueryRequest
            //{
            //    TableName = "tblCustomer",
            //    ProjectionExpression = "Name, Email"
            //};

            //var response = await client.QueryAsync(request);

            //foreach (Dictionary<string, AttributeValue> item in response.Items)
            //{

            //}
            //IEnumerable<Line> linesWithIdEqualToOne = context.Scan<Line>(new ScanCondition("LineID", ScanOperator.Equal, 1));

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            btnLogin_ClickAsync(sender, e);
        }
    }
}
