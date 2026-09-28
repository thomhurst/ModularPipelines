using System.Text;
using ModularPipelines.Caching;
using ModularPipelines.UnitTests.Attributes;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.UnitTests.Caching;

public class FileSystemModuleCacheTests
{
    [Test]
    public async Task OpenReadAsync_Returns_Null_When_Entry_Does_Not_Exist()
    {
        var cache = new FileSystemModuleCache(OptionsFactory.Create(new ModuleCacheOptions
        {
            CacheDirectory = Path.Combine(
                Path.GetTempPath(),
                $"modular-pipelines-cache-{Guid.NewGuid():N}"),
        }));

        var stream = await cache.OpenReadAsync(new string('a', 64), CancellationToken.None);

        await Assert.That(stream).IsNull();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task WriteAsync_Replaces_Entry_While_Read_Stream_Is_Open()
    {
        var cacheDirectory = Path.Combine(
            Path.GetTempPath(),
            $"modular-pipelines-cache-{Guid.NewGuid():N}");
        var cache = new FileSystemModuleCache(OptionsFactory.Create(new ModuleCacheOptions
        {
            CacheDirectory = cacheDirectory,
        }));
        var fingerprint = new string('a', 64);

        try
        {
            await using (var initialContent = CreateStream("initial"))
            {
                await cache.WriteAsync(fingerprint, initialContent, CancellationToken.None);
            }

            await using (var existingReader = await cache.OpenReadAsync(
                             fingerprint,
                             CancellationToken.None))
            {
                await using var replacementContent = CreateStream("replacement");
                await cache.WriteAsync(fingerprint, replacementContent, CancellationToken.None);

                using var reader = new StreamReader(existingReader!, Encoding.UTF8);
                await Assert.That(await reader.ReadToEndAsync()).IsEqualTo("initial");
            }

            await using var replacementReader = await cache.OpenReadAsync(
                fingerprint,
                CancellationToken.None);
            using var replacementTextReader = new StreamReader(replacementReader!, Encoding.UTF8);
            await Assert.That(await replacementTextReader.ReadToEndAsync()).IsEqualTo("replacement");
        }
        finally
        {
            Directory.Delete(cacheDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ExistsAsync_And_DeleteAsync_Manage_Entries()
    {
        var cacheDirectory = Path.Combine(
            Path.GetTempPath(),
            $"modular-pipelines-cache-{Guid.NewGuid():N}");
        var cache = new FileSystemModuleCache(OptionsFactory.Create(new ModuleCacheOptions
        {
            CacheDirectory = cacheDirectory,
        }));
        var fingerprint = new string('b', 64);

        try
        {
            // Deleting before the cache directory exists is a no-op.
            await cache.DeleteAsync(fingerprint, CancellationToken.None);
            await Assert.That(await cache.ExistsAsync(fingerprint, CancellationToken.None)).IsFalse();

            await using (var content = CreateStream("entry"))
            {
                await cache.WriteAsync(fingerprint, content, CancellationToken.None);
            }

            await Assert.That(await cache.ExistsAsync(fingerprint, CancellationToken.None)).IsTrue();

            await cache.DeleteAsync(fingerprint, CancellationToken.None);

            using (Assert.Multiple())
            {
                await Assert.That(await cache.ExistsAsync(fingerprint, CancellationToken.None)).IsFalse();
                await Assert.That(await cache.OpenReadAsync(fingerprint, CancellationToken.None)).IsNull();
            }

            await cache.DeleteAsync(fingerprint, CancellationToken.None);
        }
        finally
        {
            if (Directory.Exists(cacheDirectory))
            {
                Directory.Delete(cacheDirectory, recursive: true);
            }
        }
    }

    [Test]
    public async Task Default_ExistsAsync_Opens_And_Disposes_The_Entry()
    {
        var entry = new TrackingStream();
        IModuleCacheStore present = new OpenOnlyStore(entry);
        IModuleCacheStore missing = new OpenOnlyStore(null);

        using (Assert.Multiple())
        {
            await Assert.That(await present.ExistsAsync(new string('c', 64), CancellationToken.None)).IsTrue();
            await Assert.That(entry.Disposed).IsTrue();
            await Assert.That(await missing.ExistsAsync(new string('c', 64), CancellationToken.None)).IsFalse();
        }
    }

    private sealed class OpenOnlyStore(Stream? entry) : IModuleCacheStore
    {
        public Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken) =>
            Task.FromResult(entry);

        public Task WriteAsync(string fingerprint, Stream content, CancellationToken cancellationToken) =>
            throw new NotSupportedException();

        public Task DeleteAsync(string fingerprint, CancellationToken cancellationToken) =>
            throw new NotSupportedException();
    }

    private sealed class TrackingStream : MemoryStream
    {
        public bool Disposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            Disposed = true;
            base.Dispose(disposing);
        }
    }

    private static MemoryStream CreateStream(string contents) =>
        new(Encoding.UTF8.GetBytes(contents));
}
