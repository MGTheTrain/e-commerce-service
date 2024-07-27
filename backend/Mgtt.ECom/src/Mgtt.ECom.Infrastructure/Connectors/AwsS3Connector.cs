// <copyright file="AwsS3Connector.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Mgtt.ECom.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mgtt.ECom.Infrastructure.Connectors
{
    public class AwsS3Connector : IBlobConnector
    {
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<AwsS3Connector> logger;

        public AwsS3Connector(ILogger<AwsS3Connector> logger, IOptions<AwsS3Settings> settings)
        {
            this.logger = logger;

            var awsCredentials = new BasicAWSCredentials(settings.Value.AccessKey, settings.Value.SecretKey);

            this.s3Client = new AmazonS3Client(awsCredentials, new AmazonS3Config
            {
                ServiceURL = settings.Value.Url,
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(settings.Value.Region),
                ForcePathStyle = true,
            });
        }

        public async Task UploadImageAsync(string bucketName, string key, string filePath)
        {
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
            }
            catch (AmazonS3Exception e)
            {
                this.logger.LogError($"Error uploading image: {e.Message}");
            }
        }

        public async Task DownloadImageAsync(string bucketName, string key, string downloadPath)
        {
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

        public async Task DeleteImageAsync(string bucketName, string key)
        {
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
    }
}
