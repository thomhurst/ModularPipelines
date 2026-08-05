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

    private static MemoryStream CreateStream(string contents) =>
        new(Encoding.UTF8.GetBytes(contents));
}
