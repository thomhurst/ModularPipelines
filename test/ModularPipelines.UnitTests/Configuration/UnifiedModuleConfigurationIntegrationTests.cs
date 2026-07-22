using System.Collections.Concurrent;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Configuration;

[TUnit.Core.NotInParallel(nameof(UnifiedModuleConfigurationIntegrationTests))]
public class UnifiedModuleConfigurationIntegrationTests
{
    private static readonly ConcurrentQueue<string> ExecutionOrder = new();

    private sealed class DependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Enqueue("dependency");
            return Task.FromResult<string?>("dependency");
        }
    }

    private sealed class ConfiguredModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<DependencyModule>()
            .WithTags("configured")
            .WithCategory("configured-category")
            .WithNotInParallel("configured-lock")
            .WithPriority(ModulePriority.High)
            .WithExecutionHint(ExecutionType.IoIntensive)
            .Build();

        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Enqueue("configured");
            return Task.FromResult<string?>("configured");
        }
    }

    [DependsOnModulesWithTag("configured")]
    [DependsOnModulesInCategory("configured-category")]
    private sealed class MetadataConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Enqueue("consumer");
            return Task.FromResult<string?>("consumer");
        }
    }

    private sealed class FailingTaggedModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTags("failing")
            .Build();

        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => throw new InvalidOperationException("Expected test failure");
    }

    [DependsOnModulesWithTag("failing")]
    private sealed class FailingTagConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Enqueue("selector-consumer");
            return Task.FromResult<string?>("consumer");
        }
    }

    private sealed class SelfDependentModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<SelfDependentModule>()
            .Build();

        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("self-dependent");
    }

    private sealed class CircularDependencyModuleA : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<CircularDependencyModuleB>()
            .Build();

        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("a");
    }

    private sealed class CircularDependencyModuleB : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<CircularDependencyModuleA>()
            .Build();

        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("b");
    }

    [Before(Test)]
    public void ClearExecutionOrder()
    {
        while (ExecutionOrder.TryDequeue(out _))
        {
        }
    }

    [Test]
    public async Task Fluent_Configuration_Drives_Metadata_Scheduling_And_Dependencies()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<DependencyModule>()
            .AddModule<ConfiguredModule>()
            .AddModule<MetadataConsumerModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(ExecutionOrder).IsEquivalentTo(new[] { "dependency", "configured", "consumer" });
    }

    [Test]
    public async Task Selector_Dependency_Failure_Prevents_Consumer_Execution()
    {
        var pipeline = TestPipelineHostBuilder.Create()
            .AddModule<FailingTaggedModule>()
            .AddModule<FailingTagConsumerModule>();

        await Assert.ThrowsAsync<ModuleFailedException>(() => pipeline.ExecutePipelineAsync());
        await Assert.That(ExecutionOrder).DoesNotContain("selector-consumer");
    }

    [Test]
    public async Task Fluent_Self_Dependency_Is_Rejected_Before_Execution()
    {
        var pipeline = TestPipelineHostBuilder.Create()
            .AddModule<SelfDependentModule>();

        await Assert.ThrowsAsync<ModuleReferencingSelfException>(() => pipeline.ExecutePipelineAsync());
    }

    [Test]
    public async Task Fluent_Circular_Dependencies_Are_Rejected_Before_Execution()
    {
        var pipeline = TestPipelineHostBuilder.Create()
            .AddModule<CircularDependencyModuleA>()
            .AddModule<CircularDependencyModuleB>();

        await Assert.ThrowsAsync<DependencyCollisionException>(() => pipeline.ExecutePipelineAsync());
    }
}
