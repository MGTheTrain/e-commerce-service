// <copyright file="AwsS3ConnectorTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.InfrastructureTest.Connectors
{
    using System.IO;
    using System.Threading.Tasks;
    using Mgtt.ECom.Infrastructure.Connectors;
    using Mgtt.ECom.Infrastructure.Settings;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class AwsS3ConnectorTest
    {
        private readonly AwsS3Connector awsS3Connector;

        public AwsS3ConnectorTest()
        {
            var settings = Options.Create(new AwsS3Settings
            {
                Url = "http://localhost:4566",
                Region = "us-east-1",
            });
            var mockLogger = Mock.Of<ILogger<AwsS3Connector>>();
            this.awsS3Connector = new AwsS3Connector(mockLogger, settings);
        }

        [Fact]
        public async Task UploadDownloadAndDeleteImageAsync_VerifiesImageLifecycle()
        {
            var bucketName = "test-bucket";
            var key = "test.txt";
            var filePath = "test.txt";
            var downloadPath = "Output/test.txt";
            var content = "Sample content";

            // Create the file to upload
            await File.WriteAllTextAsync(filePath, content);

            // Upload the image
            await this.awsS3Connector.UploadImageAsync(bucketName, key, filePath);

            // Verify that the image has been uploaded
            var listResponse = await this.awsS3Connector.ListObjectsAsync(bucketName);
            Assert.Contains(listResponse.S3Objects, obj => obj.Key == key);

            // Download the image
            await this.awsS3Connector.DownloadImageAsync(bucketName, key, downloadPath);

            // Verify that the image has been downloaded correctly
            Assert.True(File.Exists(downloadPath));
            var downloadedContent = await File.ReadAllTextAsync(downloadPath);
            Assert.Equal(content, downloadedContent);

            // Delete the image
            await this.awsS3Connector.DeleteImageAsync(bucketName, key);

            // Verify that the image has been deleted
            var listAfterDeleteResponse = await this.awsS3Connector.ListObjectsAsync(bucketName);
            Assert.DoesNotContain(listAfterDeleteResponse.S3Objects, obj => obj.Key == key);

            // Clean up
            File.Delete(filePath);
            File.Delete(downloadPath);
        }
    }
}