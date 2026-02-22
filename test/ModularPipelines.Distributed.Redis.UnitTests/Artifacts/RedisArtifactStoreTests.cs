using Moq;
using ModularPipelines.Distributed.Redis.Artifacts;
using ModularPipelines.Distributed.Redis.Coordination;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis.UnitTests.Artifacts;

public class RedisArtifactStoreTests
{
    private Mock<IDatabase> _mockDb = null!;
    private RedisKeyBuilder _keys = null!;
    private RedisDistributedArtifactStore _store = null!;

    [Before(Test)]
    public void Setup()
    {
        _mockDb = new Mock<IDatabase>(MockBehavior.Loose);
        _keys = new RedisKeyBuilder("modpipe", "run123");
        var options = new ArtifactOptions
        {
            MaxSingleUploadBytes = 100,
            ChunkSizeBytes = 50,
            TimeToLiveSeconds = 3600,
        };
        _store = new RedisDistributedArtifactStore(_mockDb.Object, _keys, options);
    }

    [Test]
    public async Task Upload_SmallArtifact_ReturnsCorrectReference()
    {
        var descriptor = new ArtifactDescriptor("test-art", "Test.Module", "application/octet-stream");
        var data = new byte[] { 1, 2, 3, 4, 5 };

        using var stream = new MemoryStream(data);
        var reference = await _store.UploadAsync(descriptor, stream, CancellationToken.None);

        await Assert.That(reference.Name).IsEqualTo("test-art");
        await Assert.That(reference.ModuleTypeName).IsEqualTo("Test.Module");
        await Assert.That(reference.SizeBytes).IsEqualTo(5);
        await Assert.That(reference.ContentType).IsEqualTo("application/octet-stream");
        await Assert.That(reference.ArtifactId).IsNotNull();
    }

    [Test]
    public async Task Upload_LargeArtifact_ReturnsCorrectSize()
    {
        var descriptor = new ArtifactDescriptor("big-art", "Test.Module");
        var data = new byte[150]; // Larger than MaxSingleUploadBytes (100)

        using var stream = new MemoryStream(data);
        var reference = await _store.UploadAsync(descriptor, stream, CancellationToken.None);

        await Assert.That(reference.SizeBytes).IsEqualTo(150);
        await Assert.That(reference.Name).IsEqualTo("big-art");
    }

    [Test]
    public async Task Download_SmallArtifact_RetrievesData()
    {
        var data = new byte[] { 10, 20, 30 };
        var reference = new ArtifactReference("art1", "test", "Test.Module", 3, null, DateTimeOffset.UtcNow);

        _mockDb.Setup(db => db.StringGetAsync(
            It.Is<RedisKey>(k => k.ToString().Contains("artifacts:data:art1") && !k.ToString().Contains("chunk")),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue)data);

        await using var result = await _store.DownloadAsync(reference, CancellationToken.None);
        using var ms = new MemoryStream();
        await result.CopyToAsync(ms);

        await Assert.That(ms.ToArray()).IsEquivalentTo(data);
    }

    [Test]
    public async Task ListArtifacts_ReturnsStoredReferences()
    {
        var ref1 = new ArtifactReference("id1", "art1", "Test.Module", 100, null, DateTimeOffset.UtcNow);
        var ref1Json = System.Text.Json.JsonSerializer.Serialize(ref1);

        _mockDb.Setup(db => db.SetMembersAsync(
            It.Is<RedisKey>(k => k.ToString().Contains("artifacts:index:Test.Module")),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync([new RedisValue("id1")]);

        _mockDb.Setup(db => db.StringGetAsync(
            It.Is<RedisKey>(k => k.ToString().Contains("artifacts:meta:id1")),
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(new RedisValue(ref1Json));

        var results = await _store.ListArtifactsAsync("Test.Module", CancellationToken.None);

        await Assert.That(results.Count).IsEqualTo(1);
        await Assert.That(results[0].Name).IsEqualTo("art1");
    }

    [Test]
    public async Task Delete_CallsKeyDeleteAndSetRemove()
    {
        var reference = new ArtifactReference("art1", "test", "Test.Module", 3, null, DateTimeOffset.UtcNow);

        await _store.DeleteAsync(reference, CancellationToken.None);

        // Verify meta key deleted
        _mockDb.Verify(db => db.KeyDeleteAsync(
            It.Is<RedisKey>(k => k.ToString().Contains("artifacts:meta:art1")),
            It.IsAny<CommandFlags>()), Times.Once);

        // Verify data key deleted
        _mockDb.Verify(db => db.KeyDeleteAsync(
            It.Is<RedisKey>(k => k.ToString() == _keys.ArtifactData("art1")),
            It.IsAny<CommandFlags>()), Times.Once);

        // Verify index updated
        _mockDb.Verify(db => db.SetRemoveAsync(
            It.Is<RedisKey>(k => k.ToString().Contains("artifacts:index:Test.Module")),
            It.Is<RedisValue>(v => v.ToString() == "art1"),
            It.IsAny<CommandFlags>()), Times.Once);
    }

    [Test]
    public async Task ArtifactKeyBuilder_GeneratesCorrectPatterns()
    {
        var keys = new RedisKeyBuilder("modpipe", "abc123");

        await Assert.That(keys.ArtifactMeta("art1")).IsEqualTo("modpipe:abc123:artifacts:meta:art1");
        await Assert.That(keys.ArtifactData("art1")).IsEqualTo("modpipe:abc123:artifacts:data:art1");
        await Assert.That(keys.ArtifactChunk("art1", 0)).IsEqualTo("modpipe:abc123:artifacts:data:art1:chunk:0");
        await Assert.That(keys.ArtifactChunk("art1", 3)).IsEqualTo("modpipe:abc123:artifacts:data:art1:chunk:3");
        await Assert.That(keys.ArtifactIndex("My.Module")).IsEqualTo("modpipe:abc123:artifacts:index:My.Module");
    }
}
