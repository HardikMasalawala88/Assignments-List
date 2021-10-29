using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace lab2
{
    public static partial class DdbIntro
    {
        public static async Task<bool> CheckingTableExistence_async(string tblNm)
        {
            string lastTableNameEvaluated = null;
            var request = new ListTablesRequest
            {
                Limit = 2,
                ExclusiveStartTableName = lastTableNameEvaluated
            };
            var response1 = DdbIntro.Client.ListTablesAsync(request).Result;
            return response1.TableNames.Contains(tblNm);
            //Console.WriteLine("\n*** listing tables ***");
            //string lastTableNameEvaluated = null;
            //do
            //{
            //    var request = new ListTablesRequest
            //    {
            //        Limit = 2,
            //        ExclusiveStartTableName = lastTableNameEvaluated
            //    };

            //    var response1 = DdbIntro.Client.ListTablesAsync(request).Result;
            //    foreach (string name in response1.TableNames) { 
            //        Console.WriteLine(name);
            //    }

            //    lastTableNameEvaluated = response1.LastEvaluatedTableName;
            //} while (lastTableNameEvaluated != null);

            //var response = await DdbIntro.Client.ListTablesAsync();
            //return response.TableNames.Contains(tblNm);
        }

        public static async Task<bool> CreateTable_async(string tableName,
            List<AttributeDefinition> tableAttributes,
            List<KeySchemaElement> tableKeySchema,
            ProvisionedThroughput provisionedThroughput)
        {
            bool response = true;

            // Build the 'CreateTableRequest' structure for the new table
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = tableAttributes,
                KeySchema = tableKeySchema,
                // Provisioned-throughput settings are always required,
                // although the local test version of DynamoDB ignores them.
                ProvisionedThroughput = provisionedThroughput
            };

            try
            {
                var responseq = DdbIntro.Client.CreateTableAsync(request).Result;

                var tableDescription = responseq.TableDescription;
                Console.WriteLine("{1}: {0} \t ReadsPerSec: {2} \t WritesPerSec: {3}",
                          tableDescription.TableStatus,
                          tableDescription.TableName,
                          tableDescription.ProvisionedThroughput.ReadCapacityUnits,
                          tableDescription.ProvisionedThroughput.WriteCapacityUnits);

                string status = tableDescription.TableStatus;
                Console.WriteLine(tableName + " - " + status);

                //var makeTbl = await DdbIntro.Client.CreateTableAsync(request);
            }
            catch (Exception ex)
            {
                response = false;
            }

            return response;
        }

        public static async Task<TableDescription> GetTableDescription(string tableName)
        {
            TableDescription result = null;

            // If the table exists, get its description.
            try
            {
                var response = await DdbIntro.Client.DescribeTableAsync(tableName);
                result = response.Table;
            }
            catch (Exception)
            { }

            return result;
        }
    }
}

