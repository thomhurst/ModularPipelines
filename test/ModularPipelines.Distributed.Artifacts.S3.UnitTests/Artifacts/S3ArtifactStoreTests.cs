using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ModularPipelines.Distributed.Artifacts.S3.Artifacts;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests.Artifacts;

public class S3ArtifactStoreTests
{
    private Mock<IAmazonS3> _mockS3 = null!;
    private S3DistributedArtifactStore _store = null!;
    private Mock<ILogger> _logger = null!;

    [Before(Test)]
    public void Setup()
    {
        _mockS3 = new Mock<IAmazonS3>();
        _logger = new Mock<ILogger>();
        _logger.Setup(logger => logger.IsEnabled(LogLevel.Information)).Returns(true);
        _store = CreateStore(partSizeBytes: 16 * 1024 * 1024);
    }

    [After(Test)]
    public void Cleanup() => _store.Dispose();

    [Test]
    public async Task Upload_CallsPutObjectAsync()
    {
        var descriptor = new ArtifactDescriptor
        {
            Name = "test-art",
            ModuleId = "Test.Module",
            ContentType = "application/octet-stream",
        };
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
        var descriptor = new ArtifactDescriptor
        {
            Name = "build-output",
            ModuleId = "My.BuildModule",
        };
        PutObjectRequest? capturedRequest = null;

        _mockS3.Setup(s => s.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutObjectRequest, CancellationToken>((req, _) => capturedRequest ??= req)
            .ReturnsAsync(new PutObjectResponse());

        using var stream = new MemoryStream([1, 2]);
        await _store.UploadAsync(descriptor, stream, CancellationToken.None);

        await Assert.That(capturedRequest).IsNotNull();
        await Assert.That(capturedRequest!.BucketName).IsEqualTo("test-bucket");
        await Assert.That(capturedRequest.Key).StartsWith("modpipe/artifacts/run123/My.BuildModule/data/build-output/");
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
        var reference = await _store.UploadAsync(new ArtifactDescriptor
        {
            Name = "output",
            ModuleId = id,
        }, stream, CancellationToken.None);
        await _store.ListArtifactsAsync(id, CancellationToken.None);

        await Assert.That(reference.ModuleId.Value).IsEqualTo(id);
        await Assert.That(keys[0]).StartsWith($"modpipe/artifacts/run123/{encodedId}/data/output/");
        await Assert.That(keys[1]).StartsWith($"modpipe/artifacts/run123/{encodedId}/meta/");
        _mockS3.Verify(instance => instance.ListObjectsV2Async(
            It.Is<ListObjectsV2Request>(request => request.Prefix == $"modpipe/artifacts/run123/{encodedId}/meta/"),
            It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    [Arguments("meta", "meta")]
    [Arguments("meta/output", "meta%2Foutput")]
    [Arguments("_meta", "_meta")]
    [Arguments("data/output", "data%2Foutput")]
    [Arguments("meta%2Foutput", "meta%252Foutput")]
    [Arguments("build output/雪.zip", "build%20output%2F%E9%9B%AA.zip")]
    public async Task Artifact_Names_Stay_In_The_Data_Prefix_Throughout_Their_Lifecycle(string name, string encodedName)
    {
        var objects = new Dictionary<string, byte[]>();
        _mockS3.Setup(s => s.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutObjectRequest, CancellationToken>((request, _) =>
            {
                if (request.ContentBody is not null)
                {
                    objects.Add(request.Key, System.Text.Encoding.UTF8.GetBytes(request.ContentBody));
                    return;
                }

                using var copy = new MemoryStream();
                request.InputStream.CopyTo(copy);
                objects.Add(request.Key, copy.ToArray());
            })
            .ReturnsAsync(new PutObjectResponse());
        _mockS3.Setup(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ListObjectsV2Request request, CancellationToken _) => new ListObjectsV2Response
            {
                S3Objects = [.. objects.Keys.Where(key => key.StartsWith(request.Prefix, StringComparison.Ordinal)).Select(key => new S3Object { Key = key })],
                IsTruncated = false,
            });
        _mockS3.Setup(s => s.GetObjectAsync("test-bucket", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, string key, CancellationToken _) => new GetObjectResponse
            {
                ResponseStream = new MemoryStream(objects[key]),
            });
        _mockS3.Setup(s => s.DeleteObjectAsync("test-bucket", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, string, CancellationToken>((_, key, _) => objects.Remove(key))
            .ReturnsAsync(new DeleteObjectResponse());

        byte[] payload = [1, 2, 3];
        using var stream = new MemoryStream(payload);
        var uploaded = await _store.UploadAsync(new ArtifactDescriptor
        {
            Name = name,
            ModuleId = "Test.Module",
        }, stream, CancellationToken.None);

        var listed = await _store.ListArtifactsAsync("Test.Module", CancellationToken.None);

        // Listing must never fetch the payload, even when its name resembles the metadata prefix.
        _mockS3.Verify(s => s.GetObjectAsync("test-bucket",
            It.Is<string>(key => objects[key].SequenceEqual(payload)),
            It.IsAny<CancellationToken>()), Times.Never());
        await Assert.That(listed).HasSingleItem();
        await Assert.That(listed[0].Name).IsEqualTo(name);
        await Assert.That(listed[0].ArtifactId).IsEqualTo(uploaded.ArtifactId);
        var dataKey = $"modpipe/artifacts/run123/Test.Module/data/{encodedName}/{uploaded.ArtifactId}";
        var metaKey = $"modpipe/artifacts/run123/Test.Module/meta/{uploaded.ArtifactId}.json";
        await Assert.That(objects.Keys).IsEquivalentTo([dataKey, metaKey]);

        await using var downloaded = await _store.DownloadAsync(listed[0], CancellationToken.None);
        using var copy = new MemoryStream();
        await downloaded.CopyToAsync(copy);
        await Assert.That(copy.ToArray()).IsEquivalentTo(payload);

        await _store.DeleteAsync(listed[0], CancellationToken.None);
        await Assert.That(objects).IsEmpty();
    }

    [Test]
    public async Task Upload_DisablesPayloadSigning()
    {
        var descriptor = new ArtifactDescriptor
        {
            Name = "art1",
            ModuleId = "Test.Module",
        };
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
        var fromNonSeekable = await _store.UploadAsync(new ArtifactDescriptor { Name = "a", ModuleId = "Test.Module" }, nonSeekable, CancellationToken.None);

        using var offset = new MemoryStream([1, 2, 3, 4, 5, 6]) { Position = 2 };
        var fromOffset = await _store.UploadAsync(new ArtifactDescriptor { Name = "b", ModuleId = "Test.Module" }, offset, CancellationToken.None);

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
        var reference = await store.UploadAsync(new ArtifactDescriptor { Name = "large", ModuleId = "Test.Module" }, source, CancellationToken.None);

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
        await Assert.That(async () => await store.UploadAsync(new ArtifactDescriptor { Name = "large", ModuleId = "Test.Module" }, source, CancellationToken.None))
            .Throws<AmazonS3Exception>();

        _mockS3.Verify(s => s.AbortMultipartUploadAsync(
            It.Is<AbortMultipartUploadRequest>(request => request.UploadId == "upload-2"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Download_CallsGetObjectAsync_And_Disposes_Response()
    {
        var reference = new ArtifactReference
        {
            ArtifactId = "art1",
            Name = "test",
            ModuleId = "Test.Module",
            SizeBytes = 3,
            ContentType = null,
            UploadedAt = DateTimeOffset.UtcNow,
        };
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
        var reference = new ArtifactReference
        {
            ArtifactId = "art1",
            Name = "test",
            ModuleId = "Test.Module",
            SizeBytes = 3,
            ContentType = null,
            UploadedAt = DateTimeOffset.UtcNow,
        };

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
        var ref1 = new ArtifactReference
        {
            ArtifactId = "id1",
            Name = "art1",
            ModuleId = "Test.Module",
            SizeBytes = 100,
            ContentType = null,
            UploadedAt = DateTimeOffset.UtcNow,
        };
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
    public async Task Run_Cleanup_Deletes_Every_Version_And_Delete_Marker_Under_The_Run_Prefix()
    {
        var listRequests = new List<(string Prefix, string? KeyMarker, string? VersionIdMarker)>();
        var deleted = new List<(string Key, string VersionId)>();
        _mockS3.Setup(s => s.ListVersionsAsync(It.IsAny<ListVersionsRequest>(), It.IsAny<CancellationToken>()))
            .Callback<ListVersionsRequest, CancellationToken>((request, _) =>
                listRequests.Add((request.Prefix, request.KeyMarker, request.VersionIdMarker)))
            .ReturnsAsync((ListVersionsRequest request, CancellationToken _) => request.KeyMarker is null
                ? new ListVersionsResponse
                {
                    Versions =
                    [
                        new S3ObjectVersion { Key = "modpipe/artifacts/run123/A/output/1", VersionId = "v2" },
                        new S3ObjectVersion { Key = "modpipe/artifacts/run123/A/output/1", VersionId = "v1" },
                    ],
                    IsTruncated = true,
                    NextKeyMarker = "modpipe/artifacts/run123/A/output/1",
                    NextVersionIdMarker = "v1",
                }
                : new ListVersionsResponse
                {
                    Versions =
                    [
                        new S3ObjectVersion
                        {
                            Key = "modpipe/artifacts/run123/A/meta/1.json",
                            VersionId = "marker",
                            IsDeleteMarker = true,
                        },
                    ],
                    IsTruncated = false,
                });
        _mockS3.Setup(s => s.DeleteObjectsAsync(It.IsAny<DeleteObjectsRequest>(), It.IsAny<CancellationToken>()))
            .Callback<DeleteObjectsRequest, CancellationToken>((request, _) =>
                deleted.AddRange(request.Objects.Select(item => (item.Key, item.VersionId))))
            .ReturnsAsync(new DeleteObjectsResponse());

        await _store.CleanupRunAsync(
            new DistributedRunCleanupContext { RunId = "run123", PipelineSucceeded = true },
            CancellationToken.None);

        List<(string, string?, string?)> expectedListRequests =
        [
            ("modpipe/artifacts/run123/", null, null),
            ("modpipe/artifacts/run123/", "modpipe/artifacts/run123/A/output/1", "v1"),
        ];
        List<(string, string)> expectedDeleted =
        [
            ("modpipe/artifacts/run123/A/output/1", "v2"),
            ("modpipe/artifacts/run123/A/output/1", "v1"),
            ("modpipe/artifacts/run123/A/meta/1.json", "marker"),
        ];
        await Assert.That(listRequests).IsEquivalentTo(expectedListRequests);
        await Assert.That(deleted).IsEquivalentTo(expectedDeleted);
        _mockS3.Verify(s => s.DeleteObjectsAsync(
            It.Is<DeleteObjectsRequest>(request => request.BucketName == "test-bucket"),
            It.IsAny<CancellationToken>()), Times.Exactly(2));
        _mockS3.Verify(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    [Arguments(HttpStatusCode.BadRequest)]
    [Arguments(HttpStatusCode.Forbidden)]
    [Arguments(HttpStatusCode.MethodNotAllowed)]
    [Arguments(HttpStatusCode.NotImplemented)]
    public async Task Run_Cleanup_Falls_Back_To_Object_Listing_When_Version_Listing_Is_Rejected(HttpStatusCode statusCode)
    {
        var deletedKeys = new List<string>();
        _mockS3.Setup(s => s.ListVersionsAsync(It.IsAny<ListVersionsRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("Version listing rejected") { StatusCode = statusCode });
        _mockS3.Setup(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ListObjectsV2Request request, CancellationToken _) => request.ContinuationToken is null
                ? new ListObjectsV2Response
                {
                    S3Objects = [new S3Object { Key = "modpipe/artifacts/run123/A/output/1" }],
                    IsTruncated = true,
                    NextContinuationToken = "page-2",
                }
                : new ListObjectsV2Response
                {
                    S3Objects = [new S3Object { Key = "modpipe/artifacts/run123/A/meta/1.json" }],
                    IsTruncated = false,
                });
        _mockS3.Setup(s => s.DeleteObjectsAsync(It.IsAny<DeleteObjectsRequest>(), It.IsAny<CancellationToken>()))
            .Callback<DeleteObjectsRequest, CancellationToken>((request, _) =>
                deletedKeys.AddRange(request.Objects.Select(item => item.Key)))
            .ReturnsAsync(new DeleteObjectsResponse());

        await _store.CleanupRunAsync(
            new DistributedRunCleanupContext { RunId = "run123", PipelineSucceeded = true },
            CancellationToken.None);

        List<string> expectedDeletedKeys =
        [
            "modpipe/artifacts/run123/A/output/1",
            "modpipe/artifacts/run123/A/meta/1.json",
        ];
        await Assert.That(deletedKeys).IsEquivalentTo(expectedDeletedKeys);
        _mockS3.Verify(s => s.ListObjectsV2Async(
            It.Is<ListObjectsV2Request>(request => request.Prefix == "modpipe/artifacts/run123/"),
            It.IsAny<CancellationToken>()), Times.Exactly(2));
        _mockS3.Verify(s => s.DeleteObjectsAsync(
            It.Is<DeleteObjectsRequest>(request => request.Objects!.All(item => item.VersionId == null)),
            It.IsAny<CancellationToken>()), Times.Exactly(2));
        var informationMessages = _logger.Invocations
            .Where(call => call.Method.Name == nameof(ILogger.Log) && call.Arguments[0] is LogLevel.Information)
            .Select(call => call.Arguments[2]!.ToString())
            .ToArray();
        await Assert.That(informationMessages).HasSingleItem();
        await Assert.That(informationMessages[0]).Contains($"HTTP {(int) statusCode}");
        await Assert.That(informationMessages[0]).Contains("old versions and delete markers remain");
    }

    [Test]
    [Arguments(HttpStatusCode.NotFound)]
    [Arguments(HttpStatusCode.TooManyRequests)]
    [Arguments(HttpStatusCode.InternalServerError)]
    [Arguments(HttpStatusCode.ServiceUnavailable)]
    public async Task Run_Cleanup_Propagates_Other_Version_Listing_Errors(HttpStatusCode statusCode)
    {
        _mockS3.Setup(s => s.ListVersionsAsync(It.IsAny<ListVersionsRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("Listing failed") { StatusCode = statusCode });

        await Assert.That(async () => await _store.CleanupRunAsync(
                new DistributedRunCleanupContext { RunId = "run123", PipelineSucceeded = true },
                CancellationToken.None))
            .Throws<AmazonS3Exception>();

        _mockS3.Verify(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()), Times.Never());
        _mockS3.Verify(s => s.DeleteObjectsAsync(It.IsAny<DeleteObjectsRequest>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    [Timeout(30_000)]
    [Arguments(HttpStatusCode.NotFound)]
    [Arguments(HttpStatusCode.TooManyRequests)]
    [Arguments(HttpStatusCode.InternalServerError)]
    [Arguments(HttpStatusCode.ServiceUnavailable)]
    public async Task Run_Cleanup_Logs_Other_Listing_Errors_As_Warnings_Without_Failing_The_Pipeline(
        HttpStatusCode statusCode,
        CancellationToken cancellationToken)
    {
        var exception = new AmazonS3Exception("Listing failed") { StatusCode = statusCode };
        _mockS3.Setup(s => s.ListVersionsAsync(It.IsAny<ListVersionsRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
        var logger = new Mock<ILogger>();
        logger.Setup(instance => instance.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        var provider = new Mock<ILoggerProvider>();
        provider.Setup(instance => instance.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
        var builder = Pipeline.CreateBuilder();
        builder.AddDistributedMode(options => options with { RunId = "run123", TotalInstances = 1 });
        builder.Services.AddSingleton<IDistributedArtifactStore>(_store);
        builder.Logging.ClearProviders().AddProvider(provider.Object);
        builder.AddModule<NoOpModule>();

        await using var pipeline = await builder.BuildAsync();
        var summary = await pipeline.RunAsync(cancellationToken);

        await Assert.That(summary.Succeeded).IsTrue();
        logger.Verify(instance => instance.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) => state!.ToString()!.Contains("Could not clean up distributed run data stored by")),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once());
        _mockS3.Verify(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    [Arguments(HttpStatusCode.BadRequest)]
    [Arguments(HttpStatusCode.Forbidden)]
    [Arguments(HttpStatusCode.MethodNotAllowed)]
    [Arguments(HttpStatusCode.NotImplemented)]
    public async Task Run_Cleanup_Does_Not_Fall_Back_When_Version_Deletion_Is_Rejected(HttpStatusCode statusCode)
    {
        _mockS3.Setup(s => s.ListVersionsAsync(It.IsAny<ListVersionsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListVersionsResponse
            {
                Versions = [new S3ObjectVersion { Key = "modpipe/artifacts/run123/A/output/1", VersionId = "v1" }],
                IsTruncated = false,
            });
        _mockS3.Setup(s => s.DeleteObjectsAsync(It.IsAny<DeleteObjectsRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("Deletion rejected") { StatusCode = statusCode });

        await Assert.That(async () => await _store.CleanupRunAsync(
                new DistributedRunCleanupContext { RunId = "run123", PipelineSucceeded = true },
                CancellationToken.None))
            .Throws<AmazonS3Exception>();

        _mockS3.Verify(s => s.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    public async Task Run_Cleanup_Fails_When_Objects_Were_Not_Deleted()
    {
        _mockS3.Setup(s => s.ListVersionsAsync(It.IsAny<ListVersionsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListVersionsResponse
            {
                Versions = [new S3ObjectVersion { Key = "modpipe/artifacts/run123/A/output/1", VersionId = "v1" }],
                IsTruncated = false,
            });
        _mockS3.Setup(s => s.DeleteObjectsAsync(It.IsAny<DeleteObjectsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteObjectsResponse
            {
                DeleteErrors = [new DeleteError { Key = "modpipe/artifacts/run123/A/output/1", Code = "AccessDenied" }],
            });

        await Assert.That(async () => await _store.CleanupRunAsync(
                new DistributedRunCleanupContext { RunId = "run123", PipelineSucceeded = true },
                CancellationToken.None))
            .Throws<DeleteObjectsException>();
    }

    [Test]
    public async Task Run_Cleanup_Of_An_Empty_Run_Deletes_Nothing()
    {
        _mockS3.Setup(s => s.ListVersionsAsync(It.IsAny<ListVersionsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ListVersionsResponse { Versions = null, IsTruncated = false });

        await _store.CleanupRunAsync(
            new DistributedRunCleanupContext { RunId = "run123", PipelineSucceeded = true },
            CancellationToken.None);

        _mockS3.Verify(s => s.DeleteObjectsAsync(It.IsAny<DeleteObjectsRequest>(), It.IsAny<CancellationToken>()), Times.Never());
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
            "run123",
            _logger.Object);

    private sealed class NoOpModule : Module<int>
    {
        protected override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(0);
    }

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
