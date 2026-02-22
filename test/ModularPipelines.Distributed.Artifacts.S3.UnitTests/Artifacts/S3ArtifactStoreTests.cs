using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests.Artifacts;

public class S3ArtifactStoreTests
{
    private Mock<IAmazonS3> _mockS3 = null!;
    private S3DistributedArtifactStore _store = null!;

    [Before(Test)]
    public void Setup()
    {
        _mockS3 = new Mock<IAmazonS3>();
        _store = new S3DistributedArtifactStore(
            _mockS3.Object,
            "test-bucket",
            "modpipe-artifacts",
            "run123",
            3600);
    }

    [Test]
    public async Task Upload_CallsPutObjectAsync()
    {
        var descriptor = new ArtifactDescriptor("test-art", "Test.Module", "application/octet-stream");
        var data = new byte[] { 1, 2, 3, 4, 5 };

        _mockS3.Setup(s => s.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PutObjectResponse());

        using var stream = new MemoryStream(data);
        var reference = await _store.UploadAsync(descriptor, stream, CancellationToken.None);

        await Assert.That(reference.Name).IsEqualTo("test-art");
        await Assert.That(reference.ModuleTypeName).IsEqualTo("Test.Module");
        await Assert.That(reference.SizeBytes).IsEqualTo(5);

        // Called twice: once for data, once for metadata
        _mockS3.Verify(s => s.PutObjectAsync(
            It.IsAny<PutObjectRequest>(),
            It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Test]
    public async Task Upload_SetsCorrectBucketAndKey()
    {
        var descriptor = new ArtifactDescriptor("build-output", "My.BuildModule");
        PutObjectRequest? capturedRequest = null;

        _mockS3.Setup(s => s.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutObjectRequest, CancellationToken>((req, _) => capturedRequest ??= req)
            .ReturnsAsync(new PutObjectResponse());

        using var stream = new MemoryStream([1, 2]);
        await _store.UploadAsync(descriptor, stream, CancellationToken.None);

        await Assert.That(capturedRequest).IsNotNull();
        await Assert.That(capturedRequest!.BucketName).IsEqualTo("test-bucket");
        await Assert.That(capturedRequest.Key).StartsWith("modpipe-artifacts/run123/My.BuildModule/build-output/");
    }

    [Test]
    public async Task Upload_SetsExpirationTag()
    {
        var descriptor = new ArtifactDescriptor("art1", "Test.Module");
        PutObjectRequest? capturedRequest = null;

        _mockS3.Setup(s => s.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutObjectRequest, CancellationToken>((req, _) => capturedRequest ??= req)
            .ReturnsAsync(new PutObjectResponse());

        using var stream = new MemoryStream([1]);
        await _store.UploadAsync(descriptor, stream, CancellationToken.None);

        await Assert.That(capturedRequest).IsNotNull();
        var expiresTag = capturedRequest!.TagSet.FirstOrDefault(t => t.Key == "expires-at");
        await Assert.That(expiresTag).IsNotNull();
    }

    [Test]
    public async Task Download_CallsGetObjectAsync()
    {
        var reference = new ArtifactReference("art1", "test", "Test.Module", 3, null, DateTimeOffset.UtcNow);
        var data = new byte[] { 10, 20, 30 };

        _mockS3.Setup(s => s.GetObjectAsync(
            "test-bucket",
            It.Is<string>(k => k.Contains("art1")),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse
            {
                ResponseStream = new MemoryStream(data),
            });

        await using var result = await _store.DownloadAsync(reference, CancellationToken.None);
        using var ms = new MemoryStream();
        await result.CopyToAsync(ms);

        await Assert.That(ms.ToArray()).IsEquivalentTo(data);
    }

    [Test]
    public async Task Delete_CallsDeleteObjectAsync()
    {
        var reference = new ArtifactReference("art1", "test", "Test.Module", 3, null, DateTimeOffset.UtcNow);

        _mockS3.Setup(s => s.DeleteObjectAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteObjectResponse());

        await _store.DeleteAsync(reference, CancellationToken.None);

        // Called twice: once for data, once for metadata
        _mockS3.Verify(s => s.DeleteObjectAsync(
            "test-bucket",
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Test]
    public async Task ListArtifacts_ReturnsDeserializedReferences()
    {
        var ref1 = new ArtifactReference("id1", "art1", "Test.Module", 100, null, DateTimeOffset.UtcNow);
        var ref1Json = System.Text.Json.JsonSerializer.Serialize(ref1);

        _mockS3.Setup(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListObjectsV2Response
            {
                S3Objects = [new S3Object { Key = "modpipe-artifacts/run123/Test.Module/meta/id1.json" }],
                IsTruncated = false,
            });

        _mockS3.Setup(s => s.GetObjectAsync("test-bucket", "modpipe-artifacts/run123/Test.Module/meta/id1.json", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse
            {
                ResponseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(ref1Json)),
            });

        var results = await _store.ListArtifactsAsync("Test.Module", CancellationToken.None);

        await Assert.That(results.Count).IsEqualTo(1);
        await Assert.That(results[0].Name).IsEqualTo("art1");
        await Assert.That(results[0].ArtifactId).IsEqualTo("id1");
    }
}
