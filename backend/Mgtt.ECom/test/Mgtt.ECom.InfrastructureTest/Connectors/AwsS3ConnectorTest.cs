// <copyright file="AwsS3ConnectorTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.InfrastructureTest.Connectors
{
    using System.IO;
    using System.Text;
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
            var mockLogger = Mock.Of<ILogger<AwsS3Connector>>();
            var utilizeLocalStackS3 = true;
            this.awsS3Connector = new AwsS3Connector(mockLogger, utilizeLocalStackS3);
        }

        [Fact]
        public async Task UploadDownloadAndDeleteImageUsingFilePathAsync_VerifiesImageLifecycle()
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

        [Fact]
        public async Task UploadDownloadAndDeleteImageUsingStreamsAsync_VerifiesImageLifecycle()
        {
            var bucketName = "test-bucket";
            var key = "test.txt";
            var content = "Sample content";

            // Create a stream to upload
            var uploadStream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            // Upload the image
            await this.awsS3Connector.UploadImageAsync(bucketName, key, uploadStream);

            // Verify that the image has been uploaded
            var listResponse = await this.awsS3Connector.ListObjectsAsync(bucketName);
            Assert.Contains(listResponse.S3Objects, obj => obj.Key == key);

            // Download the image
            var downloadStream = await this.awsS3Connector.DownloadImageAsync(bucketName, key);

            // Verify that the image has been downloaded correctly
            using (var reader = new StreamReader(downloadStream))
            {
                var downloadedContent = await reader.ReadToEndAsync();
                Assert.Equal(content, downloadedContent);
            }

            // Delete the image
            await this.awsS3Connector.DeleteImageAsync(bucketName, key);

            // Verify that the image has been deleted
            var listAfterDeleteResponse = await this.awsS3Connector.ListObjectsAsync(bucketName);
            Assert.DoesNotContain(listAfterDeleteResponse.S3Objects, obj => obj.Key == key);
        }
    }
}