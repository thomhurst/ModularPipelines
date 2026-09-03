using Kevlar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.UnitTests;

public class WorkerModuleExecutorTests
{
    private abstract class RetryingModule : Module<int>
    {
        public int AttemptCount { get; private set; }

        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            if (++AttemptCount < 3)
            {
                throw new InvalidOperationException("Retry this attempt.");
            }

            return Task.FromResult(AttemptCount);
        }
    }

    private sealed class DeclarativeRetryModule : RetryingModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithRetry(2, TimeSpan.Zero);
    }

    private sealed class ShieldFactoryRetryModule : RetryingModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithShield(_ => Shield.Retry(2));
    }

    private sealed class DefaultRetryModule : RetryingModule;

    [Test]
    public Task Worker_Uses_Module_Retry_Configuration(CancellationToken cancellationToken) =>
        AssertWorkerRetriesAsync<DeclarativeRetryModule>(null, cancellationToken);

    [Test]
    public Task Worker_Uses_Module_Shield_Factory(CancellationToken cancellationToken) =>
        AssertWorkerRetriesAsync<ShieldFactoryRetryModule>(null, cancellationToken);

    [Test]
    public Task Worker_Uses_Default_Retry_Configuration(CancellationToken cancellationToken) =>
        AssertWorkerRetriesAsync<DefaultRetryModule>(
            builder => builder.ConfigureOptions(options => options with { DefaultRetryCount = 2 }),
            cancellationToken);

    private static async Task AssertWorkerRetriesAsync<TModule>(
        Action<PipelineBuilder>? configureBuilder,
        CancellationToken cancellationToken)
        where TModule : RetryingModule
    {
        var builder = TestPipelineBuilder.Create();
        configureBuilder?.Invoke(builder);
        builder.AddModule<TModule>();
        await using var pipeline = await builder.BuildAsync();
        var module = pipeline.Services.GetServices<IModule>()
            .OfType<TModule>()
            .Single();
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(TModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var assignment = new ModuleAssignment(
            typeof(TModule).FullName!,
            typeof(int).FullName!,
            [],
            DateTimeOffset.UtcNow,
            new ModuleAssignmentOptions(null, false));
        await coordinator.EnqueueModuleAsync(assignment, cancellationToken);
        var executor = new WorkerModuleExecutor(
            pipeline.Services.GetRequiredService<IHostApplicationLifetime>(),
            coordinator,
            [module],
            typeRegistry,
            serializer,
            pipeline.Services.GetRequiredService<IModuleRunner>(),
            pipeline.Services.GetRequiredService<IModuleResultRegistry>(),
            pipeline.Services.GetRequiredService<IModuleDependencyRegistry>(),
            pipeline.Services.GetRequiredService<IModuleMetadataRegistry>(),
            MsOptions.Create(new DistributedOptions
            {
                InstanceIndex = 1,
                AutoDetectOsCapability = false,
            }),
            pipeline.Services.GetRequiredService<IServiceScopeFactory>(),
            null,
            NullLogger<WorkerModuleExecutor>.Instance);

        var executionTask = executor.ExecuteAsync([module]);
        var serializedResult = await coordinator.WaitForResultAsync(
            typeof(TModule).FullName!,
            cancellationToken).WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
        await coordinator.SignalCompletionAsync(cancellationToken);
        await executionTask.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
        var result = serializer.Deserialize(serializedResult);

        using (Assert.Multiple())
        {
            await Assert.That(module.AttemptCount).IsEqualTo(3);
            await Assert.That(result?.ExceptionOrDefault).IsNull();
            await Assert.That(result?.ValueOrDefault).IsEqualTo(3);
        }
    }
}
