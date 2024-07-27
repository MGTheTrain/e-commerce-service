// <copyright file="PaypalSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Infrastructure.Settings;

public class AwsS3Settings
{
    public string Url { get; set; }

    public string Region { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
}