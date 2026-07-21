using System.Collections.Concurrent;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Enums;
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
}
