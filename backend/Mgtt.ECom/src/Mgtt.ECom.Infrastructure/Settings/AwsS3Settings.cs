// <copyright file="PaypalSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Infrastructure.Settings;

public class AwsS3Settings
{
    public string Url { get; set; }

    public string Region { get; set; }
    
    // public string AccessKey { get; set; } // Exported as an environment variable due to issues encountered with BasicAWSCredentials(...) on both public S3 and LocalStack S3 instances.

    // public string SecretKey { get; set; } // Exported as an environment variable due to issues encountered with BasicAWSCredentials(...) on both public S3 and LocalStack S3 instances.
}