using ModularPipelines.Reporting;
using ModularPipelines.Events;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
using ModularPipelines.Configuration;
using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.Validation;
using Moq;

namespace ModularPipelines.UnitTests.Artifacts;

[TUnit.Core.NotInParallel(nameof(ArtifactContractTests))]
public class ArtifactContractTests
{
    private const string LocalArtifactRoot = ".modular-pipelines-local-artifact-tests";
    private const string ProducedFile = LocalArtifactRoot + "/produced/output.txt";
    private const string RestoreDirectory = LocalArtifactRoot + "/restored";
    private const string AfterHookProducedFile = LocalArtifactRoot + "/after-hook/output.txt";
    private const string AfterHookRestoreDirectory = LocalArtifactRoot + "/after-hook-restored";
    private const string MultipleProducedDirectory = LocalArtifactRoot + "/multiple-produced";
    private const string MultipleProducedPattern = MultipleProducedDirectory + "/directory-*";
    private const string MultipleRestoreDirectory = LocalArtifactRoot + "/multiple-restored";
    private const string CacheOnlyFile = LocalArtifactRoot + "/cache-only.bin";
    private const string MissingRuntimeFile = LocalArtifactRoot + "/missing/output.txt";
    private const string FailedRuntimeFile = LocalArtifactRoot + "/failed/output.txt";
    private const string FailedRestoreDirectory = LocalArtifactRoot + "/failed-restored";
    private const string CacheKeyArtifactFile = "cache-key-input.txt";
    private const string CacheKeyRestoreDirectory = "cache-key-restored";

    private sealed class RecordingLoggerProvider : ILoggerProvider
    {
        public ConcurrentQueue<(string Category, string Message)> Entries { get; } = new();

        public ILogger CreateLogger(string categoryName) => new RecordingLogger(categoryName, Entries);

        public void Dispose()
        {
        }

        private sealed class RecordingLogger(
            string category,
            ConcurrentQueue<(string Category, string Message)> entries) : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state)
                where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception? exception,
                Func<TState, Exception?, string> formatter) =>
                entries.Enqueue((category, formatter(state, exception)));
        }
    }

    [Test]
    public async Task ArtifactLifecycleLoggingUsesAmbientModuleLogger()
    {
        var loggerProvider = new RecordingLoggerProvider();
        var builder = TestPipelineBuilder.Create();
        builder.ConfigureServices(services => services.AddLogging(logging => logging.AddProvider(loggerProvider)));
        builder.AddModule<AmbientArtifactLoggingProducerModule>();
        builder.AddModule<AmbientArtifactLoggingConsumerModule>();

        await Assert.That(async () => await builder.RunAsync())
            .Throws<ModuleFailedException>();

        await Assert.That(loggerProvider.Entries).Contains(entry =>
            entry.Category.Contains(nameof(AmbientArtifactLoggingProducerModule), StringComparison.Ordinal)
            && entry.Message.Contains("No files matched pattern", StringComparison.Ordinal));
        await Assert.That(loggerProvider.Entries).DoesNotContain(entry =>
            entry.Category.Contains(nameof(ArtifactLifecycleManager), StringComparison.Ordinal)
            && entry.Message.Contains("No files matched pattern", StringComparison.Ordinal));
    }

    [Test]
    public async Task ResolvePathPatternMatchesWildcardDirectoryComponents()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"artifact-glob-{Guid.NewGuid():N}");
        var releaseDirectory = Directory.CreateDirectory(Path.Combine(directory, "release-v1"));
        var debugDirectory = Directory.CreateDirectory(Path.Combine(directory, "debug"));
        var releaseManifest = Path.Combine(releaseDirectory.FullName, "manifest.json");
        await File.WriteAllTextAsync(releaseManifest, "release");
        await File.WriteAllTextAsync(Path.Combine(debugDirectory.FullName, "manifest.json"), "debug");

        try
        {
            var manager = new ArtifactLifecycleManager(
                Mock.Of<IDistributedArtifactStore>(),
                Microsoft.Extensions.Options.Options.Create(new ArtifactOptions()),
                NullLogger<ArtifactLifecycleManager>.Instance,
                directory);

            var matches = manager.ResolvePathPattern("release-*/manifest.json");

            await Assert.That(matches).IsEquivalentTo([releaseManifest]);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [ProducesArtifact("declared-output", "unused.txt")]
    private sealed class DeclaredProducerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("produced");
    }

    [ProducesArtifact("ambient-logging-output", ".modular-pipelines-ambient-logging/missing.txt")]
    private sealed class AmbientArtifactLoggingProducerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("produced");
    }

    [ModularPipelines.DependsOn<AmbientArtifactLoggingProducerModule>]
    [ConsumesArtifact(typeof(AmbientArtifactLoggingProducerModule), "ambient-logging-output")]
    private sealed class AmbientArtifactLoggingConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("consumed");
    }

    [ConsumesArtifact(typeof(DeclaredProducerModule), "missing-output")]
    private sealed class MissingArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("consumed");
    }

    [ProducesArtifact("duplicate-output", "first.txt")]
    [ProducesArtifact("duplicate-output", "second.txt")]
    private sealed class DuplicateArtifactProducerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("produced");
    }

    [ModularPipelines.DependsOn<DuplicateArtifactProducerModule>]
    [ConsumesArtifact(typeof(DuplicateArtifactProducerModule), "duplicate-output")]
    private sealed class DuplicateArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("consumed");
    }

    private sealed class AlwaysSkipArtifactCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(true);
    }

    [SkipIf<AlwaysSkipArtifactCondition>]
    [ModularPipelines.DependsOn<DeclaredProducerModule>]
    [ConsumesArtifact(typeof(DeclaredProducerModule), "missing-output")]
    private sealed class AttributeSkippedInvalidArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Skipped consumer must not execute");
    }

    [ConsumesArtifact(typeof(DeclaredProducerModule), "missing-output")]
    private sealed class ConfiguredSkippedInvalidArtifactConsumerModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("consumer skipped"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Skipped consumer must not execute");
    }

    [SkipIf<AlwaysSkipArtifactCondition>]
    private sealed class SkippedArtifactValidationDependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Skipped dependency must not execute");
    }

    [ModularPipelines.DependsOn<SkippedArtifactValidationDependencyModule>]
    [ModularPipelines.DependsOn<DeclaredProducerModule>]
    [ConsumesArtifact(typeof(DeclaredProducerModule), "missing-output")]
    private sealed class DependencySkippedInvalidArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dependency-skipped consumer must not execute");
    }

    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "missing-output")]
    private sealed class InvalidSkippedArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dependency-skipped consumer must not execute");
    }

    private sealed class ArtifactValidationHistoryRepository : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext) =>
            Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext)
        {
            if (module is not SkippedArtifactValidationDependencyModule)
            {
                return Task.FromResult<ModuleResult<T>?>(null);
            }

            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
    }

    [ConsumesArtifact(typeof(DeclaredProducerModule), "declared-output")]
    private sealed class UnorderedArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("consumed");
    }

    [ModularPipelines.DependsOn<DeclaredProducerModule>(Optional = true)]
    [ConsumesArtifact(typeof(DeclaredProducerModule), "declared-output")]
    private sealed class OptionalArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("consumed");
    }

    [ConsumesArtifact(typeof(UnregisteredProducerModule), "unregistered-output")]
    private sealed class UnregisteredProducerConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("consumed");
    }

    [ProducesArtifact("unregistered-output", "unused.txt")]
    private sealed class UnregisteredProducerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("produced");
    }

    [ProducesArtifact("local-output", ProducedFile)]
    private sealed class LocalProducerModule : Module<string>
    {
        public static bool IsReady { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithCacheKeyPart("local-producer-v1");

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ProducedFile)!);
            await File.WriteAllTextAsync(ProducedFile, "local artifact content", cancellationToken);
            IsReady = true;
            return "produced";
        }
    }

    [ConsumesArtifact(typeof(DeclaredProducerModule), "missing-output")]
    private sealed class DynamicSkippedInvalidArtifactConsumerModule : Module<string>
    {
        public static bool ShouldSkip { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => ShouldSkip
                ? SkipDecision.Skip("mutable condition requested a skip")
                : SkipDecision.DoNotSkip);

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Invalid consumer must fail validation");
    }

    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "skipped-runtime")]
    private sealed class MutableArtifactConsumerModule : Module<string>
    {
        public static bool ShouldSkip { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => ShouldSkip
                ? SkipDecision.Skip("mutable consumer skipped")
                : SkipDecision.DoNotSkip);

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("consumed");
    }

    [ModularPipelines.DependsOn<LocalProducerModule>]
    [ConsumesArtifact(typeof(LocalProducerModule), "local-output", RestorePath = RestoreDirectory)]
    private sealed class LocalConsumerModule : Module<string>
    {
        public static string? ConsumedContent { get; set; }

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            ConsumedContent = await File.ReadAllTextAsync(
                Path.Combine(RestoreDirectory, "local-output"),
                cancellationToken);
            return ConsumedContent;
        }
    }

    [ProducesArtifact("after-hook-output", AfterHookProducedFile)]
    private sealed class AfterHookArtifactProducerModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(AfterHookProducedFile)!);
            await File.WriteAllTextAsync(AfterHookProducedFile, "before-hook", cancellationToken);
            return "produced";
        }

        protected override async Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            await File.WriteAllTextAsync(AfterHookProducedFile, "after-hook", cancellationToken);
            return null;
        }
    }

    [ModularPipelines.DependsOn<AfterHookArtifactProducerModule>]
    [ConsumesArtifact(
        typeof(AfterHookArtifactProducerModule),
        "after-hook-output",
        RestorePath = AfterHookRestoreDirectory)]
    private sealed class AfterHookArtifactConsumerModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            await File.ReadAllTextAsync(
                Path.Combine(AfterHookRestoreDirectory, "after-hook-output"),
                cancellationToken);
    }

    private sealed class EndHookArtifactHandler : IModuleEventHandler
    {
        public Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result) =>
            context.ModuleType == typeof(AfterHookArtifactProducerModule)
                ? File.WriteAllTextAsync(AfterHookProducedFile, "end-hook")
                : Task.CompletedTask;
    }

    private sealed class AwaitingEndHookHandler : IModuleEventHandler
    {
        public static ModularPipelines.ModuleStatus? ObservedStatus { get; set; }

        public async Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result)
        {
            if (context.ModuleType != typeof(LocalProducerModule))
            {
                return;
            }

            var awaitedResult = await ((IInternalModule) context.Module).ResultTask.WaitAsync(TimeSpan.FromSeconds(5));
            ObservedStatus = awaitedResult.Status;
        }
    }

    [ModularPipelines.DependsOn<LocalProducerModule>]
    [ConsumesArtifact(typeof(LocalProducerModule), "local-output", RestorePath = RestoreDirectory)]
    private sealed class ProducerStateConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => LocalProducerModule.IsReady
                ? SkipDecision.DoNotSkip
                : SkipDecision.Skip("producer is not ready"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ModularPipelines.DependsOn<LocalProducerModule>]
    private sealed class ProducerStateIntermediateModule : Module<string>
    {
        public static bool IsReady { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            IsReady = true;
            return Task.FromResult<string?>("ready");
        }
    }

    [ModularPipelines.DependsOn<ProducerStateIntermediateModule>]
    [ConsumesArtifact(typeof(LocalProducerModule), "local-output", RestorePath = RestoreDirectory)]
    private sealed class TransitiveProducerStateConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => ProducerStateIntermediateModule.IsReady
                ? SkipDecision.DoNotSkip
                : SkipDecision.Skip("producer path is not ready"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    private sealed class DependencyStateSourceModule : Module<string>
    {
        public static bool IsReady { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            IsReady = true;
            return Task.FromResult<string?>("ready");
        }
    }

    [ModularPipelines.DependsOn<DependencyStateSourceModule>]
    private sealed class SharedDependencySiblingModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => DependencyStateSourceModule.IsReady
                ? SkipDecision.DoNotSkip
                : SkipDecision.Skip("shared dependency state is not ready"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("shared sibling");
        }
    }

    [ModularPipelines.DependsOn<DependencyStateSourceModule>]
    private sealed class StateDependentIntermediateModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => DependencyStateSourceModule.IsReady
                ? SkipDecision.DoNotSkip
                : SkipDecision.Skip("dependency state is not ready"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("intermediate");
        }
    }

    [ModularPipelines.DependsOn<LocalProducerModule>]
    [ModularPipelines.DependsOn<SharedDependencySiblingModule>]
    [ModularPipelines.DependsOn<StateDependentIntermediateModule>]
    [ConsumesArtifact(typeof(LocalProducerModule), "local-output", RestorePath = RestoreDirectory)]
    private sealed class TransitiveDependencyStateConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return await File.ReadAllTextAsync(
                Path.Combine(RestoreDirectory, "local-output"),
                cancellationToken);
        }
    }

    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ModularPipelines.DependsOn<SharedDependencySiblingModule>]
    [ModularPipelines.DependsOn<StateDependentIntermediateModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "skipped-runtime")]
    private sealed class PrerequisiteStateArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ProducesArtifact("multiple-output", MultipleProducedPattern)]
    private sealed class MultipleDirectoryProducerModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var firstDirectory = Path.Combine(MultipleProducedDirectory, "directory-first");
            var secondDirectory = Path.Combine(MultipleProducedDirectory, "directory-second");
            var emptyDirectory = Path.Combine(MultipleProducedDirectory, "directory-empty");
            Directory.CreateDirectory(firstDirectory);
            Directory.CreateDirectory(secondDirectory);
            Directory.CreateDirectory(emptyDirectory);
            Directory.CreateDirectory(Path.Combine(firstDirectory, "empty-child"));
            await File.WriteAllTextAsync(
                Path.Combine(firstDirectory, "output.txt"),
                "first",
                cancellationToken);
            await File.WriteAllTextAsync(
                Path.Combine(secondDirectory, "output.txt"),
                "second",
                cancellationToken);
            await File.WriteAllTextAsync(
                Path.Combine(MultipleProducedDirectory, "directory-file.txt"),
                "file",
                cancellationToken);
            return "produced";
        }
    }

    [ModularPipelines.DependsOn<MultipleDirectoryProducerModule>]
    [ConsumesArtifact(
        typeof(MultipleDirectoryProducerModule),
        "multiple-output",
        RestorePath = MultipleRestoreDirectory)]
    private sealed class MultipleDirectoryConsumerModule : Module<string>
    {
        public static string? ConsumedContent { get; set; }

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var file = await File.ReadAllTextAsync(
                Path.Combine(MultipleRestoreDirectory, "directory-file.txt"),
                cancellationToken);
            var first = await File.ReadAllTextAsync(
                Path.Combine(MultipleRestoreDirectory, "directory-first", "output.txt"),
                cancellationToken);
            var second = await File.ReadAllTextAsync(
                Path.Combine(MultipleRestoreDirectory, "directory-second", "output.txt"),
                cancellationToken);
            return ConsumedContent = $"{file},{first},{second}";
        }
    }

    [ProducesArtifact("cache-only", CacheOnlyFile)]
    private sealed class CacheOnlyProducerModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CacheOnlyFile)!);
            await File.WriteAllTextAsync(CacheOnlyFile, "cache only", cancellationToken);
            return "produced";
        }
    }

    [ProducesArtifact("missing-runtime", MissingRuntimeFile)]
    private sealed class MissingRuntimeProducerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("produced");
    }

    [ProducesArtifact("pending-skip-runtime", MissingRuntimeFile)]
    private sealed class PendingSkipArtifactProducerModule : Module<string>
    {
        public static TaskCompletionSource Executed { get; set; } = CreateSignal();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed.TrySetResult();
            return Task.FromResult<string?>("produced");
        }
    }

    private sealed class PendingSkipDependencyModule : Module<string>
    {
        public static TaskCompletionSource Release { get; set; } = CreateSignal();

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await Release.Task.WaitAsync(cancellationToken);
            return "dependency completed";
        }
    }

    [ModularPipelines.DependsOn<PendingSkipArtifactProducerModule>]
    [ModularPipelines.DependsOn<PendingSkipDependencyModule>]
    [ConsumesArtifact(typeof(PendingSkipArtifactProducerModule), "pending-skip-runtime")]
    private sealed class PendingSkippedArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("consumer skipped"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ModularPipelines.DependsOn<MissingRuntimeProducerModule>]
    [ConsumesArtifact(typeof(MissingRuntimeProducerModule), "missing-runtime")]
    private sealed class MissingRuntimeConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ModularPipelines.DependsOn<MissingRuntimeProducerModule>]
    private sealed class MissingRuntimeIntermediateModule : Module<string>
    {
        public static TaskCompletionSource Release { get; set; } = CreateSignal();

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await Release.Task.WaitAsync(cancellationToken);
            return "intermediate completed";
        }
    }

    [ModularPipelines.DependsOn<MissingRuntimeIntermediateModule>]
    [ConsumesArtifact(typeof(MissingRuntimeProducerModule), "missing-runtime")]
    private sealed class TransitiveMissingRuntimeConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ModularPipelines.DependsOn<MissingRuntimeProducerModule>]
    [ConsumesArtifact(typeof(MissingRuntimeProducerModule), "missing-runtime")]
    private sealed class IgnoredMissingRuntimeConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithIgnoreFailures();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ProducesArtifact("skipped-runtime", MissingRuntimeFile)]
    private sealed class SkippedArtifactProducerModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("producer skipped"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Skipped producer must not execute");
    }

    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "skipped-runtime")]
    private sealed class SkippedArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ModularPipelines.DependsOn<MissingRuntimeProducerModule>]
    [ConsumesArtifact(typeof(MissingRuntimeProducerModule), "missing-runtime")]
    private sealed class ConfiguredSkippedArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("consumer skipped"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "skipped-runtime")]
    private sealed class ConfiguredSkippedHistoricalArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        public static int SkipEvaluations;

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ =>
            {
                Interlocked.Increment(ref SkipEvaluations);
                return SkipDecision.Skip("consumer skipped");
            });

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [SkipIf<AlwaysSkipArtifactCondition>]
    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "skipped-runtime")]
    private sealed class AttributeSkippedHistoricalArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    private sealed class SkippedArtifactBlockerModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("blocker skipped"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Skipped blocker must not execute");
    }

    [ModularPipelines.DependsOn<SkippedArtifactBlockerModule>(Optional = true)]
    [ProducesArtifact("dependency-ordered", MissingRuntimeFile)]
    private sealed class DependencyOrderedSkippedArtifactProducerModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("producer skipped"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Skipped producer must not execute");
    }

    [ModularPipelines.DependsOn<SkippedArtifactBlockerModule>]
    [ModularPipelines.DependsOn<DependencyOrderedSkippedArtifactProducerModule>]
    [ConsumesArtifact(
        typeof(DependencyOrderedSkippedArtifactProducerModule),
        "dependency-ordered")]
    private sealed class DependencySkippedArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ModularPipelines.DependsOn<SkippedArtifactBlockerModule>]
    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "skipped-runtime")]
    private sealed class IndependentDependencySkippedArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    private sealed class HistoryBackedSkippedDependencyModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("history-backed dependency skipped"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("History-backed dependency must not execute");
    }

    [ModularPipelines.DependsOn<HistoryBackedSkippedDependencyModule>]
    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "skipped-runtime")]
    private sealed class HistoryBackedDependencyArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Artifact consumer must cascade-skip");
    }

    private sealed class ArtifactConsumerStateDependencyModule : Module<string>
    {
        public static bool IsReady { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            IsReady = true;
            return Task.FromResult<string?>("ready");
        }
    }

    [ModularPipelines.DependsOn<ArtifactConsumerStateDependencyModule>]
    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "skipped-runtime")]
    private sealed class DependencyStateArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => ArtifactConsumerStateDependencyModule.IsReady
                ? SkipDecision.DoNotSkip
                : SkipDecision.Skip("dependency state is not ready"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ModularPipelines.DependsOn<ArtifactConsumerStateDependencyModule>]
    [ModularPipelines.DependsOn<DeclaredProducerModule>]
    [ConsumesArtifact(typeof(DeclaredProducerModule), "missing-output")]
    private sealed class DependencyStateInvalidArtifactConsumerModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => ArtifactConsumerStateDependencyModule.IsReady
                ? SkipDecision.DoNotSkip
                : SkipDecision.Skip("dependency state is not ready"));

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Invalid consumer must fail validation");
    }

    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ModularPipelines.DependsOn<DependencyOrderedSkippedArtifactProducerModule>]
    [ConsumesArtifact(
        typeof(DependencyOrderedSkippedArtifactProducerModule),
        "dependency-ordered")]
    private sealed class FixedPointArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Fixed-point consumer must cascade-skip");
    }

    [ModularPipelines.DependsOn<DependencyOrderedSkippedArtifactProducerModule>]
    private sealed class UnrelatedHistoryDependentModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("used history dependency");
    }

    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ModularPipelines.DependsOn<DependencyOrderedSkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "skipped-runtime")]
    private sealed class OscillatingFirstArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Oscillating consumer must cascade-skip");
    }

    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ModularPipelines.DependsOn<DependencyOrderedSkippedArtifactProducerModule>]
    [ConsumesArtifact(
        typeof(DependencyOrderedSkippedArtifactProducerModule),
        "dependency-ordered")]
    private sealed class OscillatingSecondArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Oscillating consumer must cascade-skip");
    }

    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    private sealed class UnrelatedFirstHistoryDependentModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("used first history dependency");
    }

    [ModularPipelines.DependsOn<SkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(DeclaredProducerModule), "missing-output")]
    private sealed class PreservedProducerInvalidArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Invalid consumer must fail validation");
    }

    [ModularPipelines.DependsOn<SkippedArtifactBlockerModule>]
    private sealed class TransitiveSkippedArtifactIntermediateModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dependency-skipped intermediate must not execute");
    }

    [ModularPipelines.DependsOn<TransitiveSkippedArtifactIntermediateModule>]
    [ModularPipelines.DependsOn<DependencyOrderedSkippedArtifactProducerModule>]
    [ConsumesArtifact(
        typeof(DependencyOrderedSkippedArtifactProducerModule),
        "dependency-ordered")]
    private sealed class TransitiveDependencySkippedArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ProducesArtifact("failed-runtime", FailedRuntimeFile)]
    private sealed class IgnoredFailureArtifactProducerModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithIgnoreFailures();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Producer failed");
    }

    [ModularPipelines.DependsOn<IgnoredFailureArtifactProducerModule>]
    [ConsumesArtifact(
        typeof(IgnoredFailureArtifactProducerModule),
        "failed-runtime",
        RestorePath = FailedRestoreDirectory)]
    private sealed class IgnoredFailureArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    private sealed class ArtifactHistoryRepository : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext) =>
            Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext)
        {
            if (module is not SkippedArtifactProducerModule
                and not DependencyOrderedSkippedArtifactProducerModule
                and not HistoryBackedSkippedDependencyModule)
            {
                return Task.FromResult<ModuleResult<T>?>(null);
            }

            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
    }

    private sealed class FailingUploadArtifactStore : IDistributedArtifactStore
    {
        public Task<ArtifactReference> UploadAsync(
            ArtifactDescriptor descriptor,
            Stream data,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("simulated artifact upload failure");

        public Task<Stream> DownloadAsync(
            ArtifactReference reference,
            CancellationToken cancellationToken) =>
            throw new NotSupportedException();

        public Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(
            string moduleTypeName,
            CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<ArtifactReference>>([]);

        public Task DeleteAsync(
            ArtifactReference reference,
            CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }

    private sealed class RecordingResultRepository : IModuleResultRepository
    {
        public static int SaveCount { get; set; }

        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext)
        {
            SaveCount++;
            return Task.CompletedTask;
        }

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext) =>
            Task.FromResult<ModuleResult<T>?>(null);
    }

    private sealed class ProducerAndConsumerArtifactHistoryRepository : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext) =>
            Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext)
        {
            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
    }

    [ProducesArtifact("working-output", "working.txt")]
    private sealed class WorkingDirectoryProducerModule : Module<string>
    {
        public static string Root { get; set; } = string.Empty;

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await File.WriteAllTextAsync(
                Path.Combine(Root, "working.txt"),
                "working directory content",
                cancellationToken);
            return "produced";
        }
    }

    [ModularPipelines.DependsOn<WorkingDirectoryProducerModule>]
    [ConsumesArtifact(
        typeof(WorkingDirectoryProducerModule),
        "working-output",
        RestorePath = "restored")]
    private sealed class WorkingDirectoryConsumerModule : Module<string>
    {
        public static string? ConsumedContent { get; set; }

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            ConsumedContent = await File.ReadAllTextAsync(
                Path.Combine(WorkingDirectoryProducerModule.Root, "restored", "working-output"),
                cancellationToken);
            return ConsumedContent;
        }
    }

    [ProducesArtifact("cache-key-input", CacheKeyArtifactFile)]
    private sealed class CacheKeyArtifactProducerModule : Module<string>
    {
        public static string Root { get; set; } = string.Empty;

        public static string Content { get; set; } = string.Empty;

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await File.WriteAllTextAsync(
                Path.Combine(Root, CacheKeyArtifactFile),
                Content,
                cancellationToken);
            return "stable-producer-result";
        }
    }

    [ModularPipelines.DependsOn<CacheKeyArtifactProducerModule>]
    [ConsumesArtifact(
        typeof(CacheKeyArtifactProducerModule),
        "cache-key-input",
        RestorePath = CacheKeyRestoreDirectory)]
    [CacheInputs(CacheKeyRestoreDirectory + "/cache-key-input")]
    private sealed class CacheKeyArtifactConsumerModule : Module<string>
    {
        public static string Root { get; set; } = string.Empty;

        public static int ExecutionCount;

        public static string? ConsumedContent { get; set; }

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref ExecutionCount);
            ConsumedContent = await File.ReadAllTextAsync(
                Path.Combine(Root, CacheKeyRestoreDirectory, "cache-key-input"),
                cancellationToken);
            return ConsumedContent;
        }
    }

    [Test]
    public async Task BuildAsyncRejectsUnknownProducedArtifactName()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<MissingArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(MissingArtifactConsumerModule)
            && error.Message.Contains("missing-output", StringComparison.Ordinal)
            && error.Message.Contains("declared-output", StringComparison.Ordinal));
    }

    [Test]
    public async Task BuildAsyncRejectsUnregisteredArtifactProducer()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<UnregisteredProducerConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(UnregisteredProducerConsumerModule)
            && error.Message.Contains("unregistered producer", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public async Task BuildAsyncRejectsDuplicateProducedArtifactName()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DuplicateArtifactProducerModule>();
        builder.AddModule<DuplicateArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(DuplicateArtifactConsumerModule)
            && error.Message.Contains("more than once", StringComparison.Ordinal));
    }

    [Test]
    public async Task BuildAsyncRejectsConsumerWithoutProducerDependency()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<UnorderedArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(UnorderedArtifactConsumerModule)
            && error.Message.Contains("does not depend", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public async Task BuildAsyncRejectsOptionalProducerDependency()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<OptionalArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(OptionalArtifactConsumerModule)
            && error.Message.Contains("required dependencies", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public async Task BuildAsyncIgnoresInvalidContractOnExcludedConsumer()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            SkippedModules = [nameof(MissingArtifactConsumerModule)],
        });
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<MissingArtifactConsumerModule>();

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline).IsNotNull();
    }

    [Test]
    public async Task BuildAsyncRejectsInvalidContractOnAttributeSkippedConsumer()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<AttributeSkippedInvalidArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(AttributeSkippedInvalidArtifactConsumerModule));
    }

    [Test]
    public async Task BuildAsyncRejectsInvalidContractOnConfiguredSkippedConsumer()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<ConfiguredSkippedInvalidArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(ConfiguredSkippedInvalidArtifactConsumerModule));
    }

    [Test]
    public async Task BuildAsyncRejectsInvalidContractOnDynamicallySkippedConsumer()
    {
        DynamicSkippedInvalidArtifactConsumerModule.ShouldSkip = true;
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<DynamicSkippedInvalidArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(DynamicSkippedInvalidArtifactConsumerModule));
    }

    [Test]
    public async Task BuildAsyncKeepsConsumerWhoseSkipDependsOnDependencyState()
    {
        ArtifactConsumerStateDependencyModule.IsReady = false;
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<ArtifactConsumerStateDependencyModule>();
        builder.AddModule<DependencyStateInvalidArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(DependencyStateInvalidArtifactConsumerModule)
            && error.Message.Contains("does not declare", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public async Task BuildAsyncRejectsInvalidContractOnDependencySkippedConsumer()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<SkippedArtifactValidationDependencyModule>();
        builder.AddModule<DependencySkippedInvalidArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(DependencySkippedInvalidArtifactConsumerModule));
    }

    [Test]
    public async Task BuildAsyncRejectsInvalidContractWhenSkippedDependencyHasHistory()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddResultsRepository<ArtifactValidationHistoryRepository>();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<SkippedArtifactValidationDependencyModule>();
        builder.AddModule<DependencySkippedInvalidArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(DependencySkippedInvalidArtifactConsumerModule)
            && error.Message.Contains("missing-output", StringComparison.Ordinal));
    }

    [Test]
    public async Task BuildAsyncRejectsInvalidContractWhenSelectedDependencyHasHistory()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            SkippedModules = [nameof(SkippedArtifactValidationDependencyModule)],
        });
        builder.AddResultsRepository<ArtifactValidationHistoryRepository>();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<SkippedArtifactValidationDependencyModule>();
        builder.AddModule<DependencySkippedInvalidArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(DependencySkippedInvalidArtifactConsumerModule));
    }

    [Test]
    public async Task BuildAsyncRejectsInvalidContractWhenConsumedSkippedProducerHasHistory()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddResultsRepository<ArtifactHistoryRepository>();
        builder.AddModule<SkippedArtifactProducerModule>();
        builder.AddModule<InvalidSkippedArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(InvalidSkippedArtifactConsumerModule)
            && error.Message.Contains("missing-output", StringComparison.Ordinal));
    }

    [Test]
    public async Task BuildAsyncPreservesValidationAcrossArtifactDemandCycle()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddResultsRepository<ArtifactHistoryRepository>();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<SkippedArtifactProducerModule>();
        builder.AddModule<DependencyOrderedSkippedArtifactProducerModule>();
        builder.AddModule<OscillatingFirstArtifactConsumerModule>();
        builder.AddModule<OscillatingSecondArtifactConsumerModule>();
        builder.AddModule<PreservedProducerInvalidArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(PreservedProducerInvalidArtifactConsumerModule)
            && error.Message.Contains("missing-output", StringComparison.Ordinal));
    }

    [Test]
    public async Task BuildAsyncDoesNotUseMutableSkipAsFallbackDemand()
    {
        MutableArtifactConsumerModule.ShouldSkip = false;
        var builder = Pipeline.CreateBuilder();
        builder.AddResultsRepository<ArtifactHistoryRepository>();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<SkippedArtifactProducerModule>();
        builder.AddModule<MutableArtifactConsumerModule>();
        builder.AddModule<PreservedProducerInvalidArtifactConsumerModule>();

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(
            () => builder.BuildAsync());

        await Assert.That(exception!.ValidationResult.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Artifact
            && error.SourceType == typeof(PreservedProducerInvalidArtifactConsumerModule)
            && error.Message.Contains("missing-output", StringComparison.Ordinal));
    }

    [Test]
    public async Task ValidateAsyncAcceptsMatchingArtifactContract()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<LocalProducerModule>();
        builder.AddModule<LocalConsumerModule>();

        var result = await builder.ValidateAsync();

        await Assert.That(result.Errors).DoesNotContain(error =>
            error.Category == ValidationErrorCategory.Artifact);
    }

    [Test]
    public async Task StandaloneExecutionRestoresConsumedArtifact()
    {
        DeleteLocalArtifacts();
        LocalConsumerModule.ConsumedContent = null;
        AwaitingEndHookHandler.ObservedStatus = null;
        RecordingResultRepository.SaveCount = 0;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<LocalProducerModule>();
            builder.AddModule<LocalConsumerModule>();

            await builder.RunAsync();

            await Assert.That(LocalConsumerModule.ConsumedContent).IsEqualTo("local artifact content");
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    private sealed class LocalProducerCacheRepository : IModuleCacheResultRepository
    {
        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext,
            CancellationToken cancellationToken) =>
            Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext,
            CancellationToken cancellationToken)
        {
            if (module is not LocalProducerModule)
            {
                return Task.FromResult<ModuleResult<T>?>(null);
            }

            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess(default!, executionContext));
        }

        public void DiscardFingerprint(IModule module)
        {
        }
    }

    [Test]
    public async Task ArtifactDemandWaitsForProducerBeforeCachingConsumerSkip()
    {
        DeleteLocalArtifacts();
        LocalProducerModule.IsReady = false;
        ProducerStateConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<LocalProducerModule>();
            builder.AddModule<ProducerStateConsumerModule>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<LocalProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<ProducerStateConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(LocalProducerModule.IsReady).IsTrue();
                await Assert.That(ProducerStateConsumerModule.Executed).IsTrue();
                await Assert.That(File.Exists(Path.Combine(RestoreDirectory, "local-output"))).IsTrue();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactDemandWaitsForTransitiveProducerPathBeforeCachingConsumerSkip()
    {
        DeleteLocalArtifacts();
        LocalProducerModule.IsReady = false;
        ProducerStateIntermediateModule.IsReady = false;
        TransitiveProducerStateConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<LocalProducerModule>();
            builder.AddModule<ProducerStateIntermediateModule>();
            builder.AddModule<TransitiveProducerStateConsumerModule>();

            var summary = await builder.RunAsync();
            var consumerResult = await summary.Modules
                .OfType<TransitiveProducerStateConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(ProducerStateIntermediateModule.IsReady).IsTrue();
                await Assert.That(TransitiveProducerStateConsumerModule.Executed).IsTrue();
                await Assert.That(File.Exists(Path.Combine(RestoreDirectory, "local-output"))).IsTrue();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactDemandRevisitsSharedPrerequisiteAcrossSiblingBranches()
    {
        DeleteLocalArtifacts();
        DependencyStateSourceModule.IsReady = false;
        SharedDependencySiblingModule.Executed = false;
        StateDependentIntermediateModule.Executed = false;
        TransitiveDependencyStateConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<LocalProducerModule>();
            builder.AddModule<DependencyStateSourceModule>();
            builder.AddModule<SharedDependencySiblingModule>();
            builder.AddModule<StateDependentIntermediateModule>();
            builder.AddModule<TransitiveDependencyStateConsumerModule>();

            var summary = await builder.RunAsync();
            var consumerResult = await summary.Modules
                .OfType<TransitiveDependencyStateConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(DependencyStateSourceModule.IsReady).IsTrue();
                await Assert.That(SharedDependencySiblingModule.Executed).IsTrue();
                await Assert.That(StateDependentIntermediateModule.Executed).IsTrue();
                await Assert.That(TransitiveDependencyStateConsumerModule.Executed).IsTrue();
                await Assert.That(consumerResult.ValueOrDefault).IsEqualTo("local artifact content");
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionUploadsArtifactsAfterModuleAfterHook()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<AfterHookArtifactProducerModule>();
            builder.AddModule<AfterHookArtifactConsumerModule>();

            var summary = await builder.RunAsync();
            var consumerResult = await summary.Modules
                .OfType<AfterHookArtifactConsumerModule>()
                .Single();

            await Assert.That(consumerResult.ValueOrDefault).IsEqualTo("after-hook");
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionUploadsArtifactsAfterModuleEndHandler()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModuleEventHandler<EndHookArtifactHandler>();
            builder.AddModule<AfterHookArtifactProducerModule>();
            builder.AddModule<AfterHookArtifactConsumerModule>();

            var summary = await builder.RunAsync();
            var consumerResult = await summary.Modules
                .OfType<AfterHookArtifactConsumerModule>()
                .Single();

            await Assert.That(consumerResult.ValueOrDefault).IsEqualTo("end-hook");
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionRestoresArtifactsBeforeCacheLookup()
    {
        var workingDirectory = Directory.CreateTempSubdirectory("modular-pipelines-artifact-cache-key-");
        var cacheDirectory = Path.Combine(workingDirectory.FullName, "cache");
        CacheKeyArtifactProducerModule.Root = workingDirectory.FullName;
        CacheKeyArtifactConsumerModule.Root = workingDirectory.FullName;
        CacheKeyArtifactConsumerModule.ExecutionCount = 0;
        CacheKeyArtifactConsumerModule.ConsumedContent = null;

        try
        {
            CacheKeyArtifactProducerModule.Content = "first";
            await RunCacheKeyArtifactPipelineAsync(workingDirectory.FullName, cacheDirectory);

            CacheKeyArtifactProducerModule.Content = "second";
            var secondStatus = await RunCacheKeyArtifactPipelineAsync(
                workingDirectory.FullName,
                cacheDirectory);

            using (Assert.Multiple())
            {
                await Assert.That(secondStatus).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(CacheKeyArtifactConsumerModule.ExecutionCount).IsEqualTo(2);
                await Assert.That(CacheKeyArtifactConsumerModule.ConsumedContent).IsEqualTo("second");
            }
        }
        finally
        {
            workingDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task StandaloneExecutionRestoresMixedMatchedPaths()
    {
        DeleteLocalArtifacts();
        MultipleDirectoryConsumerModule.ConsumedContent = null;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<MultipleDirectoryProducerModule>();
            builder.AddModule<MultipleDirectoryConsumerModule>();

            await builder.RunAsync();

            using (Assert.Multiple())
            {
                await Assert.That(MultipleDirectoryConsumerModule.ConsumedContent).IsEqualTo("file,first,second");
                await Assert.That(Directory.Exists(
                    Path.Combine(MultipleRestoreDirectory, "directory-empty"))).IsTrue();
                await Assert.That(Directory.Exists(
                    Path.Combine(MultipleRestoreDirectory, "directory-first", "empty-child"))).IsTrue();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionDoesNotUploadUnconsumedArtifacts()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<CacheOnlyProducerModule>();
            await using var pipeline = await builder.BuildAsync();

            _ = await pipeline.RunAsync();
            var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();
            var artifacts = await store.ListArtifactsAsync(
                typeof(CacheOnlyProducerModule).FullName!,
                CancellationToken.None);

            await Assert.That(artifacts).IsEmpty();
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionUsesFilesystemBackedArtifactStore()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<CacheOnlyProducerModule>();
        await using var pipeline = await builder.BuildAsync();

        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        await Assert.That(store).IsTypeOf<FileSystemDistributedArtifactStore>();
    }

    [Test]
    public async Task StandaloneExecutionDoesNotUploadForExcludedConsumer()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules = [nameof(LocalConsumerModule)],
            });
            builder.AddModule<LocalProducerModule>();
            builder.AddModule<LocalConsumerModule>();
            await using var pipeline = await builder.BuildAsync();

            _ = await pipeline.RunAsync();
            var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();
            var artifacts = await store.ListArtifactsAsync(
                typeof(LocalProducerModule).FullName!,
                CancellationToken.None);

            await Assert.That(File.Exists(ProducedFile)).IsTrue();
            await Assert.That(artifacts).IsEmpty();
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionFailsProducerWhenDemandedArtifactMatchesNoFiles()
    {
        DeleteLocalArtifacts();
        MissingRuntimeConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<MissingRuntimeProducerModule>();
            builder.AddModule<MissingRuntimeConsumerModule>();

            var exception = await Assert.ThrowsAsync<ModuleFailedException>(
                () => builder.RunAsync());

            using (Assert.Multiple())
            {
                await Assert.That(exception!.ModuleType).IsEqualTo(typeof(MissingRuntimeProducerModule));
                await Assert.That(exception.ToString()).Contains("missing-runtime");
                await Assert.That(exception.ToString()).Contains(MissingRuntimeFile);
                await Assert.That(exception.ToString()).Contains(nameof(MissingRuntimeConsumerModule));
                await Assert.That(exception.ToString()).Contains("matched no files");
                await Assert.That(MissingRuntimeConsumerModule.Executed).IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task TransitiveConsumerFailsWithoutRewritingCompletedProducer()
    {
        DeleteLocalArtifacts();
        TransitiveMissingRuntimeConsumerModule.Executed = false;
        MissingRuntimeIntermediateModule.Release = CreateSignal();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<MissingRuntimeProducerModule>();
            builder.AddModule<MissingRuntimeIntermediateModule>();
            builder.AddModule<TransitiveMissingRuntimeConsumerModule>();
            await using var pipeline = await builder.BuildAsync();

            var resultRegistry = pipeline.Services.GetRequiredService<IModuleResultRegistry>();
            var execution = pipeline.RunAsync();
            var producerResult = await WaitForResultAsync(
                    resultRegistry,
                    typeof(MissingRuntimeProducerModule))
                .WaitAsync(TimeSpan.FromSeconds(10));
            MissingRuntimeIntermediateModule.Release.TrySetResult();
            var exception = await Assert.ThrowsAsync<ModuleFailedException>(() => execution);

            using (Assert.Multiple())
            {
                await Assert.That(exception!.ModuleType).IsEqualTo(typeof(TransitiveMissingRuntimeConsumerModule));
                await Assert.That(exception.ToString()).Contains("missing-runtime");
                await Assert.That(exception.ToString()).Contains(nameof(TransitiveMissingRuntimeConsumerModule));
                await Assert.That(exception.ToString()).Contains("not found");
                await Assert.That(producerResult!.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(TransitiveMissingRuntimeConsumerModule.Executed).IsFalse();
            }
        }
        finally
        {
            MissingRuntimeIntermediateModule.Release.TrySetResult();
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task IgnoredConsumerDoesNotHideMissingProducerArtifact()
    {
        DeleteLocalArtifacts();
        IgnoredMissingRuntimeConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<MissingRuntimeProducerModule>();
            builder.AddModule<IgnoredMissingRuntimeConsumerModule>();

            var exception = await Assert.ThrowsAsync<ModuleFailedException>(
                () => builder.RunAsync());

            using (Assert.Multiple())
            {
                await Assert.That(exception!.ModuleType).IsEqualTo(typeof(MissingRuntimeProducerModule));
                await Assert.That(exception.ToString()).Contains(nameof(IgnoredMissingRuntimeConsumerModule));
                await Assert.That(IgnoredMissingRuntimeConsumerModule.Executed).IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionFailsProducerWhenRequiredArtifactUploadFails()
    {
        DeleteLocalArtifacts();
        LocalConsumerModule.ConsumedContent = null;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.Services.AddSingleton<IDistributedArtifactStore, FailingUploadArtifactStore>();
            builder.AddResultsRepository<RecordingResultRepository>();
            builder.AddModuleEventHandler<AwaitingEndHookHandler>();
            builder.AddModule<LocalProducerModule>();
            builder.AddModule<LocalConsumerModule>();
            await using var pipeline = await builder.BuildAsync();

            var exception = await Assert.ThrowsAsync<ModuleFailedException>(
                () => pipeline.RunAsync());
            var producer = pipeline.Services
                .GetServices<IModule>()
                .OfType<LocalProducerModule>()
                .Single();
            var producerResult = pipeline.Services
                .GetRequiredService<IModuleResultRegistry>()
                .GetResult(typeof(LocalProducerModule));
            var awaitedProducerResult = await producer;

            using (Assert.Multiple())
            {
                await Assert.That(exception!.ToString()).Contains("simulated artifact upload failure");
                await Assert.That(producerResult).IsNotNull();
                await Assert.That(producerResult!.Status).IsEqualTo(ModularPipelines.ModuleStatus.Failed);
                await Assert.That(awaitedProducerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Failed);
                await Assert.That(RecordingResultRepository.SaveCount).IsEqualTo(0);
                await Assert.That(AwaitingEndHookHandler.ObservedStatus)
                    .IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(LocalConsumerModule.ConsumedContent).IsNull();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionSkipsArtifactRestoreForSkippedDependency()
    {
        DeleteLocalArtifacts();
        SkippedArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactConsumerModule>();

            await builder.RunAsync();

            await Assert.That(SkippedArtifactConsumerModule.Executed).IsFalse();
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactProducerHistoryDoesNotKeepRequiredConsumerRunnable()
    {
        DeleteLocalArtifacts();
        SkippedArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<SkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(SkippedArtifactConsumerModule.Executed).IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactProducerUsesHistoryWhenArtifactConsumerIsExcluded()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules = [nameof(SkippedArtifactConsumerModule)],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();

            await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactProducerUsesHistoryWhenConsumerHasConfiguredSkip()
    {
        DeleteLocalArtifacts();
        ConfiguredSkippedHistoricalArtifactConsumerModule.Executed = false;
        ConfiguredSkippedHistoricalArtifactConsumerModule.SkipEvaluations = 0;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<ConfiguredSkippedHistoricalArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<ConfiguredSkippedHistoricalArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(consumerResult.SkipDecisionOrDefault?.Reason)
                    .IsEqualTo("consumer skipped");
                await Assert.That(ConfiguredSkippedHistoricalArtifactConsumerModule.Executed)
                    .IsFalse();
                await Assert.That(ConfiguredSkippedHistoricalArtifactConsumerModule.SkipEvaluations)
                    .IsEqualTo(1);
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task IgnoredArtifactProducerUsesHistoryWhenConsumerHasConfiguredSkip()
    {
        DeleteLocalArtifacts();
        ConfiguredSkippedHistoricalArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules = [nameof(SkippedArtifactProducerModule)],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<ConfiguredSkippedHistoricalArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<ConfiguredSkippedHistoricalArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(ConfiguredSkippedHistoricalArtifactConsumerModule.Executed)
                    .IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactProducerUsesHistoryWhenConsumerDependencySkipped()
    {
        DeleteLocalArtifacts();
        DependencySkippedArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactBlockerModule>();
            builder.AddModule<DependencyOrderedSkippedArtifactProducerModule>();
            builder.AddModule<DependencySkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<DependencyOrderedSkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<DependencySkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(DependencySkippedArtifactConsumerModule.Executed).IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactProducerUsesHistoryWhenIndependentConsumerDependencyWillSkip()
    {
        DeleteLocalArtifacts();
        IndependentDependencySkippedArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactBlockerModule>();
            builder.AddModule<IndependentDependencySkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<IndependentDependencySkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(IndependentDependencySkippedArtifactConsumerModule.Executed)
                    .IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task IgnoredProducerUsesHistoryWhenConsumerDependencyWillSkip()
    {
        DeleteLocalArtifacts();
        IndependentDependencySkippedArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules = [nameof(SkippedArtifactProducerModule)],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactBlockerModule>();
            builder.AddModule<IndependentDependencySkippedArtifactConsumerModule>();
            builder.AddModule<UnrelatedFirstHistoryDependentModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<IndependentDependencySkippedArtifactConsumerModule>()
                .Single();
            var unrelatedResult = await summary.Modules
                .OfType<UnrelatedFirstHistoryDependentModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(unrelatedResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(IndependentDependencySkippedArtifactConsumerModule.Executed)
                    .IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task IgnoredProducerRevisitsSharedPrerequisiteAcrossSiblingBranches()
    {
        DeleteLocalArtifacts();
        DependencyStateSourceModule.IsReady = false;
        SharedDependencySiblingModule.Executed = false;
        StateDependentIntermediateModule.Executed = false;
        PrerequisiteStateArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules = [nameof(SkippedArtifactProducerModule)],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<DependencyStateSourceModule>();
            builder.AddModule<SharedDependencySiblingModule>();
            builder.AddModule<StateDependentIntermediateModule>();
            builder.AddModule<PrerequisiteStateArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<PrerequisiteStateArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(DependencyStateSourceModule.IsReady).IsTrue();
                await Assert.That(SharedDependencySiblingModule.Executed).IsTrue();
                await Assert.That(StateDependentIntermediateModule.Executed).IsTrue();
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(PrerequisiteStateArtifactConsumerModule.Executed).IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task CachedProducerFailureReplacesUsedHistoryResult()
    {
        DeleteLocalArtifacts();

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ProducedFile)!);
            await File.WriteAllTextAsync(ProducedFile, "cached artifact content");
            var builder = Pipeline.CreateBuilder();
            builder.Services.AddSingleton<IModuleCacheResultRepository, LocalProducerCacheRepository>();
            builder.Services.AddSingleton<IDistributedArtifactStore, FailingUploadArtifactStore>();
            builder.AddModule<LocalProducerModule>();
            builder.AddModule<LocalConsumerModule>();
            await using var pipeline = await builder.BuildAsync();

            var exception = await Assert.ThrowsAsync<ModuleFailedException>(
                () => pipeline.RunAsync());
            var producer = pipeline.Services
                .GetServices<IModule>()
                .OfType<LocalProducerModule>()
                .Single();
            var registeredResult = pipeline.Services
                .GetRequiredService<IModuleResultRegistry>()
                .GetResult(typeof(LocalProducerModule));
            var awaitedResult = await producer;

            using (Assert.Multiple())
            {
                await Assert.That(exception!.ToString())
                    .Contains("simulated artifact upload failure");
                await Assert.That(registeredResult!.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.Failed);
                await Assert.That(awaitedResult.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.Failed);
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactProducerExecutesWhenConsumerDependencyUsesHistory()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<HistoryBackedSkippedDependencyModule>();
            builder.AddModule<HistoryBackedDependencyArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var dependencyResult = await summary.Modules
                .OfType<HistoryBackedSkippedDependencyModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<HistoryBackedDependencyArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(dependencyResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task IgnoredProducerHistoryIsSuppressedUntilConsumerDependenciesFinish()
    {
        DeleteLocalArtifacts();
        ArtifactConsumerStateDependencyModule.IsReady = false;
        DependencyStateArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules = [nameof(SkippedArtifactProducerModule)],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<ArtifactConsumerStateDependencyModule>();
            builder.AddModule<DependencyStateArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var stateDependencyResult = await summary.Modules
                .OfType<ArtifactConsumerStateDependencyModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<DependencyStateArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(stateDependencyResult.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(ArtifactConsumerStateDependencyModule.IsReady).IsTrue();
                await Assert.That(DependencyStateArtifactConsumerModule.Executed).IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task IgnoredArtifactDemandConvergesAfterHistorySuppression()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules =
                [
                    nameof(SkippedArtifactProducerModule),
                    nameof(DependencyOrderedSkippedArtifactProducerModule),
                ],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<DependencyOrderedSkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactConsumerModule>();
            builder.AddModule<FixedPointArtifactConsumerModule>();
            builder.AddModule<UnrelatedHistoryDependentModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var firstProducerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var secondProducerResult = await summary.Modules
                .OfType<DependencyOrderedSkippedArtifactProducerModule>()
                .Single();
            var unrelatedResult = await summary.Modules
                .OfType<UnrelatedHistoryDependentModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(firstProducerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(secondProducerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(unrelatedResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task IgnoredArtifactDemandBreaksOscillationWithoutSuppressingAllHistory()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules =
                [
                    nameof(SkippedArtifactProducerModule),
                    nameof(DependencyOrderedSkippedArtifactProducerModule),
                ],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<DependencyOrderedSkippedArtifactProducerModule>();
            builder.AddModule<OscillatingFirstArtifactConsumerModule>();
            builder.AddModule<OscillatingSecondArtifactConsumerModule>();
            builder.AddModule<UnrelatedFirstHistoryDependentModule>();
            builder.AddModule<UnrelatedHistoryDependentModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var firstProducerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var secondProducerResult = await summary.Modules
                .OfType<DependencyOrderedSkippedArtifactProducerModule>()
                .Single();
            var firstUnrelatedResult = await summary.Modules
                .OfType<UnrelatedFirstHistoryDependentModule>()
                .Single();
            var secondUnrelatedResult = await summary.Modules
                .OfType<UnrelatedHistoryDependentModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(firstProducerResult.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(secondProducerResult.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(firstUnrelatedResult.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(secondUnrelatedResult.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task RuntimeArtifactDemandBreaksOscillationWithoutSuppressingAllHistory()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<DependencyOrderedSkippedArtifactProducerModule>();
            builder.AddModule<OscillatingFirstArtifactConsumerModule>();
            builder.AddModule<OscillatingSecondArtifactConsumerModule>();
            builder.AddModule<UnrelatedFirstHistoryDependentModule>();
            builder.AddModule<UnrelatedHistoryDependentModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var firstProducerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var secondProducerResult = await summary.Modules
                .OfType<DependencyOrderedSkippedArtifactProducerModule>()
                .Single();
            var firstUnrelatedResult = await summary.Modules
                .OfType<UnrelatedFirstHistoryDependentModule>()
                .Single();
            var secondUnrelatedResult = await summary.Modules
                .OfType<UnrelatedHistoryDependentModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(firstProducerResult.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(secondProducerResult.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(firstUnrelatedResult.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(secondUnrelatedResult.Status)
                    .IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task IgnoredArtifactProducerUsesHistoryWhenConsumerDependencyIgnored()
    {
        DeleteLocalArtifacts();
        DependencySkippedArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules =
                [
                    nameof(SkippedArtifactBlockerModule),
                    nameof(DependencyOrderedSkippedArtifactProducerModule),
                ],
            });
            builder.AddModule<SkippedArtifactBlockerModule>();
            builder.AddModule<DependencyOrderedSkippedArtifactProducerModule>();
            builder.AddModule<DependencySkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<DependencyOrderedSkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<DependencySkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(DependencySkippedArtifactConsumerModule.Executed).IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task IgnoredArtifactProducerUsesHistoryWhenConsumerTransitivelySkipped()
    {
        DeleteLocalArtifacts();
        TransitiveDependencySkippedArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules =
                [
                    nameof(SkippedArtifactBlockerModule),
                    nameof(DependencyOrderedSkippedArtifactProducerModule),
                ],
            });
            builder.AddModule<SkippedArtifactBlockerModule>();
            builder.AddModule<DependencyOrderedSkippedArtifactProducerModule>();
            builder.AddModule<TransitiveSkippedArtifactIntermediateModule>();
            builder.AddModule<TransitiveDependencySkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<DependencyOrderedSkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<TransitiveDependencySkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(TransitiveDependencySkippedArtifactConsumerModule.Executed)
                    .IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactProducerUsesHistoryWhenConsumerHasAttributeSkip()
    {
        DeleteLocalArtifacts();
        AttributeSkippedHistoricalArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<AttributeSkippedHistoricalArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<AttributeSkippedHistoricalArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(AttributeSkippedHistoricalArtifactConsumerModule.Executed)
                    .IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactProducerUsesHistoryWhenProducerAndConsumerAreExcluded()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules =
                [
                    nameof(SkippedArtifactProducerModule),
                    nameof(SkippedArtifactConsumerModule),
                ],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<SkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task ArtifactProducerUsesHistoryWhenArtifactConsumerIsPrecompleted()
    {
        DeleteLocalArtifacts();

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules = [nameof(SkippedArtifactConsumerModule)],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ProducerAndConsumerArtifactHistoryRepository>();

            var summary = await builder.RunAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<SkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.RestoredFromHistory);
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task IgnoredFailureProducerDoesNotUploadStaleArtifact()
    {
        DeleteLocalArtifacts();
        IgnoredFailureArtifactConsumerModule.Executed = false;

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FailedRuntimeFile)!);
            await File.WriteAllTextAsync(FailedRuntimeFile, "stale artifact");
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<IgnoredFailureArtifactProducerModule>();
            builder.AddModule<IgnoredFailureArtifactConsumerModule>();

            var exception = await Assert.ThrowsAsync<ModuleFailedException>(
                () => builder.RunAsync());

            using (Assert.Multiple())
            {
                await Assert.That(exception!.ToString()).Contains("failed-runtime");
                await Assert.That(exception.ToString()).Contains("not found");
                await Assert.That(IgnoredFailureArtifactConsumerModule.Executed).IsFalse();
            }
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionEvaluatesConsumerSkipBeforeArtifactRestore()
    {
        DeleteLocalArtifacts();
        ConfiguredSkippedArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<MissingRuntimeProducerModule>();
            builder.AddModule<ConfiguredSkippedArtifactConsumerModule>();

            await builder.RunAsync();

            await Assert.That(ConfiguredSkippedArtifactConsumerModule.Executed).IsFalse();
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionDoesNotFailProducerWhileConsumerSkipIsPending()
    {
        DeleteLocalArtifacts();
        PendingSkipArtifactProducerModule.Executed = CreateSignal();
        PendingSkipDependencyModule.Release = CreateSignal();
        PendingSkippedArtifactConsumerModule.Executed = false;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddModule<PendingSkipArtifactProducerModule>();
            builder.AddModule<PendingSkipDependencyModule>();
            builder.AddModule<PendingSkippedArtifactConsumerModule>();

            var execution = builder.RunAsync();
            await PendingSkipArtifactProducerModule.Executed.Task;
            await Task.Delay(100);
            PendingSkipDependencyModule.Release.TrySetResult();

            var summary = await execution;
            var producerResult = await summary.Modules
                .OfType<PendingSkipArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<PendingSkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
                await Assert.That(consumerResult.Status).IsEqualTo(ModularPipelines.ModuleStatus.Skipped);
                await Assert.That(PendingSkippedArtifactConsumerModule.Executed).IsFalse();
            }
        }
        finally
        {
            PendingSkipDependencyModule.Release.TrySetResult();
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneArtifactsUseConfiguredCacheWorkingDirectory()
    {
        var workingDirectory = Directory.CreateTempSubdirectory("modular-pipelines-artifact-root-");
        WorkingDirectoryProducerModule.Root = workingDirectory.FullName;
        WorkingDirectoryConsumerModule.ConsumedContent = null;

        try
        {
            var builder = Pipeline.CreateBuilder();
            builder.Services.Configure<ModuleCacheOptions>(options =>
                options.WorkingDirectory = workingDirectory.FullName);
            builder.AddModule<WorkingDirectoryProducerModule>();
            builder.AddModule<WorkingDirectoryConsumerModule>();

            await builder.RunAsync();

            await Assert.That(WorkingDirectoryConsumerModule.ConsumedContent)
                .IsEqualTo("working directory content");
        }
        finally
        {
            workingDirectory.Delete(recursive: true);
        }
    }

    private static void DeleteLocalArtifacts()
    {
        if (Directory.Exists(LocalArtifactRoot))
        {
            Directory.Delete(LocalArtifactRoot, recursive: true);
        }
    }

    private static TaskCompletionSource CreateSignal() =>
        new(TaskCreationOptions.RunContinuationsAsynchronously);

    private static async Task<IModuleResult> WaitForResultAsync(
        IModuleResultRegistry resultRegistry,
        Type moduleType)
    {
        while (true)
        {
            if (resultRegistry.GetResult(moduleType) is { } result)
            {
                return result;
            }

            await Task.Delay(10);
        }
    }

    private static async Task<ModularPipelines.ModuleStatus> RunCacheKeyArtifactPipelineAsync(
        string workingDirectory,
        string cacheDirectory)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModuleCache<FileSystemModuleCache>(options =>
        {
            options.WorkingDirectory = workingDirectory;
            options.CacheDirectory = cacheDirectory;
        });
        builder.AddModule<CacheKeyArtifactProducerModule>();
        builder.AddModule<CacheKeyArtifactConsumerModule>();
        await using var pipeline = await builder.BuildAsync();

        await pipeline.RunAsync();
        return pipeline.Services
            .GetRequiredService<IModuleResultRegistry>()
            .GetResult(typeof(CacheKeyArtifactConsumerModule))!
            .Status;
    }
}
