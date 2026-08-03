using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts;
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
    private const string CacheOnlyFile = LocalArtifactRoot + "/cache-only.bin";
    private const string MissingRuntimeFile = LocalArtifactRoot + "/missing/output.txt";

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
