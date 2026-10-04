using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Events;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Reporting;
using ModularPipelines.Requirements;
using ModularPipelines.Validation;

namespace ModularPipelines.UnitTests.Registration;

/// <summary>
/// Single-instance registrations replace; multi-instance registrations are added once.
/// </summary>
public class RegistrationSemanticsTests
{
    private sealed class FirstBackend : IExecutionBackend
    {
        public bool OwnsEntirePlan => true;

        public Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
            ExecutionBackendRequest request,
            CancellationToken cancellationToken) => throw new NotSupportedException();
    }

    private sealed class SecondBackend : IExecutionBackend
    {
        public bool OwnsEntirePlan => true;

        public Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
            ExecutionBackendRequest request,
            CancellationToken cancellationToken) => throw new NotSupportedException();
    }

    private sealed class Repository : IModuleResultRepository
    {
        public bool IsEnabled => false;

        public Task SaveResultAsync<T>(Module<T> module, ModuleResult<T> moduleResult, IPipelineContext pipelineContext, CancellationToken cancellationToken) =>
            Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(Module<T> module, IPipelineContext pipelineContext, CancellationToken cancellationToken) =>
            Task.FromResult<ModuleResult<T>?>(null);
    }

    private sealed class HistoryStore : IRunHistoryStore
    {
        public IAsyncEnumerable<PipelineRunReport> GetRunsAsync(
            RunHistoryQuery query,
            CancellationToken cancellationToken = default) => AsyncEnumerable.Empty<PipelineRunReport>();

        public Task SaveAsync(PipelineRunReport report, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }

    private sealed class EstimatedTimeProvider : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType, CancellationToken cancellationToken = default) =>
            Task.FromResult(TimeSpan.Zero);

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }

    private sealed class Validator : IPipelineValidator
    {
        public int Order => 0;

        public Task<ValidationResult> ValidateAsync(IServiceProvider services, CancellationToken cancellationToken) => throw new NotSupportedException();
    }

    private sealed class Enricher : IRunReportEnricher
    {
        public ValueTask EnrichAsync(
            RunReportEnrichmentContext context,
            CancellationToken cancellationToken = default) => ValueTask.CompletedTask;
    }

    private sealed class Requirement : IPipelineRequirement
    {
        public Task<RequirementDecision> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            throw new NotSupportedException();
    }

    private sealed class PipelineHandler : IPipelineEventHandler;

    private sealed class ModuleHandler : IModuleEventHandler;

    private sealed class FirstStore : IModuleCacheStore
    {
        public Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken) =>
            Task.FromResult<Stream?>(null);

        public Task WriteAsync(string fingerprint, Stream content, CancellationToken cancellationToken) =>
            Task.CompletedTask;

        public Task DeleteAsync(string fingerprint, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }

    private sealed class SecondStore : IModuleCacheStore
    {
        public Task<Stream?> OpenReadAsync(string fingerprint, CancellationToken cancellationToken) =>
            Task.FromResult<Stream?>(null);

        public Task WriteAsync(string fingerprint, Stream content, CancellationToken cancellationToken) =>
            Task.CompletedTask;

        public Task DeleteAsync(string fingerprint, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }

    private sealed class AnyModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }

    private static int Count<TService>(PipelineBuilder builder) =>
        builder.Services.Count(descriptor => descriptor.ServiceType == typeof(TService));

    private static Type? LastImplementation<TService>(PipelineBuilder builder) =>
        builder.Services.Last(descriptor => descriptor.ServiceType == typeof(TService)).ImplementationType;

    [Test]
    public async Task Single_Instance_Registrations_Replace_Earlier_Ones()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddExecutionBackend<FirstBackend>().AddExecutionBackend<SecondBackend>();
        builder.AddResultsRepository<Repository>().AddResultsRepository<Repository>();
        builder.AddRunHistoryStore<HistoryStore>().AddRunHistoryStore<HistoryStore>();
        builder.AddModuleEstimatedTimeProvider<EstimatedTimeProvider>()
            .AddModuleEstimatedTimeProvider<EstimatedTimeProvider>();
        builder.AddModuleCache<FirstStore>().AddModuleCache<SecondStore>();

        using (Assert.Multiple())
        {
            await Assert.That(Count<IExecutionBackend>(builder)).IsEqualTo(1);
            await Assert.That(LastImplementation<IExecutionBackend>(builder)).IsEqualTo(typeof(SecondBackend));
            await Assert.That(Count<IModuleResultRepository>(builder)).IsEqualTo(1);
            await Assert.That(Count<IRunHistoryStore>(builder)).IsEqualTo(1);
            await Assert.That(Count<IModuleEstimatedTimeProvider>(builder)).IsEqualTo(1);
            await Assert.That(Count<IModuleCacheStore>(builder)).IsEqualTo(1);
        }
    }

    [Test]
    public async Task Single_Instance_Registration_Wins_At_Resolution()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModuleCache<FirstStore>().AddModuleCache<SecondStore>().AddModule<AnyModule>();
        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline.Services.GetRequiredService<IModuleCacheStore>()).IsTypeOf<SecondStore>();
    }

    [Test]
    public async Task Multi_Instance_Type_Registrations_Are_Added_Once()
    {
        var builder = Pipeline.CreateBuilder();
        var validatorsBefore = Count<IPipelineValidator>(builder);
        var enrichersBefore = Count<IRunReportEnricher>(builder);
        var requirementsBefore = Count<IPipelineRequirement>(builder);
        var pipelineHandlersBefore = Count<IPipelineEventHandler>(builder);
        var moduleHandlersBefore = Count<IModuleEventHandler>(builder);

        builder.AddValidator<Validator>().AddValidator<Validator>();
        builder.AddRunReportEnricher<Enricher>().AddRunReportEnricher<Enricher>();
        builder.AddRequirement<Requirement>().AddRequirement<Requirement>();
        builder.AddPipelineEventHandler<PipelineHandler>().AddPipelineEventHandler<PipelineHandler>();
        builder.AddModuleEventHandler<ModuleHandler>().AddModuleEventHandler<ModuleHandler>();

        using (Assert.Multiple())
        {
            await Assert.That(Count<IPipelineValidator>(builder)).IsEqualTo(validatorsBefore + 1);
            await Assert.That(Count<IRunReportEnricher>(builder)).IsEqualTo(enrichersBefore + 1);
            await Assert.That(Count<IPipelineRequirement>(builder)).IsEqualTo(requirementsBefore + 1);
            await Assert.That(Count<IPipelineEventHandler>(builder)).IsEqualTo(pipelineHandlersBefore + 1);
            await Assert.That(Count<IModuleEventHandler>(builder)).IsEqualTo(moduleHandlersBefore + 1);
        }
    }

    [Test]
    public async Task Requirement_Instances_Are_Added_Once_Per_Instance()
    {
        var builder = Pipeline.CreateBuilder();
        var before = Count<IPipelineRequirement>(builder);
        var first = new Requirement();
        var second = new Requirement();

        builder.AddRequirement(first).AddRequirement(first).AddRequirement(second);

        await Assert.That(Count<IPipelineRequirement>(builder)).IsEqualTo(before + 2);
    }

    [Test]
    public async Task Registration_Helpers_Reject_Null_Arguments()
    {
        using (Assert.Multiple())
        {
            await Assert.That(() => PipelineBuilderExtensions.AddResultsRepository<Repository>(null!))
                .Throws<ArgumentNullException>();
            await Assert.That(() => Pipeline.CreateBuilder().AddRequirement((IPipelineRequirement) null!))
                .Throws<ArgumentNullException>();
        }
    }

    [Test]
    public async Task Module_Cache_Configurations_Apply_In_Order_After_The_Working_Directory_Seed()
    {
        var workingDirectory = Path.GetTempPath();
        var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings { WorkingDirectory = workingDirectory });
        builder.AddModuleCache<FirstStore>(options => options with { MaxInputFiles = 10, MaxHashConcurrency = 3 });
        builder.AddModuleCache<FirstStore>(options => options with { MaxInputFiles = options.MaxInputFiles * 2 });
        builder.AddModule<AnyModule>();
        await using var pipeline = await builder.BuildAsync();

        var options = pipeline.Services.GetRequiredService<IOptions<ModuleCacheOptions>>().Value;

        using (Assert.Multiple())
        {
            await Assert.That(options.MaxInputFiles).IsEqualTo(20);
            await Assert.That(options.MaxHashConcurrency).IsEqualTo(3);
            await Assert.That(Path.GetFullPath(options.WorkingDirectory).TrimEnd(Path.DirectorySeparatorChar))
                .IsEqualTo(Path.GetFullPath(workingDirectory).TrimEnd(Path.DirectorySeparatorChar));
        }
    }

    [Test]
    public async Task Module_Cache_Options_Are_Immutable()
    {
        await Assert.That(typeof(ModuleCacheOptions).GetProperties()
                .Where(property => property.SetMethod is not null)
                .All(property => property.SetMethod!.ReturnParameter.GetRequiredCustomModifiers()
                    .Contains(typeof(System.Runtime.CompilerServices.IsExternalInit))))
            .IsTrue();
    }
}
