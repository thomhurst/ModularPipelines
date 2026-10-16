using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
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

    private sealed class GenericModule<T>;

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task SaveTimes_BoundsFileNamesAndPreservesLongSubModuleNames(bool generic)
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            var moduleType = generic ? typeof(GenericModule<Dictionary<string, List<GenericModule<int[]>>>>) : typeof(PrefixModule);
            var subModuleName = new string('界', 4096) + "/release:\n\0";
            var duration = TimeSpan.FromSeconds(7);
            var provider = CreateProvider(directory);

            await provider.SaveModuleTimeAsync(ModuleId.FromType(moduleType), duration);
            await provider.SaveSubModuleTimeAsync(ModuleId.FromType(moduleType), new SubModuleEstimation(subModuleName, duration));

            // Read through a fresh provider to verify persistence rather than an in-memory cache.
            var reader = CreateProvider(directory);
            var estimations = (await reader.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(moduleType))).ToArray();

            await Assert.That(await reader.GetModuleEstimatedTimeAsync(ModuleId.FromType(moduleType))).IsEqualTo(duration);
            await Assert.That(estimations).HasSingleItem();
            await Assert.That(estimations[0].SubModuleName).IsEqualTo(subModuleName);
            await Assert.That(estimations[0].EstimatedDuration).IsEqualTo(duration);
            await Assert.That(Directory.GetFiles(directory)).Count().IsEqualTo(2);
            foreach (var path in Directory.GetFiles(directory))
            {
                await Assert.That(Path.GetFileName(path).Length).IsLessThan(160);
            }
        });
    }

    [Test]
    public async Task SaveTimes_StoresReadableNamesInsideEntries()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            var provider = CreateProvider(directory);
            const string subModuleName = "Build release";
            await provider.SaveModuleTimeAsync(ModuleId.FromType(typeof(PrefixModule)), TimeSpan.FromSeconds(3));
            await provider.SaveSubModuleTimeAsync(ModuleId.FromType(typeof(PrefixModule)), new SubModuleEstimation(subModuleName, TimeSpan.FromSeconds(5)));

            foreach (var path in Directory.GetFiles(directory))
            {
                using var document = JsonDocument.Parse(await File.ReadAllTextAsync(path));
                await Assert.That(document.RootElement.GetProperty("moduleType").GetString())
                    .IsEqualTo(ModuleId.FromType(typeof(PrefixModule)).Value);
                if (Path.GetFileName(path).StartsWith("Mod-", StringComparison.Ordinal))
                {
                    await Assert.That(document.RootElement.GetProperty("subModuleName").GetString()).IsEqualTo(subModuleName);
                }
            }
        });
    }

    [Test]
    public async Task GetTimes_IgnoresPreviousTypeKeyedEntries()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            await File.WriteAllTextAsync(Path.Combine(directory, $"{typeof(PrefixModule).FullName}.txt"), "00:00:01");
            await File.WriteAllTextAsync(Path.Combine(directory, $"Mod-{typeof(PrefixModule).FullName}-Sub-build.txt"), "00:00:01");
            var oldKey = "V2-" + Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(
                Encoding.UTF8.GetBytes(ModuleTypeIdentifier.Get(typeof(PrefixModule)))));
            var oldEntry = JsonSerializer.Serialize(
                new EstimatedTimeEntry(ModuleTypeIdentifier.Get(typeof(PrefixModule)), "build", TimeSpan.FromSeconds(1)),
                EstimatedTimeJsonContext.Default.EstimatedTimeEntry);
            await File.WriteAllTextAsync(Path.Combine(directory, $"{oldKey}.txt"), oldEntry);
            await File.WriteAllTextAsync(Path.Combine(directory, $"Mod-{oldKey}-Sub-hash.txt"), oldEntry);
            var provider = CreateProvider(directory);
            var id = ModuleId.FromType(typeof(PrefixModule));

            await Assert.That(await provider.GetModuleEstimatedTimeAsync(id)).IsEqualTo(FileSystemModuleEstimatedTimeProvider.DefaultEstimate);
            await Assert.That(await provider.GetSubModuleEstimatedTimesAsync(id)).IsEmpty();

            await provider.SaveModuleTimeAsync(id, TimeSpan.FromSeconds(3));
            await provider.SaveSubModuleTimeAsync(id, new SubModuleEstimation("build", TimeSpan.FromSeconds(5)));
            var reader = CreateProvider(directory);
            await Assert.That(await reader.GetModuleEstimatedTimeAsync(id)).IsEqualTo(TimeSpan.FromSeconds(3));
            await Assert.That((await reader.GetSubModuleEstimatedTimesAsync(id)).Single().EstimatedDuration).IsEqualTo(TimeSpan.FromSeconds(5));
        });
    }

    [Test]
    [Arguments("not JSON")]
    [Arguments("null")]
    [Arguments("{}")]
    [Arguments("{\"moduleType\":\"module\",\"subModuleName\":null,\"duration\":\"invalid\"}")]
    public async Task GetTimes_IgnoresMalformedHashedEntries(string contents)
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            var provider = CreateProvider(directory);
            await provider.SaveModuleTimeAsync(ModuleId.FromType(typeof(PrefixModule)), TimeSpan.FromSeconds(3));
            await provider.SaveSubModuleTimeAsync(ModuleId.FromType(typeof(PrefixModule)), new SubModuleEstimation("build", TimeSpan.FromSeconds(5)));
            foreach (var path in Directory.GetFiles(directory))
            {
                await File.WriteAllTextAsync(path, contents);
            }

            await Assert.That(await provider.GetModuleEstimatedTimeAsync(ModuleId.FromType(typeof(PrefixModule))))
                .IsEqualTo(FileSystemModuleEstimatedTimeProvider.DefaultEstimate);
            await Assert.That(await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)))).IsEmpty();
        });
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task SaveTimes_ReusesHistoryAcrossAssemblyVersions(bool generic)
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            var assemblyName = $"EstimateVersionTests{Guid.NewGuid():N}";
            var firstType = CreateType(assemblyName, new Version(1, 0));
            var secondType = CreateType(assemblyName, new Version(2, 0));
            if (generic)
            {
                firstType = typeof(GenericModule<>).MakeGenericType(typeof(List<>).MakeGenericType(firstType.MakeArrayType()));
                secondType = typeof(GenericModule<>).MakeGenericType(typeof(List<>).MakeGenericType(secondType.MakeArrayType()));
            }

            var duration = TimeSpan.FromSeconds(9);
            var provider = CreateProvider(directory);
            await provider.SaveModuleTimeAsync(ModuleId.FromType(firstType), duration);
            await provider.SaveSubModuleTimeAsync(ModuleId.FromType(firstType), new SubModuleEstimation("build", duration));

            var reader = CreateProvider(directory);
            var estimations = (await reader.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(secondType))).ToArray();

            await Assert.That(await reader.GetModuleEstimatedTimeAsync(ModuleId.FromType(secondType))).IsEqualTo(duration);
            await Assert.That(estimations).HasSingleItem();
            await Assert.That(estimations[0].SubModuleName).IsEqualTo("build");
            await Assert.That(estimations[0].EstimatedDuration).IsEqualTo(duration);

            await reader.SaveModuleTimeAsync(ModuleId.FromType(secondType), duration);
            await reader.SaveSubModuleTimeAsync(ModuleId.FromType(secondType), new SubModuleEstimation("build", duration));
            await Assert.That(Directory.GetFiles(directory)).Count().IsEqualTo(2);
        });
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task SaveTimes_SeparatesTypesFromDifferentAssemblies(bool generic)
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            var firstType = CreateType($"FirstEstimateTests{Guid.NewGuid():N}", new Version(1, 0));
            var secondType = CreateType($"SecondEstimateTests{Guid.NewGuid():N}", new Version(1, 0));
            if (generic)
            {
                firstType = typeof(GenericModule<>).MakeGenericType(firstType);
                secondType = typeof(GenericModule<>).MakeGenericType(secondType);
            }

            var provider = CreateProvider(directory);
            await provider.SaveModuleTimeAsync(ModuleId.FromType(firstType), TimeSpan.FromSeconds(3));
            await provider.SaveModuleTimeAsync(ModuleId.FromType(secondType), TimeSpan.FromSeconds(5));
            await provider.SaveSubModuleTimeAsync(ModuleId.FromType(firstType), new SubModuleEstimation("first", TimeSpan.FromSeconds(3)));
            await provider.SaveSubModuleTimeAsync(ModuleId.FromType(secondType), new SubModuleEstimation("second", TimeSpan.FromSeconds(5)));

            await Assert.That(await provider.GetModuleEstimatedTimeAsync(ModuleId.FromType(firstType))).IsEqualTo(TimeSpan.FromSeconds(3));
            await Assert.That(await provider.GetModuleEstimatedTimeAsync(ModuleId.FromType(secondType))).IsEqualTo(TimeSpan.FromSeconds(5));
            await Assert.That((await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(firstType))).Single().SubModuleName).IsEqualTo("first");
            await Assert.That((await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(secondType))).Single().SubModuleName).IsEqualTo("second");
        });
    }

    [Test]
    public async Task GetSubModuleEstimatedTimes_UsesExactModuleName()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            WriteEstimation(directory, ModuleId.FromType(typeof(PrefixModule)), "expected", TimeSpan.FromSeconds(1));
            WriteEstimation(directory, ModuleId.FromType(typeof(PrefixModuleSuffix)), "other", TimeSpan.FromSeconds(2));
            var provider = CreateProvider(directory);

            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)))).ToArray();

            await Assert.That(estimations).HasSingleItem();
            await Assert.That(estimations[0].SubModuleName).IsEqualTo("expected");
        });
    }

    [Test]
    public async Task GetSubModuleEstimatedTimes_ReusesDirectoryIndexAcrossModules()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            WriteEstimation(directory, ModuleId.FromType(typeof(PrefixModule)), "first", TimeSpan.FromSeconds(1));
            var cachedPath = WriteEstimation(
                directory,
                ModuleId.FromType(typeof(PrefixModuleSuffix)),
                "cached",
                TimeSpan.FromSeconds(2));
            var provider = CreateProvider(directory);

            _ = await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)));
            File.WriteAllText(cachedPath, JsonSerializer.Serialize(
                new EstimatedTimeEntry(ModuleId.FromType(typeof(PrefixModuleSuffix)).Value, "updated", TimeSpan.FromSeconds(2)),
                EstimatedTimeJsonContext.Default.EstimatedTimeEntry));
            WriteEstimation(directory, ModuleId.FromType(typeof(PrefixModuleSuffix)), "external", TimeSpan.FromSeconds(3));
            var estimations =
                (await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModuleSuffix)))).ToArray();

            await Assert.That(estimations).HasSingleItem();
            await Assert.That(estimations[0].SubModuleName).IsEqualTo("updated");
        });
    }

    [Test]
    public async Task GetSubModuleEstimatedTimes_PrunesExpiredFiles()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            var expiredPath = WriteEstimation(
                directory,
                ModuleId.FromType(typeof(PrefixModule)),
                "expired",
                TimeSpan.FromSeconds(1));
            File.SetLastWriteTimeUtc(expiredPath, CurrentTime.UtcDateTime.AddDays(-91));
            WriteEstimation(directory, ModuleId.FromType(typeof(PrefixModule)), "current", TimeSpan.FromSeconds(2));
            var provider = CreateProvider(directory);

            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)))).ToArray();

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
            WriteEstimation(directory, ModuleId.FromType(typeof(PrefixModule)), "first", TimeSpan.FromSeconds(1));
            var provider = CreateProvider(directory);
            _ = await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)));

            await provider.SaveSubModuleTimeAsync(
                ModuleId.FromType(typeof(PrefixModule)),
                new SubModuleEstimation("second", TimeSpan.FromSeconds(2)));
            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)))).ToArray();

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
                ModuleId.FromType(typeof(PrefixModule)),
                new SubModuleEstimation(subModuleName, duration));
            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)))).ToArray();

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
            WriteEstimation(directory, ModuleId.FromType(typeof(PrefixModule)), "first", TimeSpan.FromSeconds(1));
            var timeProvider = new FakeTimeProvider(CurrentTime);
            var provider = new FileSystemModuleEstimatedTimeProvider(directory, timeProvider);
            _ = await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)));

            WriteEstimation(directory, ModuleId.FromType(typeof(PrefixModule)), "external", TimeSpan.FromSeconds(2));
            timeProvider.Advance(TimeSpan.FromMinutes(2));
            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)))).ToArray();

            await Assert.That(estimations.Select(x => x.SubModuleName))
                .IsEquivalentTo(["first", "external"]);
        });
    }

    [Test]
    public async Task GetModuleEstimatedTime_Falls_Back_When_Entry_Cannot_Be_Read()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            // A directory where the entry file should be makes the read fail with an access error.
            Directory.CreateDirectory(Path.Combine(directory, $"{FileSystemModuleEstimatedTimeProvider.GetStorageKey(ModuleId.FromType(typeof(PrefixModule)))}.txt"));
            var provider = CreateProvider(directory);

            var estimate = await provider.GetModuleEstimatedTimeAsync(ModuleId.FromType(typeof(PrefixModule)));

            await Assert.That(estimate).IsEqualTo(FileSystemModuleEstimatedTimeProvider.DefaultEstimate);
        });
    }

    [Test]
    public async Task GetModuleEstimatedTime_Falls_Back_For_Malformed_Entry()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            await File.WriteAllTextAsync(
                Path.Combine(directory, $"{FileSystemModuleEstimatedTimeProvider.GetStorageKey(ModuleId.FromType(typeof(PrefixModule)))}.txt"),
                "not a duration");
            var provider = CreateProvider(directory);

            var estimate = await provider.GetModuleEstimatedTimeAsync(ModuleId.FromType(typeof(PrefixModule)));

            await Assert.That(estimate).IsEqualTo(FileSystemModuleEstimatedTimeProvider.DefaultEstimate);
        });
    }

    [Test]
    public async Task SaveModuleTime_Replaces_Entry_Without_Leaving_Temporary_Files()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            var provider = CreateProvider(directory);

            await Task.WhenAll(Enumerable.Range(1, 20).Select(seconds =>
                provider.SaveModuleTimeAsync(ModuleId.FromType(typeof(PrefixModule)), TimeSpan.FromSeconds(seconds))));
            var estimate = await provider.GetModuleEstimatedTimeAsync(ModuleId.FromType(typeof(PrefixModule)));

            using (Assert.Multiple())
            {
                await Assert.That(estimate).IsBetween(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20));
                await Assert.That(Directory.EnumerateFiles(directory, "*.tmp")).IsEmpty();
            }
        });
    }

    private static FileSystemModuleEstimatedTimeProvider CreateProvider(string directory)
    {
        return new FileSystemModuleEstimatedTimeProvider(directory, new FakeTimeProvider(CurrentTime));
    }

    [Test]
    public async Task SaveSubModuleTime_ReplacesEntryAtomicallyWithoutTemporaryFiles()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            var provider = CreateProvider(directory);
            await Task.WhenAll(Enumerable.Range(1, 20).Select(seconds =>
                provider.SaveSubModuleTimeAsync(ModuleId.FromType(typeof(PrefixModule)), new SubModuleEstimation("build", TimeSpan.FromSeconds(seconds)))));

            var estimations = (await CreateProvider(directory).GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)))).ToArray();

            await Assert.That(estimations).HasSingleItem();
            await Assert.That(estimations[0].SubModuleName).IsEqualTo("build");
            await Assert.That(estimations[0].EstimatedDuration).IsBetween(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20));
            await Assert.That(Directory.GetFiles(directory)).HasSingleItem();
        });
    }

    [Test]
    public async Task GetSubModuleEstimatedTimes_PrunesExpiredHashedFilesAndRefreshesIndex()
    {
        await RunWithTemporaryDirectoryAsync(async directory =>
        {
            var timeProvider = new FakeTimeProvider(CurrentTime);
            var provider = new FileSystemModuleEstimatedTimeProvider(directory, timeProvider);
            await provider.SaveSubModuleTimeAsync(ModuleId.FromType(typeof(PrefixModule)), new SubModuleEstimation("expired", TimeSpan.FromSeconds(1)));
            var expiredPath = Directory.GetFiles(directory).Single();
            File.SetLastWriteTimeUtc(expiredPath, CurrentTime.UtcDateTime.AddDays(-91));
            await provider.SaveSubModuleTimeAsync(ModuleId.FromType(typeof(PrefixModule)), new SubModuleEstimation("current", TimeSpan.FromSeconds(2)));

            var estimations = (await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)))).ToArray();
            await Assert.That(estimations).HasSingleItem();
            await Assert.That(estimations[0].SubModuleName).IsEqualTo("current");
            await Assert.That(File.Exists(expiredPath)).IsFalse();

            await CreateProvider(directory).SaveSubModuleTimeAsync(ModuleId.FromType(typeof(PrefixModule)), new SubModuleEstimation("external", TimeSpan.FromSeconds(3)));
            await Assert.That(await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)))).HasSingleItem();
            timeProvider.Advance(TimeSpan.FromMinutes(2));
            estimations = (await provider.GetSubModuleEstimatedTimesAsync(ModuleId.FromType(typeof(PrefixModule)))).ToArray();
            await Assert.That(estimations.Select(estimation => estimation.SubModuleName)).IsEquivalentTo(["current", "external"]);
        });
    }

    private static Type CreateType(string assemblyName, Version version)
    {
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName(assemblyName) { Version = version },
            AssemblyBuilderAccess.RunAndCollect);
        return assembly.DefineDynamicModule(assemblyName)
            .DefineType("Estimates.BuildModule", TypeAttributes.Public)
            .CreateType()!;
    }

    private static string WriteEstimation(
        string directory,
        ModuleId moduleId,
        string subModuleName,
        TimeSpan duration)
    {
        var subModuleHash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(subModuleName)));
        var path = Path.Combine(directory, $"Mod-{FileSystemModuleEstimatedTimeProvider.GetStorageKey(moduleId)}-Sub-{subModuleHash}.txt");
        File.WriteAllText(path, JsonSerializer.Serialize(
            new EstimatedTimeEntry(moduleId.Value, subModuleName, duration),
            EstimatedTimeJsonContext.Default.EstimatedTimeEntry));
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
