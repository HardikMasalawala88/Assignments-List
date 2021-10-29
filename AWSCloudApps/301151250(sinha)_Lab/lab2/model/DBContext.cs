using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public static class DBContext
    {
        public static AmazonDynamoDBClient awsDbClient = new AmazonDynamoDBClient("AKIA5ZB7NNNJXLPKQ363", "RELDMGXdHAzzxZUS6e3K89HaquXvijn5rIXygKJ0", RegionEndpoint.USEast2);
        //private static BasicAWSCredentials credentials;
        //public static DBContext()
        //{
        //    credentials = new BasicAWSCredentials("<ACCESS_KEY>", "<SECRET_KEY>");
        //}

        //var config = new AmazonDynamoDBConfig()
        //{
        //    RegionEndpoint = RegionEndpoint.APSoutheast2
        //};
        //var client = new AmazonDynamoDBClient(credentials, config);
        //services.AddSingleton<IAmazonDynamoDB>(client);
        //services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
    }
}
