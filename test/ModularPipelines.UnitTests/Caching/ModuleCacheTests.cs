using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.UnitTests.Caching;

public class ModuleCacheTests
{
    private sealed class TrackingResultRepository : IModuleResultRepository
    {
        public static int SaveCount;

        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext)
        {
            Interlocked.Increment(ref SaveCount);
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext) => Task.FromResult<ModuleResult<T>?>(null);
    }

    [CacheInputs("input.txt")]
    [ProducesArtifact("output", "output.txt")]
    private sealed class CachedModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var value = System.IO.File.ReadAllText(Path.Combine(WorkingDirectory, "input.txt"));
            System.IO.File.WriteAllText(Path.Combine(WorkingDirectory, "output.txt"), $"output:{value}");
            return Task.FromResult<string?>($"result:{value}");
        }
    }

    private sealed class DependencyModule : Module<string>
    {
        public static string Value { get; set; } = string.Empty;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(Value);
    }

    private sealed class UncachedModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>("uncached");
    }

    [CacheInputs("mutable.txt")]
    private sealed class InputMutatingModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var path = Path.Combine(WorkingDirectory, "mutable.txt");
            var value = await System.IO.File.ReadAllTextAsync(path, cancellationToken);
            if (value == "before")
            {
                await System.IO.File.WriteAllTextAsync(path, "after", cancellationToken);
            }

            return value;
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    private sealed class CachedDependentModule : Module<string>
    {
        public static int ExecutionCount;

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var dependency = await context.GetModule<DependencyModule>();
            return $"dependent:{dependency.ValueOrDefault}";
        }

        protected override ModularPipelines.Configuration.ModuleConfiguration Configure() =>
            ModularPipelines.Configuration.ModuleConfiguration.Create()
                .WithCacheKeyPart("dependent-v1")
                .Build();
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheHitSkipsExecutionAndRestoresArtifactsUntilInputChanges()
    {
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), $"ModularPipelines-cache-test-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        CachedModule.WorkingDirectory = temporaryDirectory;
        CachedModule.ExecutionCount = 0;

        try
        {
            var inputPath = Path.Combine(temporaryDirectory, "input.txt");
            var outputPath = Path.Combine(temporaryDirectory, "output.txt");
            await System.IO.File.WriteAllTextAsync(inputPath, "first");

            var firstStatus = await RunPipelineAsync(temporaryDirectory, cacheDirectory);
            System.IO.File.Delete(outputPath);
            var secondStatus = await RunPipelineAsync(temporaryDirectory, cacheDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(firstStatus).IsEqualTo(Status.Successful);
                await Assert.That(secondStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(CachedModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(await System.IO.File.ReadAllTextAsync(outputPath)).IsEqualTo("output:first");
            }

            await System.IO.File.WriteAllTextAsync(inputPath, "changed-value");
            var thirdStatus = await RunPipelineAsync(temporaryDirectory, cacheDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(thirdStatus).IsEqualTo(Status.Successful);
                await Assert.That(CachedModule.ExecutionCount).IsEqualTo(2);
                await Assert.That(await System.IO.File.ReadAllTextAsync(outputPath)).IsEqualTo("output:changed-value");
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task GlobExpansionEnforcesConfiguredFileLimit()
    {
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), $"ModularPipelines-cache-limit-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);

        try
        {
            await System.IO.File.WriteAllTextAsync(Path.Combine(temporaryDirectory, "one.cs"), "1");
            await System.IO.File.WriteAllTextAsync(Path.Combine(temporaryDirectory, "two.cs"), "2");

            var exception = Assert.Throws<InvalidOperationException>(() =>
                ModuleCacheFileResolver.ResolveFiles(temporaryDirectory, ["**/*.cs"], maximumFiles: 1));

            await Assert.That(exception.Message).Contains("configured limit of 1");
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ContentChangesInvalidateHashWhenLengthAndTimestampAreUnchanged()
    {
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), $"ModularPipelines-cache-hash-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);

        try
        {
            var path = Path.Combine(temporaryDirectory, "input.txt");
            await System.IO.File.WriteAllTextAsync(path, "first");
            var timestamp = System.IO.File.GetLastWriteTimeUtc(path);
            var hasher = new ModuleCacheFileHasher(OptionsFactory.Create(new ModuleCacheOptions()));
            var firstHash = (await hasher.HashAsync([path], CancellationToken.None))[path];

            await System.IO.File.WriteAllTextAsync(path, "other");
            System.IO.File.SetLastWriteTimeUtc(path, timestamp);
            var secondHash = (await hasher.HashAsync([path], CancellationToken.None))[path];

            await Assert.That(secondHash).IsNotEqualTo(firstHash);
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task GlobExpansionExcludesConfiguredCacheDirectory()
    {
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), $"ModularPipelines-cache-glob-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(cacheDirectory);

        try
        {
            var input = Path.Combine(temporaryDirectory, "input.txt");
            var cacheEntry = Path.Combine(cacheDirectory, "entry.zip");
            await System.IO.File.WriteAllTextAsync(input, "input");
            await System.IO.File.WriteAllTextAsync(cacheEntry, "cache");

            var files = ModuleCacheFileResolver.ResolveFiles(
                temporaryDirectory,
                ["**/*"],
                maximumFiles: 10,
                excludedDirectory: cacheDirectory);

            await Assert.That(files).IsEquivalentTo(new[] { input });
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    [Arguments(false)]
    [Arguments(true)]
    public async Task CacheComposesWithResultRepositoryRegardlessOfRegistrationOrder(
        bool cacheRegisteredFirst)
    {
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), $"ModularPipelines-cache-compose-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        TrackingResultRepository.SaveCount = 0;

        try
        {
            var builder = TestPipelineHostBuilder.Create();
            if (cacheRegisteredFirst)
            {
                builder
                    .AddModuleCache<FileSystemModuleCache>(options =>
                    {
                        options.WorkingDirectory = temporaryDirectory;
                        options.CacheDirectory = Path.Combine(temporaryDirectory, "cache");
                    })
                    .AddResultsRepository<TrackingResultRepository>();
            }
            else
            {
                builder
                    .AddResultsRepository<TrackingResultRepository>()
                    .AddModuleCache<FileSystemModuleCache>(options =>
                    {
                        options.WorkingDirectory = temporaryDirectory;
                        options.CacheDirectory = Path.Combine(temporaryDirectory, "cache");
                    });
            }

            await using var host = await builder
                .AddModule<UncachedModule>()
                .BuildAsync();

            await host.RunAsync();

            using (Assert.Multiple())
            {
                await Assert.That(TrackingResultRepository.SaveCount).IsEqualTo(1);
                await Assert.That(host.Services.GetRequiredService<IModuleResultRepository>())
                    .IsTypeOf<TrackingResultRepository>();
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task FluentCacheInputsArePreserved()
    {
        var configuration = ModularPipelines.Configuration.ModuleConfiguration.Create()
            .WithCacheKeyPart("configuration=v1")
            .WithCacheEnvironmentVariable("CI")
            .Build();

        using (Assert.Multiple())
        {
            await Assert.That(configuration.CacheEnabled).IsTrue();
            await Assert.That(configuration.CacheKeyParts).IsEquivalentTo(new[] { "configuration=v1" });
            await Assert.That(configuration.CacheEnvironmentVariables).IsEquivalentTo(new[] { "CI" });
        }
    }

    [Test]
    public async Task FingerprintValidationRejectsNonSha256Values()
    {
        ModuleCacheFingerprint.Validate(new string('A', 64));

        var exception = Assert.Throws<ArgumentException>(
            () => ModuleCacheFingerprint.Validate("not-a-fingerprint"));

        await Assert.That(exception.ParamName).IsEqualTo("fingerprint");
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task DependencyValueInvalidatesDependentFingerprintWithoutTimingNoise()
    {
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), $"ModularPipelines-cache-dependency-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        DependencyModule.Value = "one";
        CachedDependentModule.ExecutionCount = 0;

        try
        {
            var firstStatus = await RunDependencyPipelineAsync(temporaryDirectory);
            var secondStatus = await RunDependencyPipelineAsync(temporaryDirectory);
            DependencyModule.Value = "two";
            var thirdStatus = await RunDependencyPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(firstStatus).IsEqualTo(Status.Successful);
                await Assert.That(secondStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(thirdStatus).IsEqualTo(Status.Successful);
                await Assert.That(CachedDependentModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task SaveUsesFingerprintCapturedBeforeExecution()
    {
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), $"ModularPipelines-cache-mutation-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        InputMutatingModule.WorkingDirectory = temporaryDirectory;
        InputMutatingModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(Path.Combine(temporaryDirectory, "mutable.txt"), "before");

            var firstStatus = await RunInputMutatingPipelineAsync(temporaryDirectory);
            var secondStatus = await RunInputMutatingPipelineAsync(temporaryDirectory);
            var thirdStatus = await RunInputMutatingPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(firstStatus).IsEqualTo(Status.Successful);
                await Assert.That(secondStatus).IsEqualTo(Status.Successful);
                await Assert.That(thirdStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(InputMutatingModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    private static async Task<Status> RunPipelineAsync(string workingDirectory, string cacheDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = cacheDirectory;
            })
            .AddModule<CachedModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(CachedModule))!
            .ModuleStatus;
    }

    private static async Task<Status> RunDependencyPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<DependencyModule>()
            .AddModule<CachedDependentModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(CachedDependentModule))!
            .ModuleStatus;
    }

    private static async Task<Status> RunInputMutatingPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<InputMutatingModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(InputMutatingModule))!
            .ModuleStatus;
    }
}
