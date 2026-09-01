using ModularPipelines.Reporting;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Engine;

public class FileSystemModuleEstimatedTimeProviderTests
{
    private static readonly DateTimeOffset CurrentTime = new(2026, 7, 29, 0, 0, 0, TimeSpan.Zero);

    private sealed class PrefixModule;

    private sealed class PrefixModuleSuffix;

    [Test]
    public async Task GetSubModuleEstimatedTimes_UsesExactModuleName()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            WriteEstimation(directory, typeof(PrefixModule), "expected", TimeSpan.FromSeconds(1));
            WriteEstimation(directory, typeof(PrefixModuleSuffix), "other", TimeSpan.FromSeconds(2));
            var provider = CreateProvider(directory);

            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(typeof(PrefixModule))).ToArray();

            await Assert.That(estimations).HasSingleItem();
            await Assert.That(estimations[0].SubModuleName).IsEqualTo("expected");
        });
    }

    [Test]
    public async Task GetSubModuleEstimatedTimes_ReusesDirectoryIndexAcrossModules()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            WriteEstimation(directory, typeof(PrefixModule), "first", TimeSpan.FromSeconds(1));
            var cachedPath = WriteEstimation(
                directory,
                typeof(PrefixModuleSuffix),
                "cached",
                TimeSpan.FromSeconds(2));
            var provider = CreateProvider(directory);

            _ = await provider.GetSubModuleEstimatedTimesAsync(typeof(PrefixModule));
            File.Delete(cachedPath);
            var estimations =
                (await provider.GetSubModuleEstimatedTimesAsync(typeof(PrefixModuleSuffix))).ToArray();

            await Assert.That(estimations).HasSingleItem();
            await Assert.That(estimations[0].SubModuleName).IsEqualTo("cached");
        });
    }

    [Test]
    public async Task GetSubModuleEstimatedTimes_PrunesExpiredFiles()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            var expiredPath = WriteEstimation(
                directory,
                typeof(PrefixModule),
                "expired",
                TimeSpan.FromSeconds(1));
            File.SetLastWriteTimeUtc(expiredPath, CurrentTime.UtcDateTime.AddDays(-91));
            WriteEstimation(directory, typeof(PrefixModule), "current", TimeSpan.FromSeconds(2));
            var provider = CreateProvider(directory);

            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(typeof(PrefixModule))).ToArray();

            await Assert.That(estimations).HasSingleItem();
            await Assert.That(estimations[0].SubModuleName).IsEqualTo("current");
            await Assert.That(File.Exists(expiredPath)).IsFalse();
        });
    }

    [Test]
    public async Task SaveSubModuleTime_InvalidatesDirectoryIndex()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            WriteEstimation(directory, typeof(PrefixModule), "first", TimeSpan.FromSeconds(1));
            var provider = CreateProvider(directory);
            _ = await provider.GetSubModuleEstimatedTimesAsync(typeof(PrefixModule));

            await provider.SaveSubModuleTimeAsync(
                typeof(PrefixModule),
                new SubModuleEstimation("second", TimeSpan.FromSeconds(2)));
            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(typeof(PrefixModule))).ToArray();

            await Assert.That(estimations.Select(x => x.SubModuleName))
                .IsEquivalentTo(["first", "second"]);
        });
    }

    [Test]
    public async Task SaveSubModuleTime_EncodesUnsafeNameAndRestoresOriginalName()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            const string subModuleName = "Build src/api:release";
            var duration = TimeSpan.FromSeconds(3);
            var provider = CreateProvider(directory);

            await provider.SaveSubModuleTimeAsync(
                typeof(PrefixModule),
                new SubModuleEstimation(subModuleName, duration));
            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(typeof(PrefixModule))).ToArray();

            await Assert.That(Directory.GetFiles(directory)).HasSingleItem();
            await Assert.That(estimations).HasSingleItem();
            await Assert.That(estimations[0].SubModuleName).IsEqualTo(subModuleName);
            await Assert.That(estimations[0].EstimatedDuration).IsEqualTo(duration);
        });
    }

    [Test]
    public async Task GetSubModuleEstimatedTimes_RefreshesExpiredDirectoryIndex()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            WriteEstimation(directory, typeof(PrefixModule), "first", TimeSpan.FromSeconds(1));
            var timeProvider = new FakeTimeProvider(CurrentTime);
            var provider = new FileSystemModuleEstimatedTimeProvider(directory, timeProvider);
            _ = await provider.GetSubModuleEstimatedTimesAsync(typeof(PrefixModule));

            WriteEstimation(directory, typeof(PrefixModule), "external", TimeSpan.FromSeconds(2));
            timeProvider.Advance(TimeSpan.FromMinutes(2));
            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(typeof(PrefixModule))).ToArray();

            await Assert.That(estimations.Select(x => x.SubModuleName))
                .IsEquivalentTo(["first", "external"]);
        });
    }

    private static FileSystemModuleEstimatedTimeProvider CreateProvider(string directory)
    {
        return new FileSystemModuleEstimatedTimeProvider(directory, new FakeTimeProvider(CurrentTime));
    }

    private static string WriteEstimation(
        string directory,
        Type moduleType,
        string subModuleName,
        TimeSpan duration)
    {
        var path = Path.Combine(
            directory,
            $"Mod-{moduleType.FullName}-Sub-{subModuleName}.txt");
        File.WriteAllText(path, duration.ToString());
        File.SetLastWriteTimeUtc(path, CurrentTime.UtcDateTime);
        return path;
    }

    private static async Task RunWithTemporaryDirectoryAsync(Func<string, Task> test)
    {
        var directory = Path.Combine(Path.GetTempPath(), $"ModularPipelines-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);

        try
        {
            await test(directory);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
