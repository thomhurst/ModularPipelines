using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
using ModularPipelines.Configuration;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Validation;

namespace ModularPipelines.UnitTests.Artifacts;

[TUnit.Core.NotInParallel(nameof(ArtifactContractTests))]
public class ArtifactContractTests
{
    private const string LocalArtifactRoot = ".modular-pipelines-local-artifact-tests";
    private const string ProducedFile = LocalArtifactRoot + "/produced/output.txt";
    private const string RestoreDirectory = LocalArtifactRoot + "/restored";
    private const string MultipleProducedDirectory = LocalArtifactRoot + "/multiple-produced";
    private const string MultipleProducedPattern = MultipleProducedDirectory + "/*.txt";
    private const string MultipleRestoreDirectory = LocalArtifactRoot + "/multiple-restored";
    private const string CacheOnlyFile = LocalArtifactRoot + "/cache-only.bin";
    private const string MissingRuntimeFile = LocalArtifactRoot + "/missing/output.txt";
    private const string FailedRuntimeFile = LocalArtifactRoot + "/failed/output.txt";
    private const string FailedRestoreDirectory = LocalArtifactRoot + "/failed-restored";

    [ProducesArtifact("declared-output", "unused.txt")]
    private sealed class DeclaredProducerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("produced");
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

    [ModularPipelines.Attributes.DependsOn<DuplicateArtifactProducerModule>]
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
    [ModularPipelines.Attributes.DependsOn<DeclaredProducerModule>]
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
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("consumer skipped"))
            .Build();

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

    [ModularPipelines.Attributes.DependsOn<SkippedArtifactValidationDependencyModule>]
    [ModularPipelines.Attributes.DependsOn<DeclaredProducerModule>]
    [ConsumesArtifact(typeof(DeclaredProducerModule), "missing-output")]
    private sealed class DependencySkippedInvalidArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dependency-skipped consumer must not execute");
    }

    [ConsumesArtifact(typeof(DeclaredProducerModule), "declared-output")]
    private sealed class UnorderedArtifactConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("consumed");
    }

    [ModularPipelines.Attributes.DependsOn<DeclaredProducerModule>(Optional = true)]
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
        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ProducedFile)!);
            await File.WriteAllTextAsync(ProducedFile, "local artifact content", cancellationToken);
            return "produced";
        }
    }

    [ModularPipelines.Attributes.DependsOn<LocalProducerModule>]
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

    [ProducesArtifact("multiple-output", MultipleProducedPattern)]
    private sealed class MultipleFileProducerModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var firstDirectory = Path.Combine(MultipleProducedDirectory, "first");
            var secondDirectory = Path.Combine(MultipleProducedDirectory, "second");
            Directory.CreateDirectory(firstDirectory);
            Directory.CreateDirectory(secondDirectory);
            await File.WriteAllTextAsync(
                Path.Combine(firstDirectory, "output.txt"),
                "first",
                cancellationToken);
            await File.WriteAllTextAsync(
                Path.Combine(secondDirectory, "output.txt"),
                "second",
                cancellationToken);
            return "produced";
        }
    }

    [ModularPipelines.Attributes.DependsOn<MultipleFileProducerModule>]
    [ConsumesArtifact(
        typeof(MultipleFileProducerModule),
        "multiple-output",
        RestorePath = MultipleRestoreDirectory)]
    private sealed class MultipleFileConsumerModule : Module<string>
    {
        public static string? ConsumedContent { get; set; }

        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var first = await File.ReadAllTextAsync(
                Path.Combine(MultipleRestoreDirectory, "first", "output.txt"),
                cancellationToken);
            var second = await File.ReadAllTextAsync(
                Path.Combine(MultipleRestoreDirectory, "second", "output.txt"),
                cancellationToken);
            return ConsumedContent = $"{first},{second}";
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

    [ModularPipelines.Attributes.DependsOn<MissingRuntimeProducerModule>]
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

    [ProducesArtifact("skipped-runtime", MissingRuntimeFile)]
    private sealed class SkippedArtifactProducerModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("producer skipped"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Skipped producer must not execute");
    }

    [ModularPipelines.Attributes.DependsOn<SkippedArtifactProducerModule>]
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

    [ModularPipelines.Attributes.DependsOn<MissingRuntimeProducerModule>]
    [ConsumesArtifact(typeof(MissingRuntimeProducerModule), "missing-runtime")]
    private sealed class ConfiguredSkippedArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("consumer skipped"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [ModularPipelines.Attributes.DependsOn<SkippedArtifactProducerModule>]
    [ConsumesArtifact(typeof(SkippedArtifactProducerModule), "skipped-runtime")]
    private sealed class ConfiguredSkippedHistoricalArtifactConsumerModule : Module<string>
    {
        public static bool Executed { get; set; }

        public static int SkipEvaluations;

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ =>
            {
                Interlocked.Increment(ref SkipEvaluations);
                return SkipDecision.Skip("consumer skipped");
            })
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.FromResult<string?>("consumed");
        }
    }

    [SkipIf<AlwaysSkipArtifactCondition>]
    [ModularPipelines.Attributes.DependsOn<SkippedArtifactProducerModule>]
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
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("blocker skipped"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Skipped blocker must not execute");
    }

    [ModularPipelines.Attributes.DependsOn<SkippedArtifactBlockerModule>(Optional = true)]
    [ProducesArtifact("dependency-ordered", MissingRuntimeFile)]
    private sealed class DependencyOrderedSkippedArtifactProducerModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("producer skipped"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Skipped producer must not execute");
    }

    [ModularPipelines.Attributes.DependsOn<SkippedArtifactBlockerModule>]
    [ModularPipelines.Attributes.DependsOn<DependencyOrderedSkippedArtifactProducerModule>]
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

    [ModularPipelines.Attributes.DependsOn<SkippedArtifactBlockerModule>]
    [ModularPipelines.Attributes.DependsOn<SkippedArtifactProducerModule>]
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

    [ModularPipelines.Attributes.DependsOn<SkippedArtifactBlockerModule>]
    private sealed class TransitiveSkippedArtifactIntermediateModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dependency-skipped intermediate must not execute");
    }

    [ModularPipelines.Attributes.DependsOn<TransitiveSkippedArtifactIntermediateModule>]
    [ModularPipelines.Attributes.DependsOn<DependencyOrderedSkippedArtifactProducerModule>]
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
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithIgnoreFailures()
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Producer failed");
    }

    [ModularPipelines.Attributes.DependsOn<IgnoredFailureArtifactProducerModule>]
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
                and not DependencyOrderedSkippedArtifactProducerModule)
            {
                return Task.FromResult<ModuleResult<T>?>(null);
            }

            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
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

    [ModularPipelines.Attributes.DependsOn<WorkingDirectoryProducerModule>]
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

    [Test]
    public async Task BuildAsyncRejectsUnknownProducedArtifactName()
    {
        using var builder = Pipeline.CreateBuilder();
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
        using var builder = Pipeline.CreateBuilder();
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
        using var builder = Pipeline.CreateBuilder();
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
        using var builder = Pipeline.CreateBuilder();
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
        using var builder = Pipeline.CreateBuilder();
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
        using var builder = Pipeline.CreateBuilder();
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
    public async Task BuildAsyncIgnoresInvalidContractOnAttributeSkippedConsumer()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<AttributeSkippedInvalidArtifactConsumerModule>();

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline).IsNotNull();
    }

    [Test]
    public async Task BuildAsyncIgnoresInvalidContractOnConfiguredSkippedConsumer()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<ConfiguredSkippedInvalidArtifactConsumerModule>();

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline).IsNotNull();
    }

    [Test]
    public async Task BuildAsyncIgnoresInvalidContractOnDependencySkippedConsumer()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DeclaredProducerModule>();
        builder.AddModule<SkippedArtifactValidationDependencyModule>();
        builder.AddModule<DependencySkippedInvalidArtifactConsumerModule>();

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline).IsNotNull();
    }

    [Test]
    public async Task ValidateAsyncAcceptsMatchingArtifactContract()
    {
        using var builder = Pipeline.CreateBuilder();
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

        try
        {
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<LocalProducerModule>();
            builder.AddModule<LocalConsumerModule>();

            await builder.ExecutePipelineAsync();

            await Assert.That(LocalConsumerModule.ConsumedContent).IsEqualTo("local artifact content");
        }
        finally
        {
            DeleteLocalArtifacts();
        }
    }

    [Test]
    public async Task StandaloneExecutionRestoresMultipleConsumedFiles()
    {
        DeleteLocalArtifacts();
        MultipleFileConsumerModule.ConsumedContent = null;

        try
        {
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<MultipleFileProducerModule>();
            builder.AddModule<MultipleFileConsumerModule>();

            await builder.ExecutePipelineAsync();

            await Assert.That(MultipleFileConsumerModule.ConsumedContent).IsEqualTo("first,second");
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
            using var builder = Pipeline.CreateBuilder();
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
        using var builder = Pipeline.CreateBuilder();
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
            using var builder = Pipeline.CreateBuilder();
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
    public async Task StandaloneExecutionFailsWhenConsumedArtifactWasNotUploaded()
    {
        DeleteLocalArtifacts();
        MissingRuntimeConsumerModule.Executed = false;

        try
        {
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<MissingRuntimeProducerModule>();
            builder.AddModule<MissingRuntimeConsumerModule>();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => builder.ExecutePipelineAsync());

            await Assert.That(exception!.ToString()).Contains("missing-runtime");
            await Assert.That(exception.ToString()).Contains("not found");
            await Assert.That(MissingRuntimeConsumerModule.Executed).IsFalse();
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
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactConsumerModule>();

            await builder.ExecutePipelineAsync();

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
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<SkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.Skipped);
                await Assert.That(consumerResult.ModuleStatus).IsEqualTo(Enums.Status.Skipped);
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
            using var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules = [nameof(SkippedArtifactConsumerModule)],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();

            await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
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
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<ConfiguredSkippedHistoricalArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<ConfiguredSkippedHistoricalArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
                await Assert.That(consumerResult.ModuleStatus).IsEqualTo(Enums.Status.Skipped);
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
            using var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules = [nameof(SkippedArtifactProducerModule)],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<ConfiguredSkippedHistoricalArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<ConfiguredSkippedHistoricalArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
                await Assert.That(consumerResult.ModuleStatus).IsEqualTo(Enums.Status.Skipped);
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
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactBlockerModule>();
            builder.AddModule<DependencyOrderedSkippedArtifactProducerModule>();
            builder.AddModule<DependencySkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<DependencyOrderedSkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<DependencySkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
                await Assert.That(consumerResult.ModuleStatus).IsEqualTo(Enums.Status.Skipped);
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
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactBlockerModule>();
            builder.AddModule<IndependentDependencySkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<IndependentDependencySkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
                await Assert.That(consumerResult.ModuleStatus).IsEqualTo(Enums.Status.Skipped);
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
    public async Task IgnoredArtifactProducerUsesHistoryWhenConsumerDependencyIgnored()
    {
        DeleteLocalArtifacts();
        DependencySkippedArtifactConsumerModule.Executed = false;

        try
        {
            using var builder = Pipeline.CreateBuilder();
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

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<DependencyOrderedSkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<DependencySkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
                await Assert.That(consumerResult.ModuleStatus).IsEqualTo(Enums.Status.Skipped);
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
            using var builder = Pipeline.CreateBuilder();
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

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<DependencyOrderedSkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<TransitiveDependencySkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
                await Assert.That(consumerResult.ModuleStatus).IsEqualTo(Enums.Status.Skipped);
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
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<AttributeSkippedHistoricalArtifactConsumerModule>();
            builder.AddResultsRepository<ArtifactHistoryRepository>();

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<AttributeSkippedHistoricalArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
                await Assert.That(consumerResult.ModuleStatus).IsEqualTo(Enums.Status.Skipped);
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
            using var builder = Pipeline.CreateBuilder();
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

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<SkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
                await Assert.That(consumerResult.ModuleStatus).IsEqualTo(Enums.Status.Skipped);
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
            using var builder = Pipeline.CreateBuilder();
            builder.ConfigurePipelineOptions(options => options with
            {
                SkippedModules = [nameof(SkippedArtifactConsumerModule)],
            });
            builder.AddModule<SkippedArtifactProducerModule>();
            builder.AddModule<SkippedArtifactConsumerModule>();
            builder.AddResultsRepository<ProducerAndConsumerArtifactHistoryRepository>();

            var summary = await builder.ExecutePipelineAsync();
            var producerResult = await summary.Modules
                .OfType<SkippedArtifactProducerModule>()
                .Single();
            var consumerResult = await summary.Modules
                .OfType<SkippedArtifactConsumerModule>()
                .Single();

            using (Assert.Multiple())
            {
                await Assert.That(producerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
                await Assert.That(consumerResult.ModuleStatus).IsEqualTo(Enums.Status.UsedHistory);
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
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<IgnoredFailureArtifactProducerModule>();
            builder.AddModule<IgnoredFailureArtifactConsumerModule>();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => builder.ExecutePipelineAsync());

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
            using var builder = Pipeline.CreateBuilder();
            builder.AddModule<MissingRuntimeProducerModule>();
            builder.AddModule<ConfiguredSkippedArtifactConsumerModule>();

            await builder.ExecutePipelineAsync();

            await Assert.That(ConfiguredSkippedArtifactConsumerModule.Executed).IsFalse();
        }
        finally
        {
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
            using var builder = Pipeline.CreateBuilder();
            builder.Services.Configure<ModuleCacheOptions>(options =>
                options.WorkingDirectory = workingDirectory.FullName);
            builder.AddModule<WorkingDirectoryProducerModule>();
            builder.AddModule<WorkingDirectoryConsumerModule>();

            await builder.ExecutePipelineAsync();

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
}
