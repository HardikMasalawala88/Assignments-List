using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS_WPF_AWS_S3_IAM.Class
{
    public static class S3Service
    {
        private const string AWS_ACCESS_KEY = "AKIA5ZB7NNNJYQ5VNREW";
        private const string AWS_SECRET_KEY = "q8cKXN4EPXn/KgJBahmZiw6aXZyxhAoBbVB0Zsxf";
        private const string BUCKET_NAME = "ec2studio_sample_bucket";
        private const string S3_KEY = "s3_key";
        public static AmazonS3Client client = new AmazonS3Client();
        
        /// <param name="client"></param>
        public static async Task<PutBucketResponse> CreateBucket(string BucketName)
        {
            var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY,
                     RegionEndpoint.USEast1);

            Console.Out.WriteLine("Checking S3 bucket with name " + BucketName);

            ListBucketsResponse response = client.ListBucketsAsync().Result;

            bool found = false;
            foreach (S3Bucket bucket in response.Buckets)
            {
                if (bucket.BucketName == BucketName)
                {
                    Console.Out.WriteLine("Same bucket found. It will not be recreated.");
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                Console.Out.WriteLine("It is a unique bucket. It will be created.");
                return await client.PutBucketAsync(BucketName);
                Console.Out.WriteLine("Created S3 bucket with name " + BucketName);
            }
            return null;
        }

      
        /// <param name="client"></param>
        public static void WriteS3Object()    // Sample S3 object
        {
            PutObjectRequest request = new PutObjectRequest();

            request.BucketName = BUCKET_NAME;
            request.Key = S3_KEY;
            request.ContentBody = "Body of S3 object.";
 
            Console.Out.WriteLine("Writing S3 object with key " + S3_KEY + " in bucket " + BUCKET_NAME);
            client.PutObjectAsync(request);
        }

        
        /// <param name="client"></param>
        public static void ReadS3Object()   // Reading from S3
        {
            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = BUCKET_NAME;
            request.Key = S3_KEY;

            GetObjectResponse response = client.GetObjectAsync(request).Result;
            StreamReader reader = new StreamReader(response.ResponseStream);
            String content = reader.ReadToEnd();

            Console.Out.WriteLine("Read S3 object with key " + S3_KEY + " in bucket " + BUCKET_NAME + ". Content is: " + content);
        }
       
        /// <param name="client"></param>
        public static void WriteS3ObjectMetadata()  //Writing metadata to S3
        {
            CopyObjectRequest request = new CopyObjectRequest();
            request.DestinationBucket = BUCKET_NAME;
            request.DestinationKey = S3_KEY;
            request.MetadataDirective = S3MetadataDirective.REPLACE;

            request.SourceBucket = BUCKET_NAME;
            request.SourceKey = S3_KEY;

            client.CopyObjectAsync(request);
            Console.Out.WriteLine("Added metadata to S3 object with key " + S3_KEY + " in bucket " + BUCKET_NAME);
        }

        /// <param name="client"></param>
        public static void ReadS3ObjectMetadata()   // Reading S3 object metadata
        {
            GetObjectMetadataRequest request = new GetObjectMetadataRequest();

            request.BucketName = BUCKET_NAME;
            request.Key = S3_KEY;

            GetObjectMetadataResponse response = client.GetObjectMetadataAsync(request).Result;

            Console.Out.WriteLine("Loaded metadata for S3 object with key " + S3_KEY + " in bucket " + BUCKET_NAME);
            foreach (string key in response.ResponseMetadata.Metadata.Keys)
            {
                Console.Out.WriteLine("   key: " + key + ", value: " + response.Metadata[key]);
            }
        }


        /// <param name="client"></param>
        public static void GetS3ObjectAccessList()  // Reading ACL for S3 object
        {
            GetACLResponse response = client.GetACLAsync(new GetACLRequest() { BucketName = BUCKET_NAME, Key = S3_KEY }).Result;

            Console.Out.WriteLine("Object owner is " + response.AccessControlList.Owner.DisplayName);
        }

        /// <param name="client"></param>
        public static void GenerateAccessURL()
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
            request.BucketName = BUCKET_NAME;
            request.Key = S3_KEY;

            request.Expires = DateTime.Now.Add(new TimeSpan(7, 0, 0, 0));

            Console.Out.WriteLine("Generated signed URL that will be valid for a week for object: " + client.GetPreSignedURL(request));
        }
    
        /// <param name="client"></param>
        public static void GetBucketLoggingRequest()  //Checking if logging is enabled
        {
            GetBucketLoggingResponse response = client.GetBucketLoggingAsync(new GetBucketLoggingRequest() { BucketName = BUCKET_NAME }).Result;

            bool loggingEnabled = string.IsNullOrEmpty(response.BucketLoggingConfig.TargetBucketName) == false;

            Console.Out.WriteLine("Bucket logging is " + loggingEnabled);
        }

        /// <param name="client"></param>
        public static void GetObjectVersions()
        {
            ListVersionsResponse response = client.ListVersionsAsync(new ListVersionsRequest() { BucketName = BUCKET_NAME, Prefix = S3_KEY }).Result;
            Console.Out.WriteLine("Found the following versions for prefix " + S3_KEY);

            foreach (S3ObjectVersion version in response.Versions)
            {
                Console.Out.WriteLine("   version id: " + version.VersionId + ", last modified time: " + version.LastModified);
            }
        }

    
        /// <param name="client"></param>
        public static void GetDirectoryListing()    // Directory Listing
        {
            String DIR_NAME = "folder_sample";
            String newKey = DIR_NAME + "/" + S3_KEY;
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = BUCKET_NAME;
            request.Key = newKey;
            request.ContentBody = "This is body of another S3 object.";
            client.PutObjectAsync(request);
            Console.Out.WriteLine("Created new S3 Object with key: " + newKey);

            ListObjectsRequest req = new ListObjectsRequest();
            req.BucketName = BUCKET_NAME;
            req.Prefix = DIR_NAME;

            ListObjectsResponse res = client.ListObjectsAsync(req).Result;

            Console.Out.WriteLine("Enumerating all objects in directory: " + DIR_NAME);
            foreach (S3Object obj in res.S3Objects)
            {
                Console.Out.WriteLine("   S3 object key: " + obj.Key);
            }
        }


        /// <param name=""></param>
        public static async Task<ListBucketsResponse> GetBucketListing()   // Listing the bucket list
        {
            var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY,
                    RegionEndpoint.USEast1);
            return client.ListBucketsAsync().Result;
        }
        public static async Task<List<BucketList>> transformBucketListFromS3()
        {


            List<BucketList> bucketLists = new List<BucketList>();

            var BucketListing = await S3Service.GetBucketListing();
            for (int i = 0; i < BucketListing.Buckets.Count; i++)
            {
                var bucket = BucketListing.Buckets[i];
                bucketLists.Add(new BucketList
                {
                    BucketName = bucket.BucketName,
                    CreationDate = bucket.CreationDate
                });
            }

            return bucketLists;
        }
        public static async Task<ListObjectsV2Response> ListAllItemOfBucketAsync(string bucketName)
        {
            var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY,
                    RegionEndpoint.USEast1);
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = bucketName,
            };
            ListObjectsV2Response response;
            do
            {
                response = await client.ListObjectsV2Async(request);

                // Process the response.
                foreach (S3Object entry in response.S3Objects)
                {
                    Console.WriteLine("key = {0} size = {1}",
                        entry.Key, entry.Size);
                }
                Console.WriteLine("Next Continuation Token: {0}", response.NextContinuationToken);
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);
            return response;
        }
        public static bool sendMyFileToS3(string localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3)
        {
          
            var client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY,
                   RegionEndpoint.USEast1);

            TransferUtility utility = new TransferUtility(client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            if (subDirectoryInBucket == "" || subDirectoryInBucket == null)
            {
                request.BucketName = bucketName; 
            }
            else
            {   // subdirectory and bucket name
                request.BucketName = bucketName + @"/" + subDirectoryInBucket;
            }
            request.Key = fileNameInS3; //file name up in S3
            request.FilePath = localFilePath; //local file name
            utility.Upload(request); //commensing the transfer

            return true; //indicate that the file was sent
        }
    }
}
