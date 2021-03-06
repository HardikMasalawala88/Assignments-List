using System;
using System.Net;
using System.Net.NetworkInformation;
using Amazon.DynamoDBv2;

namespace lab2
{
    public static partial class DdbIntro
    {
        /*-----------------------------------------------------------------------------------
          *  If you are creating a client for the Amazon DynamoDB service, make sure your credentials
          *  are set up first, as explained in:
          *  https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/SettingUp.DynamoWebService.html,
          *
          *  If you are creating a client for DynamoDBLocal (for testing purposes),
          *  DynamoDB-Local should be started first. For most simple testing, you can keep
          *  data in memory only, without writing anything to disk.  To do this, use the
          *  following command line:
          *
          *    java -Djava.library.path=./DynamoDBLocal_lib -jar DynamoDBLocal.jar -inMemory
          *
          *  For information about DynamoDBLocal, see:
          *  https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.html.
          *-----------------------------------------------------------------------------------*/
        // So we know whether local DynamoDB is running
        private static readonly string Ip = "localhost";
        private static readonly int Port = 8000;
        private static readonly string EndpointUrl = "http://" + Ip + ":" + Port;
        private static AmazonDynamoDBClient Client;

        private static bool IsPortInUse()
        {
            bool isAvailable = true;
            // Evaluate current system TCP connections. This is the same information provided
            // by the netstat command line application, just in .Net strongly-typed object
            // form.  We will look through the list, and if our port we would like to use
            // in our TcpClient is occupied, we will set isAvailable to false.
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();
            foreach (IPEndPoint endpoint in tcpConnInfoArray)
            {
                if (endpoint.Port == Port)
                {
                    isAvailable = false;
                    break;
                }
            }

            return isAvailable;
        }

        public static bool createClient(bool useDynamoDbLocal)
        {
            if (useDynamoDbLocal)
            {
                // First, check to see whether anyone is listening on the DynamoDB local port
                // (by default, this is port 8000, so if you are using a different port, modify this accordingly)
                var portUsed = IsPortInUse();
                if (portUsed)
                {
                    Console.WriteLine("The local version of DynamoDB is NOT running.");
                    return (false);
                }

                // DynamoDB-Local is running, so create a client
                Console.WriteLine("  -- Setting up a DynamoDB-Local client (DynamoDB Local seems to be running)");
                AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
                ddbConfig.ServiceURL = EndpointUrl;
                try
                {
                    Client = new AmazonDynamoDBClient(ddbConfig);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("     FAILED to create a DynamoDBLocal client; " + ex.Message);
                    return false;
                }
            }
            else
            {
                //Client = new AmazonDynamoDBClient("AKIA5ZB7NNNJXLPKQ363", "DIMGSVyxHm+y4Q5bvd1TkC4QPtdUHDfsaGqMhKNs", Amazon.RegionEndpoint.USEast1);
                Client = new AmazonDynamoDBClient("AKIA5ZB7NNNJXLPKQ363", "RELDMGXdHAzzxZUS6e3K89HaquXvijn5rIXygKJ0", Amazon.RegionEndpoint.USEast2);

            }

            return true;
        }
    }
}

