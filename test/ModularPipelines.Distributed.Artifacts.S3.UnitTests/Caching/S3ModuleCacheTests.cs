using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using ModularPipelines.Distributed.Artifacts.S3.Caching;
using ModularPipelines.Distributed.Artifacts.S3.Configuration;
using Moq;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests.Caching;

public class S3ModuleCacheTests
{
    private const string Fingerprint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

    [Test]
    public async Task WriteUsesStableCrossRunKey()
    {
        var s3 = new Mock<IAmazonS3>();
        PutObjectRequest? request = null;
        s3.Setup(client => client.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutObjectRequest, CancellationToken>((value, _) => request = value)
            .ReturnsAsync(new PutObjectResponse());
        using var cache = CreateCache(s3.Object);
        await using var content = new MemoryStream([1, 2, 3]);

        await cache.WriteAsync(Fingerprint, content, CancellationToken.None);

        await Assert.That(request).IsNotNull();
        await Assert.That(request!.BucketName).IsEqualTo("cache-bucket");
        await Assert.That(request.Key).IsEqualTo(
            "custom-prefix/module-cache/v1/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.zip");
    }

    [Test]
    public async Task OpenReadReturnsStoredContent()
    {
        var s3 = new Mock<IAmazonS3>();
        s3.Setup(client => client.GetObjectAsync(
                "cache-bucket",
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse
            {
                ResponseStream = new MemoryStream([4, 5, 6]),
            });
        using var cache = CreateCache(s3.Object);

        await using var result = await cache.OpenReadAsync(Fingerprint, CancellationToken.None);

        await Assert.That(result).IsNotNull();
        using var destination = new MemoryStream();
        await result!.CopyToAsync(destination);
        await Assert.That(destination.ToArray()).IsEquivalentTo(new byte[] { 4, 5, 6 });
    }

    [Test]
    public async Task OpenReadReturnsNullForMissingObject()
    {
        var s3 = new Mock<IAmazonS3>();
        s3.Setup(client => client.GetObjectAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("missing")
            {
                StatusCode = HttpStatusCode.NotFound,
            });
        using var cache = CreateCache(s3.Object);

        var result = await cache.OpenReadAsync(Fingerprint, CancellationToken.None);

        await Assert.That(result).IsNull();
    }

    private static S3ModuleCache CreateCache(IAmazonS3 client) =>
        new(
            new S3ArtifactOptions
            {
                BucketName = "cache-bucket",
                KeyPrefix = "custom-prefix",
            },
            client);
}
