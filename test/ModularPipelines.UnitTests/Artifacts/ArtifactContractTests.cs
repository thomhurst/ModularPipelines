using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Validation;

namespace ModularPipelines.UnitTests.Artifacts;

[TUnit.Core.NotInParallel(nameof(ArtifactContractTests))]
public class ArtifactContractTests
{
    private const string LocalArtifactRoot = ".modular-pipelines-local-artifact-tests";
    private const string ProducedFile = LocalArtifactRoot + "/produced/output.txt";
    private const string RestoreDirectory = LocalArtifactRoot + "/restored";

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

    private static void DeleteLocalArtifacts()
    {
        if (Directory.Exists(LocalArtifactRoot))
        {
            Directory.Delete(LocalArtifactRoot, recursive: true);
        }
    }
}
