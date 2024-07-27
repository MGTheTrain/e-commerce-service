﻿// <copyright file="IBlobConnector.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Infrastructure.Connectors;

public interface IBlobConnector
{
    Task UploadImageAsync(string bucketName, string key, string filePath);
    Task UploadImageAsync(string bucketName, string key, Stream inputStream);
    Task DownloadImageAsync(string bucketName, string key, string downloadPath);
    Task<Stream> DownloadImageAsync(string bucketName, string key);
    Task DeleteImageAsync(string bucketName, string key);
}
