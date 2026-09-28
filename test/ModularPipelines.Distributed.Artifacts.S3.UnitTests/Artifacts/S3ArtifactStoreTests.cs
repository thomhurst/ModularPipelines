using System.Net;
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
        _store = CreateStore(partSizeBytes: 16 * 1024 * 1024);
    }

    [After(Test)]
    public void Cleanup() => _store.Dispose();

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
        await Assert.That(reference.ModuleId).IsEqualTo((ModuleId) "Test.Module");
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
        await Assert.That(capturedRequest.Key).StartsWith("modpipe/artifacts/run123/My.BuildModule/build-output/");
    }

    [Test]
    [Arguments("build/meta/other", "build%2Fmeta%2Fother")]
    [Arguments("build%2Fmeta", "build%252Fmeta")]
    public async Task Custom_Module_Id_Remains_One_Object_Key_Segment(string id, string encodedId)
    {
        var keys = new List<string>();
        _mockS3.Setup(instance => instance.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutObjectRequest, CancellationToken>((request, _) => keys.Add(request.Key))
            .ReturnsAsync(new PutObjectResponse());
        _mockS3.Setup(instance => instance.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListObjectsV2Response { S3Objects = [], IsTruncated = false });
        using var stream = new MemoryStream([1]);
        var reference = await _store.UploadAsync(new ArtifactDescriptor("output", id), stream, CancellationToken.None);
        await _store.ListArtifactsAsync(id, CancellationToken.None);

        await Assert.That(reference.ModuleId.Value).IsEqualTo(id);
        await Assert.That(keys[0]).StartsWith($"modpipe/artifacts/run123/{encodedId}/output/");
        await Assert.That(keys[1]).StartsWith($"modpipe/artifacts/run123/{encodedId}/meta/");
        _mockS3.Verify(instance => instance.ListObjectsV2Async(
            It.Is<ListObjectsV2Request>(request => request.Prefix == $"modpipe/artifacts/run123/{encodedId}/meta/"),
            It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task Upload_DisablesPayloadSigning()
    {
        var descriptor = new ArtifactDescriptor("art1", "Test.Module");
        PutObjectRequest? capturedRequest = null;

        _mockS3.Setup(s => s.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutObjectRequest, CancellationToken>((req, _) => capturedRequest ??= req)
            .ReturnsAsync(new PutObjectResponse());

        using var stream = new MemoryStream([1]);
        await _store.UploadAsync(descriptor, stream, CancellationToken.None);

        await Assert.That(capturedRequest).IsNotNull();
        await Assert.That(capturedRequest!.DisablePayloadSigning).IsTrue();
    }

    [Test]
    public async Task Upload_Counts_Bytes_From_NonSeekable_Or_Offset_Streams()
    {
        _mockS3.Setup(s => s.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PutObjectResponse());

        await using var nonSeekable = new NonSeekableStream(new MemoryStream([1, 2, 3, 4]));
        var fromNonSeekable = await _store.UploadAsync(new ArtifactDescriptor("a", "Test.Module"), nonSeekable, CancellationToken.None);

        using var offset = new MemoryStream([1, 2, 3, 4, 5, 6]) { Position = 2 };
        var fromOffset = await _store.UploadAsync(new ArtifactDescriptor("b", "Test.Module"), offset, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(fromNonSeekable.SizeBytes).IsEqualTo(4);
            await Assert.That(fromOffset.SizeBytes).IsEqualTo(4);
        }
    }

    [Test]
    public async Task Upload_Larger_Than_One_Part_Uses_Multipart_Upload()
    {
        using var store = CreateStore(partSizeBytes: 4);
        var uploadedParts = new List<(int PartNumber, byte[] Data)>();
        CompleteMultipartUploadRequest? completed = null;
        _mockS3.Setup(s => s.InitiateMultipartUploadAsync(It.IsAny<InitiateMultipartUploadRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new InitiateMultipartUploadResponse { UploadId = "upload-1" });
        _mockS3.Setup(s => s.UploadPartAsync(It.IsAny<UploadPartRequest>(), It.IsAny<CancellationToken>()))
            .Returns<UploadPartRequest, CancellationToken>((request, _) =>
            {
                using var copy = new MemoryStream();
                request.InputStream.CopyTo(copy);
                uploadedParts.Add((request.PartNumber!.Value, copy.ToArray()));
                return Task.FromResult(new UploadPartResponse { ETag = $"etag-{request.PartNumber}" });
            });
        _mockS3.Setup(s => s.CompleteMultipartUploadAsync(It.IsAny<CompleteMultipartUploadRequest>(), It.IsAny<CancellationToken>()))
            .Callback<CompleteMultipartUploadRequest, CancellationToken>((request, _) => completed = request)
            .ReturnsAsync(new CompleteMultipartUploadResponse());
        _mockS3.Setup(s => s.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PutObjectResponse());

        await using var source = new NonSeekableStream(new MemoryStream([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]));
        var reference = await store.UploadAsync(new ArtifactDescriptor("large", "Test.Module"), source, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(reference.SizeBytes).IsEqualTo(10);
            await Assert.That(uploadedParts.Select(part => part.PartNumber)).IsEquivalentTo([1, 2, 3]);
            await Assert.That(uploadedParts.SelectMany(part => part.Data).ToArray())
                .IsEquivalentTo(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            await Assert.That(completed).IsNotNull();
            await Assert.That(completed!.UploadId).IsEqualTo("upload-1");
            await Assert.That(completed.PartETags!.Select(part => part.ETag)).IsEquivalentTo(["etag-1", "etag-2", "etag-3"]);
        }

        // Only the metadata object uses PutObject; the content went through multipart.
        _mockS3.Verify(s => s.PutObjectAsync(
            It.Is<PutObjectRequest>(request => request.Key.EndsWith(".json", StringComparison.Ordinal)),
            It.IsAny<CancellationToken>()), Times.Once);
        _mockS3.Verify(s => s.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Failed_Multipart_Upload_Is_Aborted()
    {
        using var store = CreateStore(partSizeBytes: 4);
        _mockS3.Setup(s => s.InitiateMultipartUploadAsync(It.IsAny<InitiateMultipartUploadRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new InitiateMultipartUploadResponse { UploadId = "upload-2" });
        _mockS3.Setup(s => s.UploadPartAsync(It.IsAny<UploadPartRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("boom"));

        using var source = new MemoryStream(new byte[12]);
        await Assert.That(async () => await store.UploadAsync(new ArtifactDescriptor("large", "Test.Module"), source, CancellationToken.None))
            .Throws<AmazonS3Exception>();

        _mockS3.Verify(s => s.AbortMultipartUploadAsync(
            It.Is<AbortMultipartUploadRequest>(request => request.UploadId == "upload-2"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Download_CallsGetObjectAsync_And_Disposes_Response()
    {
        var reference = new ArtifactReference("art1", "test", "Test.Module", 3, null, DateTimeOffset.UtcNow);
        var data = new byte[] { 10, 20, 30 };
        var responseStream = new TrackingStream(data);

        _mockS3.Setup(s => s.GetObjectAsync(
            "test-bucket",
            It.Is<string>(k => k.Contains("art1")),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse
            {
                ResponseStream = responseStream,
            });

        await using var result = await _store.DownloadAsync(reference, CancellationToken.None);
        using var ms = new MemoryStream();
        await result.CopyToAsync(ms);

        using (Assert.Multiple())
        {
            await Assert.That(ms.ToArray()).IsEquivalentTo(data);
            await Assert.That(responseStream.IsDisposed).IsTrue();
        }
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
        var metadataStream = new TrackingStream(System.Text.Encoding.UTF8.GetBytes(ref1Json));

        _mockS3.Setup(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListObjectsV2Response
            {
                S3Objects = [new S3Object { Key = "modpipe/artifacts/run123/Test.Module/meta/id1.json" }],
                IsTruncated = false,
            });

        _mockS3.Setup(s => s.GetObjectAsync("test-bucket", "modpipe/artifacts/run123/Test.Module/meta/id1.json", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse
            {
                ResponseStream = metadataStream,
            });

        var results = await _store.ListArtifactsAsync("Test.Module", CancellationToken.None);

        await Assert.That(results.Count).IsEqualTo(1);
        await Assert.That(results[0].Name).IsEqualTo("art1");
        await Assert.That(results[0].ArtifactId).IsEqualTo("id1");
        await Assert.That(metadataStream.IsDisposed).IsTrue();
    }

    [Test]
    public async Task ListArtifacts_Skips_Malformed_Metadata()
    {
        _mockS3.Setup(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListObjectsV2Response
            {
                S3Objects = [new S3Object { Key = "modpipe/artifacts/run123/Test.Module/meta/bad.json" }],
                IsTruncated = false,
            });
        _mockS3.Setup(s => s.GetObjectAsync("test-bucket", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse
            {
                ResponseStream = new MemoryStream("{not json"u8.ToArray()),
            });

        var results = await _store.ListArtifactsAsync("Test.Module", CancellationToken.None);

        await Assert.That(results).IsEmpty();
    }

    [Test]
    public async Task ListArtifacts_Propagates_Storage_Errors()
    {
        _mockS3.Setup(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListObjectsV2Response
            {
                S3Objects = [new S3Object { Key = "modpipe/artifacts/run123/Test.Module/meta/id1.json" }],
                IsTruncated = false,
            });
        _mockS3.Setup(s => s.GetObjectAsync("test-bucket", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("Access Denied") { StatusCode = HttpStatusCode.Forbidden, ErrorCode = "AccessDenied" });

        await Assert.That(async () => await _store.ListArtifactsAsync("Test.Module", CancellationToken.None))
            .Throws<AmazonS3Exception>();
    }

    [Test]
    public async Task Dispose_DisposesS3Client()
    {
        _store.Dispose();

        _mockS3.Verify(s => s.Dispose(), Times.Once);
    }

    private S3DistributedArtifactStore CreateStore(int partSizeBytes) =>
        new(
            _mockS3.Object,
            new S3StorageOptions
            {
                BucketName = "test-bucket",
                MultipartPartSizeBytes = partSizeBytes,
            },
            "run123");

    private sealed class NonSeekableStream(Stream inner) : Stream
    {
        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count) => inner.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                inner.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    private sealed class TrackingStream(byte[] data) : MemoryStream(data)
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
