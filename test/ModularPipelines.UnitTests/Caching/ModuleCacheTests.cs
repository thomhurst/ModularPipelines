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

    [CacheInputs("hook-input.txt")]
    [ProducesArtifact("hook-output", "hook-output.txt")]
    private sealed class AfterHookArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            System.IO.File.WriteAllText(
                Path.Combine(WorkingDirectory, "hook-output.txt"),
                "before-hook");
            return Task.FromResult<string?>("result");
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            System.IO.File.WriteAllText(
                Path.Combine(WorkingDirectory, "hook-output.txt"),
                "after-hook");
            return Task.FromResult<ModuleResult<string>?>(null);
        }
    }

    private sealed class ResultTransformingCachedModule : Module<string>
    {
        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            return Task.FromResult<string?>("original");
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<ModuleResult<string>?>(
                result is ModuleResult<string>.Success success
                    ? success with { Value = "transformed" }
                    : null);
        }

        protected override ModularPipelines.Configuration.ModuleConfiguration Configure() =>
            ModularPipelines.Configuration.ModuleConfiguration.Create()
                .WithCacheKeyPart("result-transform-v1")
                .Build();
    }

    [ModularPipelines.Attributes.DependsOn<ResultTransformingCachedModule>]
    private sealed class TransformedResultDependentModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var dependency = await context.GetModule<ResultTransformingCachedModule>();
            return dependency.ValueOrDefault;
        }
    }

    [CacheInputs("set-input.txt")]
    [ProducesArtifact("artifact-set", "artifact-set")]
    private sealed class VaryingArtifactSetModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var value = System.IO.File.ReadAllText(
                Path.Combine(WorkingDirectory, "set-input.txt"));
            var artifactDirectory = Path.Combine(WorkingDirectory, "artifact-set");
            Directory.CreateDirectory(artifactDirectory);
            System.IO.File.WriteAllText(Path.Combine(artifactDirectory, $"{value}.txt"), value);
            return Task.FromResult<string?>(value);
        }
    }

    [CacheInputs("mode-input.txt")]
    [ProducesArtifact("executable", "run.sh")]
    private sealed class ExecutableArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var path = Path.Combine(WorkingDirectory, "run.sh");
            System.IO.File.WriteAllText(path, "#!/bin/sh\nexit 0\n");
            if (!OperatingSystem.IsWindows())
            {
                System.IO.File.SetUnixFileMode(
                    path,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute
                    | UnixFileMode.GroupRead
                    | UnixFileMode.GroupExecute);
            }

            return Task.FromResult<string?>("result");
        }
    }

    [CacheInputs("symlink-input.txt")]
    [ProducesArtifact("symlink-artifacts", "symlink-artifacts")]
    private sealed class SymbolicLinkArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var artifactDirectory = Directory.CreateDirectory(
                Path.Combine(WorkingDirectory, "symlink-artifacts"));
            System.IO.File.WriteAllText(
                Path.Combine(artifactDirectory.FullName, "tool-v2"),
                "version two");
            System.IO.File.CreateSymbolicLink(
                Path.Combine(artifactDirectory.FullName, "tool"),
                "tool-v2");
            return Task.FromResult<string?>("result");
        }
    }

    [CacheInputs("directory-symlink-input.txt")]
    [ProducesArtifact("directory-symlink-artifacts", "directory-symlink-artifacts")]
    private sealed class DirectorySymbolicLinkArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var artifactDirectory = Directory.CreateDirectory(
                Path.Combine(WorkingDirectory, "directory-symlink-artifacts"));
            var versionDirectory = Directory.CreateDirectory(
                Path.Combine(artifactDirectory.FullName, "version-2"));
            System.IO.File.WriteAllText(
                Path.Combine(versionDirectory.FullName, "payload.txt"),
                "version two");
            Directory.CreateSymbolicLink(
                Path.Combine(artifactDirectory.FullName, "current"),
                "version-2");
            if (!OperatingSystem.IsWindows())
            {
                System.IO.File.SetUnixFileMode(
                    artifactDirectory.FullName,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserExecute
                    | UnixFileMode.GroupRead
                    | UnixFileMode.GroupExecute);
            }

            return Task.FromResult<string?>("result");
        }
    }

    [CacheInputs("optional-artifact-input.txt")]
    [ProducesArtifact("optional-artifact", "optional-artifact")]
    private sealed class OptionalArtifactModule : Module<string>
    {
        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            return Task.FromResult<string?>("result");
        }
    }

    [ProducesArtifact("skippable-output", "skippable-output.txt")]
    private sealed class SkippableCachedModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static bool ShouldSkip;

        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            System.IO.File.WriteAllText(
                Path.Combine(WorkingDirectory, "skippable-output.txt"),
                "created");
            return Task.FromResult<string?>("result");
        }

        protected override ModularPipelines.Configuration.ModuleConfiguration Configure() =>
            ModularPipelines.Configuration.ModuleConfiguration.Create()
                .WithCacheKeyPart("skippable-v1")
                .WithSkipWhen(_ => ShouldSkip
                    ? SkipDecision.Skip("gate closed")
                    : SkipDecision.DoNotSkip)
                .Build();
    }

    [CacheInputs("empty-directory-input.txt")]
    [ProducesArtifact("empty-tree", "empty-tree")]
    private sealed class EmptyDirectoryArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var nestedDirectory = Directory.CreateDirectory(
                Path.Combine(WorkingDirectory, "empty-tree", "nested-empty"));
            if (!OperatingSystem.IsWindows())
            {
                System.IO.File.SetUnixFileMode(
                    nestedDirectory.FullName,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute
                    | UnixFileMode.GroupRead
                    | UnixFileMode.GroupWrite
                    | UnixFileMode.GroupExecute);
            }

            return Task.FromResult<string?>("result");
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
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task FluentSkipConditionTakesPrecedenceOverCacheHit()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-skip-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        SkippableCachedModule.WorkingDirectory = temporaryDirectory;
        SkippableCachedModule.ShouldSkip = false;
        SkippableCachedModule.ExecutionCount = 0;

        try
        {
            var firstResult = await RunSkippableCachePipelineAsync(temporaryDirectory);
            var outputPath = Path.Combine(temporaryDirectory, "skippable-output.txt");
            System.IO.File.Delete(outputPath);
            SkippableCachedModule.ShouldSkip = true;

            var secondResult = await RunSkippableCachePipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(firstResult.ModuleStatus).IsEqualTo(Status.Successful);
                await Assert.That(secondResult.ModuleStatus).IsEqualTo(Status.Skipped);
                await Assert.That(secondResult.SkipDecisionOrDefault?.Reason).IsEqualTo("gate closed");
                await Assert.That(SkippableCachedModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(System.IO.File.Exists(outputPath)).IsFalse();
            }
        }
        finally
        {
            SkippableCachedModule.ShouldSkip = false;
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
    public async Task GlobExpansionDoesNotExcludeWorkingTreeWhenCacheIsAncestor()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-ancestor-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        var workingDirectory = Path.Combine(cacheDirectory, "job");
        Directory.CreateDirectory(workingDirectory);

        try
        {
            var input = Path.Combine(workingDirectory, "input.txt");
            await System.IO.File.WriteAllTextAsync(input, "input");

            var files = ModuleCacheFileResolver.ResolveFiles(
                workingDirectory,
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
    public async Task GlobExpansionTraversesSymbolicLinkedWorkingDirectoryRoot()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-linked-root-{Guid.NewGuid():N}");
        var targetDirectory = Path.Combine(temporaryDirectory, "target");
        var workingDirectory = Path.Combine(temporaryDirectory, "working");
        Directory.CreateDirectory(targetDirectory);

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(targetDirectory, "input.txt"),
                "input");
            Directory.CreateSymbolicLink(workingDirectory, targetDirectory);

            var files = ModuleCacheFileResolver.ResolveFiles(
                workingDirectory,
                ["**/*"],
                maximumFiles: 10);

            await Assert.That(files.Select(path => Path.GetFileName(path)!))
                .IsEquivalentTo(new[] { "input.txt" });
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task GlobExpansionIsCaseInsensitiveOnWindows()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-case-{Guid.NewGuid():N}");
        var sourceDirectory = Path.Combine(temporaryDirectory, "src");
        Directory.CreateDirectory(sourceDirectory);

        try
        {
            var sourceFile = Path.Combine(sourceDirectory, "App.cs");
            await System.IO.File.WriteAllTextAsync(sourceFile, "source");

            var files = ModuleCacheFileResolver.ResolveFiles(
                temporaryDirectory,
                ["SRC/**/*.CS"],
                maximumFiles: 10);

            await Assert.That(files).IsEquivalentTo(new[] { sourceFile });
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ArtifactExpansionDoesNotTraverseDirectorySymbolicLinks()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-directory-link-{Guid.NewGuid():N}");
        var workingDirectory = Path.Combine(temporaryDirectory, "working");
        var artifactDirectory = Path.Combine(workingDirectory, "artifacts");
        var externalDirectory = Path.Combine(temporaryDirectory, "external");
        Directory.CreateDirectory(artifactDirectory);
        Directory.CreateDirectory(externalDirectory);

        try
        {
            var localFile = Path.Combine(artifactDirectory, "local.txt");
            await System.IO.File.WriteAllTextAsync(localFile, "local");
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(externalDirectory, "external.txt"),
                "external");
            Directory.CreateSymbolicLink(
                Path.Combine(artifactDirectory, "external-link"),
                externalDirectory);

            var files = ModuleCacheFileResolver.ResolveFiles(
                workingDirectory,
                ["artifacts"],
                maximumFiles: 10);
            var exactLinkedFile = ModuleCacheFileResolver.ResolveFiles(
                workingDirectory,
                ["artifacts/external-link/external.txt"],
                maximumFiles: 10);
            var directories = ModuleCacheFileResolver.ResolveDirectories(
                workingDirectory,
                ["artifacts"],
                maximumDirectories: 10);
            var directoryLinks = ModuleCacheFileResolver.ResolveDirectoryLinks(
                workingDirectory,
                ["artifacts"],
                maximumLinks: 10);

            using (Assert.Multiple())
            {
                await Assert.That(files).IsEquivalentTo(new[] { localFile });
                await Assert.That(exactLinkedFile).IsEmpty();
                await Assert.That(directories).IsEquivalentTo(new[] { artifactDirectory });
                await Assert.That(directoryLinks).IsEquivalentTo(
                    new[] { Path.Combine(artifactDirectory, "external-link") });
            }
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

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheCapturesArtifactsAfterAfterExecuteHook()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-after-hook-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        AfterHookArtifactModule.WorkingDirectory = temporaryDirectory;
        AfterHookArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "hook-input.txt"),
                "input");
            var firstStatus = await RunAfterHookArtifactPipelineAsync(temporaryDirectory);
            var outputPath = Path.Combine(temporaryDirectory, "hook-output.txt");
            System.IO.File.Delete(outputPath);
            var secondStatus = await RunAfterHookArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(firstStatus).IsEqualTo(Status.Successful);
                await Assert.That(secondStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(AfterHookArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(await System.IO.File.ReadAllTextAsync(outputPath))
                    .IsEqualTo("after-hook");
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task TransformedResultIsConsistentForFreshAndCachedExecution()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-transformed-result-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        ResultTransformingCachedModule.ExecutionCount = 0;

        try
        {
            var first = await RunTransformedResultPipelineAsync(temporaryDirectory);
            var second = await RunTransformedResultPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(first.ModuleResult.ModuleStatus).IsEqualTo(Status.Successful);
                await Assert.That(second.ModuleResult.ModuleStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(first.ModuleResult.ValueOrDefault).IsEqualTo("transformed");
                await Assert.That(second.ModuleResult.ValueOrDefault).IsEqualTo("transformed");
                await Assert.That(first.DependentValue).IsEqualTo("transformed");
                await Assert.That(second.DependentValue).IsEqualTo("transformed");
                await Assert.That(ResultTransformingCachedModule.ExecutionCount).IsEqualTo(1);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRemovesArtifactsAbsentFromSelectedEntry()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-artifact-set-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            var inputPath = Path.Combine(temporaryDirectory, "set-input.txt");
            var artifactDirectory = Path.Combine(temporaryDirectory, "artifact-set");
            await System.IO.File.WriteAllTextAsync(inputPath, "a");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            Directory.Delete(artifactDirectory, recursive: true);
            await System.IO.File.WriteAllTextAsync(inputPath, "b");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            await System.IO.File.WriteAllTextAsync(inputPath, "a");
            var restoredStatus = await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
                await Assert.That(System.IO.File.Exists(Path.Combine(artifactDirectory, "a.txt")))
                    .IsTrue();
                await Assert.That(System.IO.File.Exists(Path.Combine(artifactDirectory, "b.txt")))
                    .IsFalse();
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreReplacesStaleDirectorySymbolicLink()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-stale-directory-link-{Guid.NewGuid():N}");
        var externalDirectory = Path.Combine(temporaryDirectory, "external");
        Directory.CreateDirectory(temporaryDirectory);
        Directory.CreateDirectory(externalDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            var inputPath = Path.Combine(temporaryDirectory, "set-input.txt");
            var artifactDirectory = Path.Combine(temporaryDirectory, "artifact-set");
            var externalArtifact = Path.Combine(externalDirectory, "a.txt");
            var externalSentinel = Path.Combine(externalDirectory, "sentinel.txt");
            await System.IO.File.WriteAllTextAsync(inputPath, "a");
            await System.IO.File.WriteAllTextAsync(externalSentinel, "untouched");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            Directory.Delete(artifactDirectory, recursive: true);
            Directory.CreateSymbolicLink(artifactDirectory, externalDirectory);
            var restoredStatus = await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(new DirectoryInfo(artifactDirectory).LinkTarget).IsNull();
                await Assert.That(await System.IO.File.ReadAllTextAsync(
                        Path.Combine(artifactDirectory, "a.txt")))
                    .IsEqualTo("a");
                await Assert.That(System.IO.File.Exists(externalArtifact)).IsFalse();
                await Assert.That(await System.IO.File.ReadAllTextAsync(externalSentinel))
                    .IsEqualTo("untouched");
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreReplacesDanglingArtifactSymbolicLink()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-dangling-link-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        CachedModule.WorkingDirectory = temporaryDirectory;
        CachedModule.ExecutionCount = 0;

        try
        {
            var inputPath = Path.Combine(temporaryDirectory, "input.txt");
            var outputPath = Path.Combine(temporaryDirectory, "output.txt");
            var externalPath = Path.Combine(temporaryDirectory, "external", "escaped.txt");
            await System.IO.File.WriteAllTextAsync(inputPath, "input");
            await RunPipelineAsync(temporaryDirectory, Path.Combine(temporaryDirectory, "cache"));

            System.IO.File.Delete(outputPath);
            System.IO.File.CreateSymbolicLink(outputPath, externalPath);
            var restoredStatus = await RunPipelineAsync(
                temporaryDirectory,
                Path.Combine(temporaryDirectory, "cache"));

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(CachedModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(new FileInfo(outputPath).LinkTarget).IsNull();
                await Assert.That(await System.IO.File.ReadAllTextAsync(outputPath))
                    .IsEqualTo("output:input");
                await Assert.That(System.IO.File.Exists(externalPath)).IsFalse();
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreClearsExactArtifactLinkAbsentFromSnapshot()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-absent-artifact-link-{Guid.NewGuid():N}");
        var externalDirectory = Path.Combine(temporaryDirectory, "external");
        Directory.CreateDirectory(temporaryDirectory);
        Directory.CreateDirectory(externalDirectory);
        OptionalArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "optional-artifact-input.txt"),
                "input");
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(externalDirectory, "sentinel.txt"),
                "untouched");
            await RunOptionalArtifactPipelineAsync(temporaryDirectory);

            var artifactPath = Path.Combine(temporaryDirectory, "optional-artifact");
            Directory.CreateSymbolicLink(artifactPath, externalDirectory);
            var restoredStatus = await RunOptionalArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(OptionalArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(Directory.Exists(artifactPath)).IsFalse();
                await Assert.That(await System.IO.File.ReadAllTextAsync(
                        Path.Combine(externalDirectory, "sentinel.txt")))
                    .IsEqualTo("untouched");
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestorePreservesUnixExecutableMode()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-unix-mode-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        ExecutableArtifactModule.WorkingDirectory = temporaryDirectory;
        ExecutableArtifactModule.ExecutionCount = 0;
        var expectedMode =
            UnixFileMode.UserRead
            | UnixFileMode.UserWrite
            | UnixFileMode.UserExecute
            | UnixFileMode.GroupRead
            | UnixFileMode.GroupExecute;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "mode-input.txt"),
                "input");
            await RunExecutableArtifactPipelineAsync(temporaryDirectory);
            var artifactPath = Path.Combine(temporaryDirectory, "run.sh");
            System.IO.File.Delete(artifactPath);
            var restoredStatus = await RunExecutableArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(ExecutableArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(System.IO.File.GetUnixFileMode(artifactPath))
                    .IsEqualTo(expectedMode);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestorePreservesUnixSymbolicLinks()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-symlink-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        SymbolicLinkArtifactModule.WorkingDirectory = temporaryDirectory;
        SymbolicLinkArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "symlink-input.txt"),
                "input");
            await RunSymbolicLinkArtifactPipelineAsync(temporaryDirectory);
            var artifactDirectory = Path.Combine(temporaryDirectory, "symlink-artifacts");
            Directory.Delete(artifactDirectory, recursive: true);

            var restoredStatus = await RunSymbolicLinkArtifactPipelineAsync(temporaryDirectory);
            var link = new FileInfo(Path.Combine(artifactDirectory, "tool"));

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(SymbolicLinkArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(link.LinkTarget).IsEqualTo("tool-v2");
                await Assert.That(await System.IO.File.ReadAllTextAsync(link.FullName))
                    .IsEqualTo("version two");
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreCreatesDirectoryLinksBeforeReadOnlyDirectoryModes()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-directory-symlink-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        DirectorySymbolicLinkArtifactModule.WorkingDirectory = temporaryDirectory;
        DirectorySymbolicLinkArtifactModule.ExecutionCount = 0;
        var artifactDirectory = Path.Combine(
            temporaryDirectory,
            "directory-symlink-artifacts");

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "directory-symlink-input.txt"),
                "input");
            await RunDirectorySymbolicLinkArtifactPipelineAsync(temporaryDirectory);
            System.IO.File.SetUnixFileMode(
                artifactDirectory,
                UnixFileMode.UserRead
                | UnixFileMode.UserWrite
                | UnixFileMode.UserExecute);
            Directory.Delete(artifactDirectory, recursive: true);

            var restoredStatus =
                await RunDirectorySymbolicLinkArtifactPipelineAsync(temporaryDirectory);
            var directoryLink = new DirectoryInfo(
                Path.Combine(artifactDirectory, "current"));

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(DirectorySymbolicLinkArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(directoryLink.LinkTarget).IsEqualTo("version-2");
                await Assert.That(await System.IO.File.ReadAllTextAsync(
                        Path.Combine(directoryLink.FullName, "payload.txt")))
                    .IsEqualTo("version two");
                await Assert.That(System.IO.File.GetUnixFileMode(artifactDirectory))
                    .IsEqualTo(
                        UnixFileMode.UserRead
                        | UnixFileMode.UserExecute
                        | UnixFileMode.GroupRead
                        | UnixFileMode.GroupExecute);
            }
        }
        finally
        {
            if (Directory.Exists(artifactDirectory))
            {
                System.IO.File.SetUnixFileMode(
                    artifactDirectory,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute);
            }

            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestorePreservesEmptyArtifactDirectories()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-empty-directory-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        EmptyDirectoryArtifactModule.WorkingDirectory = temporaryDirectory;
        EmptyDirectoryArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "empty-directory-input.txt"),
                "input");
            await RunEmptyDirectoryArtifactPipelineAsync(temporaryDirectory);
            var artifactDirectory = Path.Combine(temporaryDirectory, "empty-tree");
            Directory.Delete(artifactDirectory, recursive: true);

            var restoredStatus = await RunEmptyDirectoryArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(Status.UsedHistory);
                await Assert.That(EmptyDirectoryArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(Directory.Exists(artifactDirectory)).IsTrue();
                await Assert.That(Directory.Exists(Path.Combine(artifactDirectory, "nested-empty")))
                    .IsTrue();
                await Assert.That(Directory.EnumerateFiles(artifactDirectory, "*", SearchOption.AllDirectories))
                    .IsEmpty();
                if (!OperatingSystem.IsWindows())
                {
                    await Assert.That(System.IO.File.GetUnixFileMode(
                            Path.Combine(artifactDirectory, "nested-empty")))
                        .IsEqualTo(
                            UnixFileMode.UserRead
                            | UnixFileMode.UserWrite
                            | UnixFileMode.UserExecute
                            | UnixFileMode.GroupRead
                            | UnixFileMode.GroupWrite
                            | UnixFileMode.GroupExecute);
                }
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

    private static async Task<Status> RunAfterHookArtifactPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<AfterHookArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(AfterHookArtifactModule))!
            .ModuleStatus;
    }

    private static async Task<(ModuleResult<string> ModuleResult, string? DependentValue)>
        RunTransformedResultPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<ResultTransformingCachedModule>()
            .AddModule<TransformedResultDependentModule>()
            .BuildAsync();

        await host.RunAsync();
        var module = host.Services
            .GetServices<IModule>()
            .OfType<ResultTransformingCachedModule>()
            .Single();
        var moduleResult = await module;
        var dependentResult = host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(TransformedResultDependentModule))!;
        return (moduleResult, dependentResult.ValueOrDefault as string);
    }

    private static async Task<Status> RunVaryingArtifactSetPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<VaryingArtifactSetModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(VaryingArtifactSetModule))!
            .ModuleStatus;
    }

    private static async Task<Status> RunExecutableArtifactPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<ExecutableArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(ExecutableArtifactModule))!
            .ModuleStatus;
    }

    private static async Task<Status> RunSymbolicLinkArtifactPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<SymbolicLinkArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(SymbolicLinkArtifactModule))!
            .ModuleStatus;
    }

    private static async Task<Status> RunDirectorySymbolicLinkArtifactPipelineAsync(
        string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<DirectorySymbolicLinkArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(DirectorySymbolicLinkArtifactModule))!
            .ModuleStatus;
    }

    private static async Task<Status> RunOptionalArtifactPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<OptionalArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(OptionalArtifactModule))!
            .ModuleStatus;
    }

    private static async Task<IModuleResult> RunSkippableCachePipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<SkippableCachedModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(SkippableCachedModule))!;
    }

    private static async Task<Status> RunEmptyDirectoryArtifactPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineHostBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<EmptyDirectoryArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(EmptyDirectoryArtifactModule))!
            .ModuleStatus;
    }
}
