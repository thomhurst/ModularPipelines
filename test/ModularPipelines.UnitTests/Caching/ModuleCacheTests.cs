using System.IO.Compression;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.UnitTests.Caching;

public class ModuleCacheTests
{
    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Debug;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            Messages.Add(formatter(state, exception));
    }

    private sealed class TrackingProgressDisplay : IProgressDisplay
    {
        public bool? LastCompletionWasSuccessful { get; private set; }

        public Task RunAsync(
            OrganizedModules organizedModules,
            CancellationToken cancellationToken) => Task.CompletedTask;

        public void OnModuleStarted(ModuleState moduleState, TimeSpan estimatedDuration)
        {
        }

        public void OnModuleCompleted(ModuleState moduleState, bool isSuccessful) =>
            LastCompletionWasSuccessful = isSuccessful;

        public void OnModuleSkipped(ModuleState moduleState)
        {
        }

        public void OnSubModuleCreated(
            IModule parentModule,
            SubModuleBase subModule,
            TimeSpan estimatedDuration)
        {
        }

        public void OnSubModuleCompleted(SubModuleBase subModule, bool isSuccessful)
        {
        }
    }

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

        public static int SkippedHookCount;

        public static int CachedResultHookCount;

        public static ModuleStatus? CachedResultHookStatus;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var value = System.IO.File.ReadAllText(Path.Combine(WorkingDirectory, "input.txt"));
            System.IO.File.WriteAllText(Path.Combine(WorkingDirectory, "output.txt"), $"output:{value}");
            return Task.FromResult<string>($"result:{value}");
        }

        protected override Task OnSkippedAsync(
            IModuleContext context,
            SkipDecision skipDecision,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref SkippedHookCount);
            return Task.CompletedTask;
        }

        protected override Task OnCachedResultAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref CachedResultHookCount);
            CachedResultHookStatus = result.Status;
            return Task.CompletedTask;
        }
    }

    private sealed class DependencyModule : Module<string>
    {
        public static string Value { get; set; } = string.Empty;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>(Value);
    }

    [CacheInputs("unused.txt")]
    private sealed class StableAssemblyVersionKeyModule : Module<string>
    {
        public const string AssemblyVersionKey = "stable-build-module-v3";
        public const string KeyPart = "configuration-value";

        protected override void Configure(ModularPipelines.Configuration.ModuleConfigurationBuilder module) => module
                .WithCacheKeyPart(KeyPart)
                .WithCacheAssemblyVersionKey(AssemblyVersionKey);

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult("stable");
    }

    private sealed class RuntimeTypedDependencyModule : Module<object>
    {
        public static object Value { get; set; } = 1;

        protected internal override Task<object> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<object>(Value);
    }

    [ModularPipelines.Attributes.DependsOn<RuntimeTypedDependencyModule>]
    private sealed class RuntimeTypedCachedDependentModule : Module<string>
    {
        public static int ExecutionCount;

        protected internal override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var dependency = await context.GetModule<RuntimeTypedDependencyModule>();
            return dependency.Value.GetType().FullName!;
        }

        protected override void Configure(ModularPipelines.Configuration.ModuleConfigurationBuilder module) => module
                .WithCacheKeyPart("runtime-typed-dependency-v1");
    }

    private sealed class RuntimeTypedCachedResultModule : Module<object>
    {
        public static int ExecutionCount;

        protected internal override Task<object> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            return Task.FromResult<object>(1);
        }

        protected override void Configure(ModularPipelines.Configuration.ModuleConfigurationBuilder module) => module
                .WithCacheKeyPart("runtime-typed-result-v1");
    }

    private sealed class EnvironmentCachedModule : Module<string?>
    {
        public const string EnvironmentVariableName =
            "MODULAR_PIPELINES_CACHE_NULL_SENTINEL_TEST";

        public static int ExecutionCount;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            return Task.FromResult(Environment.GetEnvironmentVariable(EnvironmentVariableName));
        }

        protected override void Configure(ModularPipelines.Configuration.ModuleConfigurationBuilder module) => module
                .WithCacheEnvironmentVariable(EnvironmentVariableName);
    }

    private sealed class UncachedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>("uncached");
    }

    [CacheInputs("mutable.txt")]
    private sealed class InputMutatingModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override async Task<string> ExecuteAsync(
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

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            System.IO.File.WriteAllText(
                Path.Combine(WorkingDirectory, "hook-output.txt"),
                "before-hook");
            return Task.FromResult<string>("result");
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

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            return Task.FromResult<string>("original");
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

        protected override void Configure(ModularPipelines.Configuration.ModuleConfigurationBuilder module) => module
                .WithCacheKeyPart("result-transform-v1");
    }

    [ModularPipelines.Attributes.DependsOn<ResultTransformingCachedModule>]
    private sealed class TransformedResultDependentModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var dependency = await context.GetModule<ResultTransformingCachedModule>();
            return dependency.Value;
        }
    }

    [CacheInputs("set-input.txt")]
    [ProducesArtifact("artifact-set", "artifact-set")]
    private sealed class VaryingArtifactSetModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        public static bool SawStaleArtifact;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var value = System.IO.File.ReadAllText(
                Path.Combine(WorkingDirectory, "set-input.txt"));
            var artifactDirectory = Path.Combine(WorkingDirectory, "artifact-set");
            SawStaleArtifact = System.IO.File.Exists(
                Path.Combine(artifactDirectory, "stale.txt"));
            Directory.CreateDirectory(artifactDirectory);
            System.IO.File.WriteAllText(Path.Combine(artifactDirectory, $"{value}.txt"), value);
            return Task.FromResult<string>(value);
        }
    }

    [CacheInputs("multiple-artifacts-input.txt")]
    [ProducesArtifact("multiple-artifacts", "multiple artifacts")]
    private sealed class MultipleArtifactFilesModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var artifactDirectory = Path.Combine(WorkingDirectory, "multiple-artifacts");
            Directory.CreateDirectory(artifactDirectory);
            System.IO.File.WriteAllText(Path.Combine(artifactDirectory, "first.txt"), "first");
            System.IO.File.WriteAllText(Path.Combine(artifactDirectory, "second.txt"), "second");
            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("lookup-failure-inputs/*")]
    private sealed class LookupFailureInputMutatingModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            foreach (var input in Directory.EnumerateFiles(
                         Path.Combine(WorkingDirectory, "lookup-failure-inputs")))
            {
                System.IO.File.Delete(input);
            }

            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("glob-link-input.txt")]
    [ProducesArtifact("glob-links", "glob-links/**/*")]
    private sealed class GlobOptionalArtifactModule : Module<string>
    {
        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("shallow-glob-input.txt")]
    [ProducesArtifact("shallow-glob", "shallow-glob/*")]
    private sealed class ShallowGlobArtifactModule : Module<string>
    {
        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("mode-input.txt")]
    [ProducesArtifact("executable", "run.sh")]
    private sealed class ExecutableArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
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

            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("file-parent-input.txt")]
    [ProducesArtifact("file-only", "read-only-parent/tool")]
    private sealed class ReadOnlyFileParentArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var parent = Directory.CreateDirectory(
                Path.Combine(WorkingDirectory, "read-only-parent"));
            if (!OperatingSystem.IsWindows())
            {
                System.IO.File.SetUnixFileMode(
                    parent.FullName,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute);
            }

            System.IO.File.WriteAllText(
                Path.Combine(parent.FullName, "tool"),
                "artifact");
            if (!OperatingSystem.IsWindows())
            {
                System.IO.File.SetUnixFileMode(
                    parent.FullName,
                    UnixFileMode.UserRead | UnixFileMode.UserExecute);
            }

            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("symlink-input.txt")]
    [ProducesArtifact("symlink-artifacts", "symlink-artifacts")]
    private sealed class SymbolicLinkArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
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
            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("directory-symlink-input.txt")]
    [ProducesArtifact("directory-symlink-artifacts", "directory-symlink-artifacts")]
    private sealed class DirectorySymbolicLinkArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
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

            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("optional-artifact-input.txt")]
    [ProducesArtifact("optional-artifact", "optional-artifact")]
    private sealed class OptionalArtifactModule : Module<string>
    {
        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("nested-optional-artifact-input.txt")]
    [ProducesArtifact("nested-optional-artifact", "read-only-optional/optional.txt")]
    private sealed class NestedOptionalArtifactModule : Module<string>
    {
        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("dangling-artifact-input.txt")]
    [ProducesArtifact("dangling-artifact", "dangling-output")]
    private sealed class DanglingSymbolicLinkArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            System.IO.File.CreateSymbolicLink(
                Path.Combine(WorkingDirectory, "dangling-output"),
                "missing-target");
            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("working-directory-input.txt")]
    [ProducesArtifact("working-directory", ".")]
    private sealed class WorkingDirectoryArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        public static UnixFileMode? ModeBeforeExecution;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            if (!OperatingSystem.IsWindows())
            {
                ModeBeforeExecution = System.IO.File.GetUnixFileMode(WorkingDirectory);
                System.IO.File.SetUnixFileMode(
                    WorkingDirectory,
                    ModeBeforeExecution.Value
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute);
            }

            System.IO.File.WriteAllText(
                Path.Combine(WorkingDirectory, "root-artifact.txt"),
                "artifact");
            if (!OperatingSystem.IsWindows())
            {
                System.IO.File.SetUnixFileMode(
                    WorkingDirectory,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserExecute
                    | UnixFileMode.GroupRead
                    | UnixFileMode.GroupExecute
                    | UnixFileMode.OtherRead
                    | UnixFileMode.OtherExecute);
            }

            return Task.FromResult<string>("result");
        }
    }

    [CacheInputs("dangling-directory-link-input.txt")]
    [ProducesArtifact("dangling-directory-link", "dangling-directory-link")]
    private sealed class DanglingDirectorySymbolicLinkArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            Directory.CreateSymbolicLink(
                Path.Combine(WorkingDirectory, "dangling-directory-link"),
                "missing-directory");
            return Task.FromResult<string>("result");
        }
    }

    [ProducesArtifact("skippable-output", "skippable-output.txt")]
    private sealed class SkippableCachedModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static bool ShouldSkip;

        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            System.IO.File.WriteAllText(
                Path.Combine(WorkingDirectory, "skippable-output.txt"),
                "created");
            return Task.FromResult<string>("result");
        }

        protected override void Configure(ModularPipelines.Configuration.ModuleConfigurationBuilder module) => module
                .WithCacheKeyPart("skippable-v1")
                .WithSkipWhen(_ => ShouldSkip
                    ? SkipDecision.Skip("gate closed")
                    : SkipDecision.DoNotSkip);
    }

    [CacheInputs("empty-directory-input.txt")]
    [ProducesArtifact("empty-tree", "empty-tree")]
    private sealed class EmptyDirectoryArtifactModule : Module<string>
    {
        public static string WorkingDirectory { get; set; } = string.Empty;

        public static int ExecutionCount;

        protected internal override Task<string> ExecuteAsync(
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

            return Task.FromResult<string>("result");
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    private sealed class CachedDependentModule : Module<string>
    {
        public static int ExecutionCount;

        protected internal override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            var dependency = await context.GetModule<DependencyModule>();
            return $"dependent:{dependency.ValueOrDefault}";
        }

        protected override void Configure(ModularPipelines.Configuration.ModuleConfigurationBuilder module) => module
                .WithCacheKeyPart("dependent-v1");
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
        CachedModule.SkippedHookCount = 0;
        CachedModule.CachedResultHookCount = 0;
        CachedModule.CachedResultHookStatus = null;

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
                await Assert.That(firstStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(secondStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(CachedModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(CachedModule.SkippedHookCount).IsEqualTo(0);
                await Assert.That(CachedModule.CachedResultHookCount).IsEqualTo(1);
                await Assert.That(CachedModule.CachedResultHookStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(await System.IO.File.ReadAllTextAsync(outputPath)).IsEqualTo("output:first");
            }

            await System.IO.File.WriteAllTextAsync(inputPath, "changed-value");
            var thirdStatus = await RunPipelineAsync(temporaryDirectory, cacheDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(thirdStatus).IsEqualTo(ModuleStatus.Succeeded);
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
    public async Task DisabledModuleCacheNeitherReadsNorWritesEntries()
    {
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), $"ModularPipelines-cache-disabled-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        CachedModule.WorkingDirectory = temporaryDirectory;
        CachedModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(Path.Combine(temporaryDirectory, "input.txt"), "value");

            var firstStatus = await RunPipelineAsync(temporaryDirectory, cacheDirectory);
            var disabledStatus = await RunPipelineAsync(
                temporaryDirectory,
                cacheDirectory,
                disableModuleCache: true);

            Directory.Delete(cacheDirectory, recursive: true);
            var disabledWithoutEntryStatus = await RunPipelineAsync(
                temporaryDirectory,
                cacheDirectory,
                disableModuleCache: true);

            using (Assert.Multiple())
            {
                await Assert.That(firstStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(disabledStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(disabledWithoutEntryStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(CachedModule.ExecutionCount).IsEqualTo(3);
                await Assert.That(Directory.Exists(cacheDirectory)).IsFalse();
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
                await Assert.That(firstResult.Status).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(secondResult.Status).IsEqualTo(ModuleStatus.Skipped);
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
            var firstHash = (await hasher.HashAsync(
                [path],
                temporaryDirectory,
                CancellationToken.None))[path];

            await System.IO.File.WriteAllTextAsync(path, "other");
            System.IO.File.SetLastWriteTimeUtc(path, timestamp);
            var secondHash = (await hasher.HashAsync(
                [path],
                temporaryDirectory,
                CancellationToken.None))[path];

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
                maximumFiles: 10,
                rejectLinkedPaths: true);

            await Assert.That(files.Select(path => Path.GetFileName(path)!))
                .IsEquivalentTo(new[] { "input.txt" });
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task GlobExpansionIsCaseInsensitiveOnDefaultDesktopFileSystems()
    {
        if (!OperatingSystem.IsWindows() && !OperatingSystem.IsMacOS())
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
            var builder = TestPipelineBuilder.Create();
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
        var configuration = new ModularPipelines.Configuration.ModuleConfigurationBuilder()
            .WithCacheKeyPart("configuration=v1")
            .WithCacheEnvironmentVariable("CI")
            .WithCacheAssemblyVersionKey("build-module-v3")
            .Build();

        using (Assert.Multiple())
        {
            await Assert.That(configuration.CacheEnabled).IsTrue();
            await Assert.That(configuration.CacheKeyParts).IsEquivalentTo(new[] { "configuration=v1" });
            await Assert.That(configuration.CacheEnvironmentVariables).IsEquivalentTo(new[] { "CI" });
            await Assert.That(configuration.CacheAssemblyVersionKey).IsEqualTo("build-module-v3");
        }
    }

    [Test]
    public async Task CacheAssemblyVersionKeyRejectsWhitespace()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new ModularPipelines.Configuration.ModuleConfigurationBuilder()
                .WithCacheAssemblyVersionKey(" "));

        await Assert.That(exception.ParamName).IsEqualTo("value");
    }

    [Test]
    public async Task CacheMissLogsProtectedFingerprintComponents()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-diagnostics-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        var logger = new RecordingLogger<ModuleCacheResultRepository>();

        try
        {
            using var builder = Pipeline.CreateBuilder();
            builder.AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = temporaryDirectory;
                options.CacheDirectory = Path.Combine(temporaryDirectory, "cache");
            });
            builder.Services.AddSingleton<ILogger<ModuleCacheResultRepository>>(logger);
            builder.AddModule<StableAssemblyVersionKeyModule>();

            await builder.RunAsync();

            var miss = logger.Messages.Single(message => message.Contains("Module cache miss"));
            using (Assert.Multiple())
            {
                await Assert.That(miss).Contains("module-version-override=");
                await Assert.That(miss).Contains("key-part=");
                await Assert.That(miss).Contains("sha256:");
                await Assert.That(miss).DoesNotContain(StableAssemblyVersionKeyModule.AssemblyVersionKey);
                await Assert.That(miss).DoesNotContain(StableAssemblyVersionKeyModule.KeyPart);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
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
                await Assert.That(firstStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(secondStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(thirdStatus).IsEqualTo(ModuleStatus.Succeeded);
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
                await Assert.That(firstStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(secondStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(thirdStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
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
                await Assert.That(firstStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(secondStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
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
                await Assert.That(first.ModuleResult.Status).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(second.ModuleResult.Status).IsEqualTo(ModuleStatus.RestoredFromCache);
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
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
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
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
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
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
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
    public async Task DependencyRuntimeTypeInvalidatesDependentFingerprint()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-dependency-type-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        RuntimeTypedDependencyModule.Value = 1;
        RuntimeTypedCachedDependentModule.ExecutionCount = 0;

        try
        {
            var firstStatus = await RunRuntimeTypedDependencyPipelineAsync(temporaryDirectory);
            var secondStatus = await RunRuntimeTypedDependencyPipelineAsync(temporaryDirectory);
            RuntimeTypedDependencyModule.Value = 1L;
            var thirdStatus = await RunRuntimeTypedDependencyPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(firstStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(secondStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(thirdStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(RuntimeTypedCachedDependentModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestorePreservesResultRuntimeType()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-result-type-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        RuntimeTypedCachedResultModule.ExecutionCount = 0;

        try
        {
            var first = await RunRuntimeTypedResultPipelineAsync(temporaryDirectory);
            var second = await RunRuntimeTypedResultPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(first.Status).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(second.Status).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(second.Value).IsTypeOf<int>();
                await Assert.That(second.Value).IsEqualTo(1);
                await Assert.That(RuntimeTypedCachedResultModule.ExecutionCount).IsEqualTo(1);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel("ProcessEnvironment")]
    public async Task UnsetAndLiteralNullEnvironmentValuesHaveDifferentFingerprints()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-environment-presence-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        var previousValue = Environment.GetEnvironmentVariable(
            EnvironmentCachedModule.EnvironmentVariableName);
        EnvironmentCachedModule.ExecutionCount = 0;

        try
        {
            Environment.SetEnvironmentVariable(
                EnvironmentCachedModule.EnvironmentVariableName,
                null);
            var firstStatus = await RunEnvironmentPipelineAsync(temporaryDirectory);
            var secondStatus = await RunEnvironmentPipelineAsync(temporaryDirectory);
            Environment.SetEnvironmentVariable(
                EnvironmentCachedModule.EnvironmentVariableName,
                "<null>");
            var thirdStatus = await RunEnvironmentPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(firstStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(secondStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(thirdStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(EnvironmentCachedModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable(
                EnvironmentCachedModule.EnvironmentVariableName,
                previousValue);
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task LookupFingerprintFailureSkipsPostExecutionCacheSave()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-lookup-failure-{Guid.NewGuid():N}");
        var inputDirectory = Path.Combine(temporaryDirectory, "lookup-failure-inputs");
        Directory.CreateDirectory(inputDirectory);
        LookupFailureInputMutatingModule.WorkingDirectory = temporaryDirectory;
        LookupFailureInputMutatingModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(Path.Combine(inputDirectory, "one.txt"), "1");
            await System.IO.File.WriteAllTextAsync(Path.Combine(inputDirectory, "two.txt"), "2");

            var firstStatus = await RunLookupFailurePipelineAsync(temporaryDirectory);
            var secondStatus = await RunLookupFailurePipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(firstStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(secondStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(LookupFailureInputMutatingModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheHitPublishesSuccessfulCompletion()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-progress-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        CachedModule.WorkingDirectory = temporaryDirectory;
        CachedModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "input.txt"),
                "input");
            await RunPipelineAsync(temporaryDirectory, cacheDirectory);

            var completionWasSuccessful = await RunPipelineWithProgressAsync(
                temporaryDirectory,
                cacheDirectory);

            await Assert.That(completionWasSuccessful).IsTrue();
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreReplacesReadOnlyArtifactFilesOnWindows()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-read-only-file-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        var outputPath = Path.Combine(temporaryDirectory, "output.txt");
        Directory.CreateDirectory(temporaryDirectory);
        CachedModule.WorkingDirectory = temporaryDirectory;
        CachedModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "input.txt"),
                "input");
            await RunPipelineAsync(temporaryDirectory, cacheDirectory);
            System.IO.File.SetAttributes(
                outputPath,
                System.IO.File.GetAttributes(outputPath) | FileAttributes.ReadOnly);

            var restoredStatus = await RunPipelineAsync(temporaryDirectory, cacheDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(CachedModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(await System.IO.File.ReadAllTextAsync(outputPath))
                    .IsEqualTo("output:input");
            }
        }
        finally
        {
            if (System.IO.File.Exists(outputPath))
            {
                System.IO.File.SetAttributes(
                    outputPath,
                    System.IO.File.GetAttributes(outputPath) & ~FileAttributes.ReadOnly);
            }

            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task GlobExpansionAndHashingPreserveCaseDistinctFiles()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-case-sensitive-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);

        try
        {
            var upperCaseFile = Path.Combine(temporaryDirectory, "Input.txt");
            var lowerCaseFile = Path.Combine(temporaryDirectory, "input.txt");
            await System.IO.File.WriteAllTextAsync(upperCaseFile, "upper");
            await System.IO.File.WriteAllTextAsync(lowerCaseFile, "lower");
            if (Directory.EnumerateFiles(temporaryDirectory).Count() != 2)
            {
                return;
            }

            var files = ModuleCacheFileResolver.ResolveFiles(
                temporaryDirectory,
                ["*.txt"],
                maximumFiles: 10);
            var hasher = new ModuleCacheFileHasher(
                OptionsFactory.Create(new ModuleCacheOptions()));
            var hashes = await hasher.HashAsync(
                files,
                temporaryDirectory,
                CancellationToken.None);

            using (Assert.Multiple())
            {
                await Assert.That(files).IsEquivalentTo([upperCaseFile, lowerCaseFile]);
                await Assert.That(hashes).Count().IsEqualTo(2);
                await Assert.That(hashes[upperCaseFile])
                    .IsNotEqualTo(hashes[lowerCaseFile]);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task VolumeComparerRejectsCaseDistinctSiblingDirectory()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-containment-{Guid.NewGuid():N}");
        var root = Path.Combine(temporaryDirectory, "work");
        var sibling = Path.Combine(temporaryDirectory, "WORK", "escaped.txt");
        Directory.CreateDirectory(root);

        try
        {
            if (ModuleCacheFileResolver.GetPathComparer(root) == StringComparer.OrdinalIgnoreCase)
            {
                return;
            }

            await Assert.That(ModuleCacheFileResolver.IsWithin(root, sibling)).IsFalse();
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ArtifactScopeRequiresExactDeclarationForWorkingRoot()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-root-scope-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);

        try
        {
            await Assert.That(ModuleCacheFileResolver.IsWithinDeclaredArtifactScope(
                    temporaryDirectory,
                    temporaryDirectory,
                    ["*"]))
                .IsFalse();
            await Assert.That(ModuleCacheFileResolver.IsWithinDeclaredArtifactScope(
                    temporaryDirectory,
                    temporaryDirectory,
                    ["**/*"]))
                .IsFalse();
            await Assert.That(ModuleCacheFileResolver.IsWithinDeclaredArtifactScope(
                    temporaryDirectory,
                    temporaryDirectory,
                    ["."]))
                .IsTrue();
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task InputExpansionRejectsSymbolicLinkedPaths()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-linked-input-{Guid.NewGuid():N}");
        var workingDirectory = Path.Combine(temporaryDirectory, "working");
        var externalDirectory = Path.Combine(temporaryDirectory, "external");
        Directory.CreateDirectory(workingDirectory);
        Directory.CreateDirectory(externalDirectory);

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(externalDirectory, "input.txt"),
                "input");
            Directory.CreateSymbolicLink(
                Path.Combine(workingDirectory, "linked-input"),
                externalDirectory);

            var exactException = Assert.Throws<InvalidOperationException>(() =>
                ModuleCacheFileResolver.ResolveFiles(
                    workingDirectory,
                    ["linked-input"],
                    maximumFiles: 10,
                    rejectLinkedPaths: true));
            var globException = Assert.Throws<InvalidOperationException>(() =>
                ModuleCacheFileResolver.ResolveFiles(
                    workingDirectory,
                    ["**/*.txt"],
                    maximumFiles: 10,
                    rejectLinkedPaths: true));

            using (Assert.Multiple())
            {
                await Assert.That(exactException.Message).Contains("linked-input");
                await Assert.That(globException.Message).Contains("linked-input");
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    public async Task InputExpansionIgnoresSymbolicLinksOutsideGlob()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-unrelated-link-{Guid.NewGuid():N}");
        var workingDirectory = Path.Combine(temporaryDirectory, "working");
        var sourceDirectory = Path.Combine(workingDirectory, "src");
        var externalDirectory = Path.Combine(temporaryDirectory, "external");
        Directory.CreateDirectory(sourceDirectory);
        Directory.CreateDirectory(externalDirectory);

        try
        {
            var sourceFile = Path.Combine(sourceDirectory, "Program.cs");
            await System.IO.File.WriteAllTextAsync(sourceFile, "class Program;");
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(externalDirectory, "logo.png"),
                "logo");
            Directory.CreateSymbolicLink(
                Path.Combine(sourceDirectory, "assets"),
                externalDirectory);

            var files = ModuleCacheFileResolver.ResolveFiles(
                workingDirectory,
                ["src/*.cs"],
                maximumFiles: 10,
                rejectLinkedPaths: true);

            await Assert.That(files).IsEquivalentTo([sourceFile]);
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
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
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
    public async Task CacheRestoreClearsEmptySnapshotArtifactUnderReadOnlyParent()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-empty-read-only-parent-{Guid.NewGuid():N}");
        var artifactParent = Path.Combine(temporaryDirectory, "read-only-optional");
        var artifactPath = Path.Combine(artifactParent, "optional.txt");
        Directory.CreateDirectory(temporaryDirectory);
        NestedOptionalArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "nested-optional-artifact-input.txt"),
                "input");
            await RunNestedOptionalArtifactPipelineAsync(temporaryDirectory);

            Directory.CreateDirectory(artifactParent);
            await System.IO.File.WriteAllTextAsync(artifactPath, "stale");
            System.IO.File.SetUnixFileMode(
                artifactParent,
                UnixFileMode.UserRead | UnixFileMode.UserExecute);

            var restoredStatus =
                await RunNestedOptionalArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(NestedOptionalArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(System.IO.File.Exists(artifactPath)).IsFalse();
                await Assert.That(System.IO.File.GetUnixFileMode(artifactParent))
                    .IsEqualTo(UnixFileMode.UserRead | UnixFileMode.UserExecute);
            }
        }
        finally
        {
            if (Directory.Exists(artifactParent))
            {
                System.IO.File.SetUnixFileMode(
                    artifactParent,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute);
            }

            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestorePreservesExactDanglingSymbolicLink()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-dangling-artifact-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        DanglingSymbolicLinkArtifactModule.WorkingDirectory = temporaryDirectory;
        DanglingSymbolicLinkArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "dangling-artifact-input.txt"),
                "input");
            await RunDanglingSymbolicLinkArtifactPipelineAsync(temporaryDirectory);
            var artifactPath = Path.Combine(temporaryDirectory, "dangling-output");
            System.IO.File.Delete(artifactPath);

            var restoredStatus =
                await RunDanglingSymbolicLinkArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(DanglingSymbolicLinkArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(new FileInfo(artifactPath).LinkTarget)
                    .IsEqualTo("missing-target");
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreSupportsWorkingDirectoryArtifact()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-working-directory-artifact-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        WorkingDirectoryArtifactModule.WorkingDirectory = temporaryDirectory;
        WorkingDirectoryArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "working-directory-input.txt"),
                "input");
            await RunWorkingDirectoryArtifactPipelineAsync(temporaryDirectory);
            var restoredStatus =
                await RunWorkingDirectoryArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(WorkingDirectoryArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(await System.IO.File.ReadAllTextAsync(
                        Path.Combine(temporaryDirectory, "root-artifact.txt")))
                    .IsEqualTo("artifact");
                if (!OperatingSystem.IsWindows())
                {
                    await Assert.That(System.IO.File.GetUnixFileMode(temporaryDirectory))
                        .IsEqualTo(
                            UnixFileMode.UserRead
                            | UnixFileMode.UserExecute
                            | UnixFileMode.GroupRead
                            | UnixFileMode.GroupExecute
                            | UnixFileMode.OtherRead
                            | UnixFileMode.OtherExecute);
                }
            }
        }
        finally
        {
            if (!OperatingSystem.IsWindows() && Directory.Exists(temporaryDirectory))
            {
                System.IO.File.SetUnixFileMode(
                    temporaryDirectory,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute);
            }

            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestorePreservesExactDanglingDirectorySymbolicLink()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-dangling-directory-artifact-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        DanglingDirectorySymbolicLinkArtifactModule.WorkingDirectory = temporaryDirectory;
        DanglingDirectorySymbolicLinkArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "dangling-directory-link-input.txt"),
                "input");
            await RunDanglingDirectorySymbolicLinkArtifactPipelineAsync(temporaryDirectory);
            var artifactPath = Path.Combine(temporaryDirectory, "dangling-directory-link");
            if (OperatingSystem.IsWindows())
            {
                Directory.Delete(artifactPath);
            }
            else
            {
                System.IO.File.Delete(artifactPath);
            }

            var restoredStatus =
                await RunDanglingDirectorySymbolicLinkArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(DanglingDirectorySymbolicLinkArtifactModule.ExecutionCount)
                    .IsEqualTo(1);
                await Assert.That(new DirectoryInfo(artifactPath).LinkTarget)
                    .IsEqualTo("missing-directory");
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreClearsGlobMatchedDirectoryLinkAbsentFromSnapshot()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-glob-link-{Guid.NewGuid():N}");
        var externalDirectory = Path.Combine(temporaryDirectory, "external");
        var externalSentinel = Path.Combine(externalDirectory, "sentinel.txt");
        var artifactDirectory = Path.Combine(temporaryDirectory, "glob-links");
        var staleLink = Path.Combine(artifactDirectory, "stale");
        Directory.CreateDirectory(temporaryDirectory);
        Directory.CreateDirectory(externalDirectory);
        GlobOptionalArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "glob-link-input.txt"),
                "input");
            await System.IO.File.WriteAllTextAsync(externalSentinel, "untouched");
            await RunGlobOptionalArtifactPipelineAsync(temporaryDirectory);

            Directory.CreateDirectory(artifactDirectory);
            Directory.CreateSymbolicLink(staleLink, externalDirectory);
            var restoredStatus =
                await RunGlobOptionalArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(GlobOptionalArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(Directory.Exists(staleLink)).IsFalse();
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
    public async Task CacheRestoreClearsGlobMatchedFileUnderReadOnlyParent()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-glob-read-only-{Guid.NewGuid():N}");
        var artifactDirectory = Path.Combine(temporaryDirectory, "glob-links");
        var staleFile = Path.Combine(artifactDirectory, "stale.txt");
        Directory.CreateDirectory(temporaryDirectory);
        GlobOptionalArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "glob-link-input.txt"),
                "input");
            await RunGlobOptionalArtifactPipelineAsync(temporaryDirectory);

            Directory.CreateDirectory(artifactDirectory);
            await System.IO.File.WriteAllTextAsync(staleFile, "stale");
            System.IO.File.SetUnixFileMode(
                artifactDirectory,
                UnixFileMode.UserRead
                | UnixFileMode.UserExecute
                | UnixFileMode.GroupRead
                | UnixFileMode.GroupExecute
                | UnixFileMode.OtherRead
                | UnixFileMode.OtherExecute);

            var restoredStatus =
                await RunGlobOptionalArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(GlobOptionalArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(System.IO.File.Exists(staleFile)).IsFalse();
                await Assert.That(System.IO.File.GetUnixFileMode(artifactDirectory))
                    .IsEqualTo(
                        UnixFileMode.UserRead
                        | UnixFileMode.UserExecute
                        | UnixFileMode.GroupRead
                        | UnixFileMode.GroupExecute
                        | UnixFileMode.OtherRead
                        | UnixFileMode.OtherExecute);
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
    public async Task FailedCacheRestoreRollsBackPartialArtifacts()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-restore-rollback-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;
        VaryingArtifactSetModule.SawStaleArtifact = false;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            var cacheEntry = Directory.GetFiles(cacheDirectory, "*.zip").Single();
            using (var archive = ZipFile.Open(cacheEntry, ZipArchiveMode.Update))
            {
                await using (var staleEntry = archive
                                 .CreateEntry("artifacts/artifact-set/stale.txt")
                                 .Open())
                {
                    await staleEntry.WriteAsync("stale"u8.ToArray());
                }

                await using (var failingEntry = archive
                                 .CreateEntry("artifacts/blocked/child.txt")
                                 .Open())
                {
                    await failingEntry.WriteAsync("blocked"u8.ToArray());
                }
            }

            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "blocked"),
                "pre-existing file");
            var restoredStatus =
                await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
                await Assert.That(VaryingArtifactSetModule.SawStaleArtifact).IsFalse();
                await Assert.That(System.IO.File.Exists(Path.Combine(
                        temporaryDirectory,
                        "artifact-set",
                        "stale.txt")))
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
    public async Task ZipCentralDirectoryReadsEntryCountWithoutMaterializingEntries()
    {
        var archivePath = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-central-directory-{Guid.NewGuid():N}.zip");

        try
        {
            using (var archive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
            {
                archive.CreateEntry("result.json");
                archive.CreateEntry("artifacts/output/");
                archive.CreateEntry("artifacts/output/value.txt");
            }

            await Assert.That(ZipCentralDirectory.ReadEntryCount(archivePath)).IsEqualTo(3);
        }
        finally
        {
            System.IO.File.Delete(archivePath);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRejectsArtifactEntryCountAboveConfiguredLimit()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-entry-limit-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            await RunVaryingArtifactSetPipelineAsync(
                temporaryDirectory,
                maximumArtifactEntries: 2);

            var cacheEntry = Directory.GetFiles(cacheDirectory, "*.zip").Single();
            using (var archive = ZipFile.Open(cacheEntry, ZipArchiveMode.Update))
            await using (var output = archive
                             .CreateEntry("artifacts/artifact-set/extra.txt")
                             .Open())
            {
                await output.WriteAsync("extra"u8.ToArray());
            }

            var restoredStatus = await RunVaryingArtifactSetPipelineAsync(
                temporaryDirectory,
                maximumArtifactEntries: 2);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreSeparatesInputAndArtifactEntryLimits()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-separate-entry-limits-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        MultipleArtifactFilesModule.WorkingDirectory = temporaryDirectory;
        MultipleArtifactFilesModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "multiple-artifacts-input.txt"),
                "a");

            var firstStatus = await RunMultipleArtifactFilesPipelineAsync(temporaryDirectory);
            var secondStatus = await RunMultipleArtifactFilesPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(firstStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(secondStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(MultipleArtifactFilesModule.ExecutionCount).IsEqualTo(1);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRejectsArtifactBytesAboveConfiguredLimit()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-byte-limit-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            await RunVaryingArtifactSetPipelineAsync(
                temporaryDirectory,
                maximumArtifactBytes: 1);

            var cacheEntry = Directory.GetFiles(cacheDirectory, "*.zip").Single();
            using (var archive = ZipFile.Open(cacheEntry, ZipArchiveMode.Update))
            await using (var output = archive
                             .CreateEntry("artifacts/artifact-set/extra.txt")
                             .Open())
            {
                await output.WriteAsync("x"u8.ToArray());
            }

            var restoredStatus = await RunVaryingArtifactSetPipelineAsync(
                temporaryDirectory,
                maximumArtifactBytes: 1);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRejectsCacheEntryAboveConfiguredLimit()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-download-limit-{Guid.NewGuid():N}");
        Directory.CreateDirectory(temporaryDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            var restoredStatus = await RunVaryingArtifactSetPipelineAsync(
                temporaryDirectory,
                maximumCacheEntryBytes: 1);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRejectsFailureResultVariant()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-failure-result-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            ModuleResult<string> failure = new ModuleResult<string>.Failure(
                new InvalidOperationException("poisoned cache result"))
            {
                ModuleName = nameof(VaryingArtifactSetModule),
                ModuleDuration = TimeSpan.Zero,
                ModuleStart = DateTimeOffset.UtcNow,
                ModuleEnd = DateTimeOffset.UtcNow,
                Status = ModuleStatus.Failed,
            };
            var cacheEntry = Directory.GetFiles(cacheDirectory, "*.zip").Single();
            using (var archive = ZipFile.Open(cacheEntry, ZipArchiveMode.Update))
            {
                archive.GetEntry("result.json")!.Delete();
                await using var output = archive.CreateEntry("result.json").Open();
                await JsonSerializer.SerializeAsync<ModuleResult<string>>(output, failure);
            }

            var restoredStatus = await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRestoresModeForNonEmptyStaleGlobDirectory()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-shallow-glob-mode-{Guid.NewGuid():N}");
        var staleDirectory = Path.Combine(temporaryDirectory, "shallow-glob", "old");
        Directory.CreateDirectory(temporaryDirectory);
        ShallowGlobArtifactModule.ExecutionCount = 0;
        var expectedMode = UnixFileMode.UserRead | UnixFileMode.UserExecute;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "shallow-glob-input.txt"),
                "input");
            await RunShallowGlobArtifactPipelineAsync(temporaryDirectory);

            Directory.CreateDirectory(staleDirectory);
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(staleDirectory, "keep.bin"),
                "undeclared");
            System.IO.File.SetUnixFileMode(staleDirectory, expectedMode);

            var restoredStatus =
                await RunShallowGlobArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(ShallowGlobArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(System.IO.File.GetUnixFileMode(staleDirectory))
                    .IsEqualTo(expectedMode);
            }
        }
        finally
        {
            if (Directory.Exists(staleDirectory))
            {
                System.IO.File.SetUnixFileMode(
                    staleDirectory,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute);
            }

            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRejectsResultAboveConfiguredLimit()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-result-limit-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            var cacheEntry = Directory.GetFiles(cacheDirectory, "*.zip").Single();
            string resultJson;
            using (var archive = ZipFile.OpenRead(cacheEntry))
            using (var reader = new StreamReader(
                       archive.GetEntry("result.json")!.Open()))
            {
                resultJson = await reader.ReadToEndAsync();
            }

            var oversizedResult = resultJson.Insert(
                1,
                $"\"Padding\":\"{new string('x', 4 * 1024)}\",");
            using (var archive = ZipFile.Open(cacheEntry, ZipArchiveMode.Update))
            {
                archive.GetEntry("result.json")!.Delete();
                await using var writer = new StreamWriter(
                    archive.CreateEntry("result.json", CompressionLevel.Optimal).Open());
                await writer.WriteAsync(oversizedResult);
            }

            var restoredStatus = await RunVaryingArtifactSetPipelineAsync(
                temporaryDirectory,
                maximumResultBytes: 1024);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public Task CacheSaveSkipsResultAboveConfiguredLimit() =>
        AssertCacheSaveSkippedAsync(
            maximumCacheEntryBytes: 10L * 1024 * 1024 * 1024,
            maximumResultBytes: 1);

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public Task CacheSaveSkipsEntryAboveConfiguredLimit() =>
        AssertCacheSaveSkippedAsync(
            maximumCacheEntryBytes: 1,
            maximumResultBytes: 64L * 1024 * 1024);

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task FailedWorkingDirectoryRestoreRestoresModeBeforeExecution()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-root-rollback-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        WorkingDirectoryArtifactModule.WorkingDirectory = temporaryDirectory;
        WorkingDirectoryArtifactModule.ExecutionCount = 0;
        WorkingDirectoryArtifactModule.ModeBeforeExecution = null;
        var expectedMode =
            UnixFileMode.UserRead
            | UnixFileMode.UserExecute
            | UnixFileMode.GroupRead
            | UnixFileMode.GroupExecute
            | UnixFileMode.OtherRead
            | UnixFileMode.OtherExecute;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "working-directory-input.txt"),
                "input");
            await RunWorkingDirectoryArtifactPipelineAsync(temporaryDirectory);

            var cacheEntry = Directory.GetFiles(cacheDirectory, "*.zip").Single();
            using (var archive = ZipFile.Open(cacheEntry, ZipArchiveMode.Update))
            {
                await using (var output = archive.CreateEntry("artifacts/collision").Open())
                {
                    await output.WriteAsync("file"u8.ToArray());
                }

                await using (var output = archive
                                 .CreateEntry("artifacts/collision/child.txt")
                                 .Open())
                {
                    await output.WriteAsync("child"u8.ToArray());
                }
            }

            var restoredStatus =
                await RunWorkingDirectoryArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(WorkingDirectoryArtifactModule.ExecutionCount).IsEqualTo(2);
                await Assert.That(WorkingDirectoryArtifactModule.ModeBeforeExecution)
                    .IsEqualTo(expectedMode);
            }
        }
        finally
        {
            if (Directory.Exists(temporaryDirectory))
            {
                System.IO.File.SetUnixFileMode(
                    temporaryDirectory,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute);
            }

            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRejectsUndeclaredArtifactEntries()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-undeclared-artifact-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        var sourceFile = Path.Combine(temporaryDirectory, "source.txt");
        Directory.CreateDirectory(temporaryDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            await System.IO.File.WriteAllTextAsync(sourceFile, "original");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            var cacheEntry = Directory.GetFiles(cacheDirectory, "*.zip").Single();
            using (var archive = ZipFile.Open(cacheEntry, ZipArchiveMode.Update))
            await using (var output = archive
                             .CreateEntry("artifacts/source.txt")
                             .Open())
            {
                await output.WriteAsync("poisoned"u8.ToArray());
            }

            var restoredStatus =
                await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
                await Assert.That(await System.IO.File.ReadAllTextAsync(sourceFile))
                    .IsEqualTo("original");
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRejectsEntriesNestedUnderDirectoryLinks()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-nested-link-{Guid.NewGuid():N}");
        var externalDirectory = $"{temporaryDirectory}-external";
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        Directory.CreateDirectory(externalDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            var cacheEntry = Directory.GetFiles(cacheDirectory, "*.zip").Single();
            using (var archive = ZipFile.Open(cacheEntry, ZipArchiveMode.Update))
            {
                var directoryLink =
                    archive.CreateEntry("artifacts/artifact-set/escape");
                directoryLink.ExternalAttributes = 0xA000 << 16;
                await using (var output = directoryLink.Open())
                {
                    await output.WriteAsync(
                        Encoding.UTF8.GetBytes(externalDirectory));
                }

                var nestedParent = ModuleCacheFileResolver
                    .GetPathComparer(temporaryDirectory)
                    .Equals("escape", "ESCAPE")
                    ? "ESCAPE"
                    : "escape";
                var nestedLink = archive.CreateEntry(
                    $"artifacts/artifact-set/{nestedParent}/child");
                nestedLink.ExternalAttributes = 0xA000 << 16;
                await using (var output = nestedLink.Open())
                {
                    await output.WriteAsync("escaped.txt"u8.ToArray());
                }
            }

            var restoredStatus =
                await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
                await Assert.That(Directory.EnumerateFileSystemEntries(externalDirectory))
                    .IsEmpty();
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
            Directory.Delete(externalDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRejectsSymbolicLinkTargetsOutsideWorkingDirectory()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-external-link-{Guid.NewGuid():N}");
        var externalDirectory = $"{temporaryDirectory}-external";
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        Directory.CreateDirectory(externalDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            var cacheEntry = Directory.GetFiles(cacheDirectory, "*.zip").Single();
            using (var archive = ZipFile.Open(cacheEntry, ZipArchiveMode.Update))
            {
                var link = archive.CreateEntry("artifacts/artifact-set/escape");
                link.ExternalAttributes = 0xA000 << 16;
                await using var output = link.Open();
                await output.WriteAsync(Encoding.UTF8.GetBytes(Path.GetRelativePath(
                    Path.Combine(temporaryDirectory, "artifact-set"),
                    externalDirectory)));
            }

            var restoredStatus =
                await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
                await Assert.That(System.IO.File.Exists(Path.Combine(
                        temporaryDirectory,
                        "artifact-set",
                        "escape")))
                    .IsFalse();
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
            Directory.Delete(externalDirectory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel(nameof(ModuleCacheTests))]
    public async Task CacheRestoreRejectsTargetsThroughExistingSymbolicLinks()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-linked-target-{Guid.NewGuid():N}");
        var externalDirectory = $"{temporaryDirectory}-external";
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        Directory.CreateDirectory(externalDirectory);
        Directory.CreateSymbolicLink(
            Path.Combine(temporaryDirectory, "linked-target"),
            externalDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            var cacheEntry = Directory.GetFiles(cacheDirectory, "*.zip").Single();
            using (var archive = ZipFile.Open(cacheEntry, ZipArchiveMode.Update))
            {
                var link = archive.CreateEntry("artifacts/artifact-set/escape");
                link.ExternalAttributes = 0xA000 << 16;
                await using var output = link.Open();
                await output.WriteAsync("../linked-target"u8.ToArray());
            }

            var restoredStatus =
                await RunVaryingArtifactSetPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
                await Assert.That(Directory.EnumerateFileSystemEntries(externalDirectory))
                    .IsEmpty();
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
            Directory.Delete(externalDirectory, recursive: true);
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
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
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
    public async Task CacheRestorePreservesSymbolicLinks()
    {
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
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
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
    public async Task CacheRestorePreservesDirectorySymbolicLinks()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-directory-link-{Guid.NewGuid():N}");
        var artifactDirectory = Path.Combine(
            temporaryDirectory,
            "directory-symlink-artifacts");
        Directory.CreateDirectory(temporaryDirectory);
        DirectorySymbolicLinkArtifactModule.WorkingDirectory = temporaryDirectory;
        DirectorySymbolicLinkArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "directory-symlink-input.txt"),
                "input");
            await RunDirectorySymbolicLinkArtifactPipelineAsync(temporaryDirectory);
            if (!OperatingSystem.IsWindows())
            {
                System.IO.File.SetUnixFileMode(
                    artifactDirectory,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute);
            }

            Directory.Delete(artifactDirectory, recursive: true);

            var restoredStatus =
                await RunDirectorySymbolicLinkArtifactPipelineAsync(temporaryDirectory);
            var directoryLink = new DirectoryInfo(
                Path.Combine(artifactDirectory, "current"));

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(DirectorySymbolicLinkArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(directoryLink.LinkTarget).IsEqualTo("version-2");
                await Assert.That(await System.IO.File.ReadAllTextAsync(
                        Path.Combine(directoryLink.FullName, "payload.txt")))
                    .IsEqualTo("version two");
            }
        }
        finally
        {
            if (!OperatingSystem.IsWindows() && Directory.Exists(artifactDirectory))
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
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
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
    public async Task CacheRestoreClearsReadOnlyArtifactDirectories()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-read-only-cleanup-{Guid.NewGuid():N}");
        var artifactDirectory = Path.Combine(
            temporaryDirectory,
            "directory-symlink-artifacts");
        Directory.CreateDirectory(temporaryDirectory);
        DirectorySymbolicLinkArtifactModule.WorkingDirectory = temporaryDirectory;
        DirectorySymbolicLinkArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "directory-symlink-input.txt"),
                "input");
            await RunDirectorySymbolicLinkArtifactPipelineAsync(temporaryDirectory);

            var restoredStatus =
                await RunDirectorySymbolicLinkArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(DirectorySymbolicLinkArtifactModule.ExecutionCount).IsEqualTo(1);
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
    public async Task CacheRestoreClearsFilesUnderReadOnlyParentDirectories()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-read-only-file-parent-{Guid.NewGuid():N}");
        var artifactParent = Path.Combine(temporaryDirectory, "read-only-parent");
        Directory.CreateDirectory(temporaryDirectory);
        ReadOnlyFileParentArtifactModule.WorkingDirectory = temporaryDirectory;
        ReadOnlyFileParentArtifactModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "file-parent-input.txt"),
                "input");
            await RunReadOnlyFileParentArtifactPipelineAsync(temporaryDirectory);

            var restoredStatus =
                await RunReadOnlyFileParentArtifactPipelineAsync(temporaryDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
                await Assert.That(ReadOnlyFileParentArtifactModule.ExecutionCount).IsEqualTo(1);
                await Assert.That(await System.IO.File.ReadAllTextAsync(
                        Path.Combine(artifactParent, "tool")))
                    .IsEqualTo("artifact");
                await Assert.That(System.IO.File.GetUnixFileMode(artifactParent))
                    .IsEqualTo(UnixFileMode.UserRead | UnixFileMode.UserExecute);
            }
        }
        finally
        {
            if (Directory.Exists(artifactParent))
            {
                System.IO.File.SetUnixFileMode(
                    artifactParent,
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
                await Assert.That(restoredStatus).IsEqualTo(ModuleStatus.RestoredFromCache);
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

    private static async Task<ModuleStatus> RunPipelineAsync(
        string workingDirectory,
        string cacheDirectory,
        bool disableModuleCache = false)
    {
        var builder = TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = cacheDirectory;
            })
            .AddModule<CachedModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            DisableModuleCache = disableModuleCache,
        });

        await using var host = await builder.BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(CachedModule))!
            .Status;
    }

    private static async Task<bool?> RunPipelineWithProgressAsync(
        string workingDirectory,
        string cacheDirectory)
    {
        var progressDisplay = new TrackingProgressDisplay();
        var builder = TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = cacheDirectory;
            })
            .AddModule<CachedModule>();
        builder.Services.AddSingleton<IProgressDisplay>(progressDisplay);

        await using var host = await builder.BuildAsync();
        await host.RunAsync();
        return progressDisplay.LastCompletionWasSuccessful;
    }

    private static async Task<ModuleStatus> RunDependencyPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
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
            .Status;
    }

    private static async Task<ModuleStatus> RunInputMutatingPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
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
            .Status;
    }

    private static async Task<ModuleStatus> RunRuntimeTypedDependencyPipelineAsync(
        string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<RuntimeTypedDependencyModule>()
            .AddModule<RuntimeTypedCachedDependentModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(RuntimeTypedCachedDependentModule))!
            .Status;
    }

    private static async Task<(ModuleStatus Status, object? Value)>
        RunRuntimeTypedResultPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<RuntimeTypedCachedResultModule>()
            .BuildAsync();

        await host.RunAsync();
        var result = host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(RuntimeTypedCachedResultModule))!;
        return (result.Status, result.ValueOrDefault);
    }

    private static async Task<ModuleStatus> RunEnvironmentPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<EnvironmentCachedModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(EnvironmentCachedModule))!
            .Status;
    }

    private static async Task<ModuleStatus> RunLookupFailurePipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
                options.MaximumInputFiles = 1;
            })
            .AddModule<LookupFailureInputMutatingModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(LookupFailureInputMutatingModule))!
            .Status;
    }

    private static async Task<ModuleStatus> RunAfterHookArtifactPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
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
            .Status;
    }

    private static async Task<(ModuleResult<string> ModuleResult, string? DependentValue)>
        RunTransformedResultPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
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

    private static async Task AssertCacheSaveSkippedAsync(
        long maximumCacheEntryBytes,
        long maximumResultBytes)
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-cache-save-limit-{Guid.NewGuid():N}");
        var cacheDirectory = Path.Combine(temporaryDirectory, "cache");
        Directory.CreateDirectory(temporaryDirectory);
        VaryingArtifactSetModule.WorkingDirectory = temporaryDirectory;
        VaryingArtifactSetModule.ExecutionCount = 0;

        try
        {
            await System.IO.File.WriteAllTextAsync(
                Path.Combine(temporaryDirectory, "set-input.txt"),
                "a");
            var firstStatus = await RunVaryingArtifactSetPipelineAsync(
                temporaryDirectory,
                maximumCacheEntryBytes: maximumCacheEntryBytes,
                maximumResultBytes: maximumResultBytes);

            var cacheEntries = Directory.Exists(cacheDirectory)
                ? Directory.GetFiles(cacheDirectory, "*.zip")
                : [];
            await Assert.That(cacheEntries).IsEmpty();

            var secondStatus = await RunVaryingArtifactSetPipelineAsync(
                temporaryDirectory,
                maximumCacheEntryBytes: maximumCacheEntryBytes,
                maximumResultBytes: maximumResultBytes);

            using (Assert.Multiple())
            {
                await Assert.That(firstStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(secondStatus).IsEqualTo(ModuleStatus.Succeeded);
                await Assert.That(VaryingArtifactSetModule.ExecutionCount).IsEqualTo(2);
            }
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    private static async Task<ModuleStatus> RunVaryingArtifactSetPipelineAsync(
        string workingDirectory,
        int maximumInputFiles = 100_000,
        int maximumArtifactEntries = 100_000,
        long maximumArtifactBytes = 10L * 1024 * 1024 * 1024,
        long maximumCacheEntryBytes = 10L * 1024 * 1024 * 1024,
        long maximumResultBytes = 64L * 1024 * 1024)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
                options.MaximumInputFiles = maximumInputFiles;
                options.MaximumArtifactEntries = maximumArtifactEntries;
                options.MaximumArtifactBytes = maximumArtifactBytes;
                options.MaximumCacheEntryBytes = maximumCacheEntryBytes;
                options.MaximumResultBytes = maximumResultBytes;
            })
            .AddModule<VaryingArtifactSetModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(VaryingArtifactSetModule))!
            .Status;
    }

    private static async Task<ModuleStatus> RunMultipleArtifactFilesPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
                options.MaximumInputFiles = 1;
                options.MaximumArtifactEntries = 3;
            })
            .AddModule<MultipleArtifactFilesModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(MultipleArtifactFilesModule))!
            .Status;
    }

    private static async Task<ModuleStatus> RunExecutableArtifactPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
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
            .Status;
    }

    private static async Task<ModuleStatus> RunReadOnlyFileParentArtifactPipelineAsync(
        string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<ReadOnlyFileParentArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(ReadOnlyFileParentArtifactModule))!
            .Status;
    }

    private static async Task<ModuleStatus> RunSymbolicLinkArtifactPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
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
            .Status;
    }

    private static async Task<ModuleStatus> RunDirectorySymbolicLinkArtifactPipelineAsync(
        string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
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
            .Status;
    }

    private static async Task<ModuleStatus> RunOptionalArtifactPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
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
            .Status;
    }

    private static async Task<ModuleStatus> RunNestedOptionalArtifactPipelineAsync(
        string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<NestedOptionalArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(NestedOptionalArtifactModule))!
            .Status;
    }

    private static async Task<ModuleStatus> RunDanglingSymbolicLinkArtifactPipelineAsync(
        string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<DanglingSymbolicLinkArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(DanglingSymbolicLinkArtifactModule))!
            .Status;
    }

    private static async Task<ModuleStatus> RunWorkingDirectoryArtifactPipelineAsync(
        string workingDirectory)
    {
        Directory.CreateDirectory(Path.Combine(workingDirectory, "cache"));
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<WorkingDirectoryArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(WorkingDirectoryArtifactModule))!
            .Status;
    }

    private static async Task<ModuleStatus> RunDanglingDirectorySymbolicLinkArtifactPipelineAsync(
        string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<DanglingDirectorySymbolicLinkArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(DanglingDirectorySymbolicLinkArtifactModule))!
            .Status;
    }

    private static async Task<ModuleStatus> RunGlobOptionalArtifactPipelineAsync(
        string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<GlobOptionalArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(GlobOptionalArtifactModule))!
            .Status;
    }

    private static async Task<ModuleStatus> RunShallowGlobArtifactPipelineAsync(
        string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
            .AddModuleCache<FileSystemModuleCache>(options =>
            {
                options.WorkingDirectory = workingDirectory;
                options.CacheDirectory = Path.Combine(workingDirectory, "cache");
            })
            .AddModule<ShallowGlobArtifactModule>()
            .BuildAsync();

        await host.RunAsync();
        return host.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(ShallowGlobArtifactModule))!
            .Status;
    }

    private static async Task<IModuleResult> RunSkippableCachePipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
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

    private static async Task<ModuleStatus> RunEmptyDirectoryArtifactPipelineAsync(string workingDirectory)
    {
        await using var host = await TestPipelineBuilder.Create()
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
            .Status;
    }
}
