using ModularPipelines.Distributed.Artifacts;

namespace ModularPipelines.Distributed.UnitTests.Artifacts;

public class InMemoryArtifactStoreTests
{
    [Test]
    public async Task Upload_And_Download_Returns_Same_Data()
    {
        var store = new InMemoryDistributedArtifactStore();
        var data = new byte[] { 1, 2, 3, 4, 5 };

        var descriptor = new ArtifactDescriptor(
            Name: "test-artifact",
            ModuleTypeName: "Test.Module",
            ContentType: "application/octet-stream");

        ArtifactReference reference;
        using (var uploadStream = new MemoryStream(data))
        {
            reference = await store.UploadAsync(descriptor, uploadStream, CancellationToken.None);
        }

        await Assert.That(reference).IsNotNull();
        await Assert.That(reference.Name).IsEqualTo("test-artifact");
        await Assert.That(reference.ModuleTypeName).IsEqualTo("Test.Module");
        await Assert.That(reference.SizeBytes).IsEqualTo(5);

        await using var downloadStream = await store.DownloadAsync(reference, CancellationToken.None);
        using var ms = new MemoryStream();
        await downloadStream.CopyToAsync(ms);

        await Assert.That(ms.ToArray()).IsEquivalentTo(data);
    }

    [Test]
    public async Task ListArtifacts_Returns_Uploaded_Artifacts()
    {
        var store = new InMemoryDistributedArtifactStore();

        var descriptor1 = new ArtifactDescriptor("art1", "Module.A");
        var descriptor2 = new ArtifactDescriptor("art2", "Module.A");
        var descriptor3 = new ArtifactDescriptor("art3", "Module.B");

        using (var s1 = new MemoryStream([1, 2]))
        {
            await store.UploadAsync(descriptor1, s1, CancellationToken.None);
        }

        using (var s2 = new MemoryStream([3, 4]))
        {
            await store.UploadAsync(descriptor2, s2, CancellationToken.None);
        }

        using (var s3 = new MemoryStream([5, 6]))
        {
            await store.UploadAsync(descriptor3, s3, CancellationToken.None);
        }

        var moduleAList = await store.ListArtifactsAsync("Module.A", CancellationToken.None);
        var moduleBList = await store.ListArtifactsAsync("Module.B", CancellationToken.None);
        var moduleCList = await store.ListArtifactsAsync("Module.C", CancellationToken.None);

        await Assert.That(moduleAList.Count).IsEqualTo(2);
        await Assert.That(moduleBList.Count).IsEqualTo(1);
        await Assert.That(moduleCList.Count).IsEqualTo(0);
    }

    [Test]
    public async Task Delete_Removes_Artifact()
    {
        var store = new InMemoryDistributedArtifactStore();

        var descriptor = new ArtifactDescriptor("art1", "Module.A");
        ArtifactReference reference;
        using (var stream = new MemoryStream([1, 2, 3]))
        {
            reference = await store.UploadAsync(descriptor, stream, CancellationToken.None);
        }

        await store.DeleteAsync(reference, CancellationToken.None);

        var list = await store.ListArtifactsAsync("Module.A", CancellationToken.None);
        await Assert.That(list.Count).IsEqualTo(0);
    }

    [Test]
    public async Task Download_NonExistent_Throws()
    {
        var store = new InMemoryDistributedArtifactStore();

        var fakeRef = new ArtifactReference(
            ArtifactId: "nonexistent",
            Name: "fake",
            ModuleTypeName: "Fake.Module",
            SizeBytes: 0,
            ContentType: null,
            UploadedAt: DateTimeOffset.UtcNow);

        Assert.Throws<InvalidOperationException>(() => store.DownloadAsync(fakeRef, CancellationToken.None));
    }
}
