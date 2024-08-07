// <copyright file="AwsS3Connector.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Infrastructure.Connectors
{
    using Amazon.Runtime;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Mgtt.ECom.Infrastructure.Settings;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class AwsS3Connector : IBlobConnector
    {
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<AwsS3Connector> logger;
        private readonly IOptions<AwsS3Settings> settings;

        public AwsS3Connector(ILogger<AwsS3Connector> logger, IOptions<AwsS3Settings> settings)
        {
            this.logger = logger;
            this.settings = settings;

            // The AWS SDK will use externally set environment variables for credentials and configurations
            if (settings.Value.UtilizeLocalStack)
            {
                var awsConfig = new AmazonS3Config
                {
                    ForcePathStyle = true, // Necessary when working with LocalStack S3 instances
                };

                this.s3Client = new AmazonS3Client(awsConfig);
            }
            else
            {
                this.s3Client = new AmazonS3Client();
            }
        }

        private async Task EnsureBucketExistsAsync(string bucketName)
        {
            try
            {
                var listBucketsResponse = await this.s3Client.ListBucketsAsync();
                if (!listBucketsResponse.Buckets.Any(b => b.BucketName == bucketName))
                {
                    var createBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true,
                    };
                    await this.s3Client.PutBucketAsync(createBucketRequest);
                    this.logger.LogInformation($"Bucket {bucketName} created successfully.");
                }
            }
            catch (AmazonS3Exception e)
            {
                this.logger.LogError($"Error checking or creating bucket: {e.Message}");
                throw;
            }
        }

        public async Task<string?> UploadImageAsync(string bucketName, string key, string filePath)
        {
            await this.EnsureBucketExistsAsync(bucketName);
            this.logger.LogInformation($"Uploading {filePath} to bucket {bucketName}...");

            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    FilePath = filePath,
                };

                PutObjectResponse response = await this.s3Client.PutObjectAsync(putRequest);
                this.logger.LogInformation($"Upload response status code: {response.HttpStatusCode}");

                var url = string.Empty;
                if (this.settings.Value.UtilizeLocalStack)
                {
                    url = $"http://localhost:4566/{bucketName}/{key}";
                }
                else
                {
                    url = $"https://{bucketName}.s3.amazonaws.com/{key}";
                }

                return url;
            }
            catch (AmazonS3Exception e)
            {
                this.logger.LogError($"Error uploading image: {e.Message}");
                return null;
            }
        }

        public async Task<string?> UploadImageAsync(string bucketName, string key, Stream inputStream)
        {
            await this.EnsureBucketExistsAsync(bucketName);
            this.logger.LogInformation($"Uploading {key} to bucket {bucketName}...");

            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    InputStream = inputStream,
                };

                PutObjectResponse response = await this.s3Client.PutObjectAsync(putRequest);
                this.logger.LogInformation($"Upload response status code: {response.HttpStatusCode}");

                var url = string.Empty;
                if (this.settings.Value.UtilizeLocalStack)
                {
                    url = $"http://localhost:4566/{bucketName}/{key}";
                }
                else
                {
                    url = $"https://{bucketName}.s3.amazonaws.com/{key}";
                }

                return url;
            }
            catch (AmazonS3Exception e)
            {
                this.logger.LogError($"Error uploading image: {e.Message}");
                return null;
            }
        }

        public async Task DownloadImageAsync(string bucketName, string key, string downloadPath)
        {
            await this.EnsureBucketExistsAsync(bucketName);
            this.logger.LogInformation($"Downloading {key} from bucket {bucketName}...");

            try
            {
                var getRequest = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                };

                using (GetObjectResponse response = await this.s3Client.GetObjectAsync(getRequest))
                {
                    await response.WriteResponseStreamToFileAsync(downloadPath, false, default);
                    this.logger.LogInformation($"Downloaded image to {downloadPath}");
                }
            }
            catch (AmazonS3Exception e)
            {
                this.logger.LogError($"Error downloading image: {e.Message}");
            }
        }

        public async Task<Stream> DownloadImageAsync(string bucketName, string key)
        {
            await this.EnsureBucketExistsAsync(bucketName);
            this.logger.LogInformation($"Downloading {key} from bucket {bucketName}...");

            try
            {
                var getRequest = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                };

                GetObjectResponse response = await this.s3Client.GetObjectAsync(getRequest);
                var outputStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(outputStream);
                outputStream.Position = 0; // Reset the stream position to the beginning
                this.logger.LogInformation($"Downloaded image to stream.");
                return outputStream;
            }
            catch (AmazonS3Exception e)
            {
                this.logger.LogError($"Error downloading image: {e.Message}");
                throw;
            }
        }

        public async Task DeleteImageAsync(string bucketName, string key)
        {
            await this.EnsureBucketExistsAsync(bucketName);
            this.logger.LogInformation($"Deleting {key} from bucket {bucketName}...");

            try
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                };

                DeleteObjectResponse response = await this.s3Client.DeleteObjectAsync(deleteRequest);
                this.logger.LogInformation($"Delete response status code: {response.HttpStatusCode}");
            }
            catch (AmazonS3Exception e)
            {
                this.logger.LogError($"Error deleting image: {e.Message}");
            }
        }

        public async Task<ListObjectsV2Response> ListObjectsAsync(string bucketName)
        {
            try
            {
                await this.EnsureBucketExistsAsync(bucketName);
                var request = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                };

                ListObjectsV2Response response = await this.s3Client.ListObjectsV2Async(request);
                return response;
            }
            catch (AmazonS3Exception e)
            {
                this.logger.LogError($"Error listing objects: {e.Message}");
                throw;
            }
        }
    }
}
