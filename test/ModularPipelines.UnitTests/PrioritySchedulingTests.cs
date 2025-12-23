using System.Collections.Concurrent;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

[TUnit.Core.NotInParallel(nameof(PrioritySchedulingTests))]
public class PrioritySchedulingTests : TestBase
{
    private static readonly ConcurrentQueue<string> ExecutionOrder = new();

    [Priority(ModulePriority.Low)]
    public class LowPriorityModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Enqueue("Low");
            await Task.Delay(10, cancellationToken);
            return "Low";
        }
    }

    [Priority(ModulePriority.Normal)]
    public class NormalPriorityModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Enqueue("Normal");
            await Task.Delay(10, cancellationToken);
            return "Normal";
        }
    }

    [Priority(ModulePriority.High)]
    public class HighPriorityModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Enqueue("High");
            await Task.Delay(10, cancellationToken);
            return "High";
        }
    }

    [Priority(ModulePriority.Critical)]
    public class CriticalPriorityModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Enqueue("Critical");
            await Task.Delay(10, cancellationToken);
            return "Critical";
        }
    }

    public class DefaultPriorityModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Enqueue("Default");
            await Task.Delay(10, cancellationToken);
            return "Default";
        }
    }

    [Before(Test)]
    public void ClearExecutionOrder()
    {
        while (ExecutionOrder.TryDequeue(out _)) { }
    }

    [Test]
    public async Task PriorityAttribute_CanBeAppliedToModule()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<CriticalPriorityModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task ModulesWithoutPriorityAttribute_UseNormalPriority()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<DefaultPriorityModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task AllPriorityLevels_ExecuteSuccessfully()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<LowPriorityModule>()
            .AddModule<NormalPriorityModule>()
            .AddModule<HighPriorityModule>()
            .AddModule<CriticalPriorityModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(ExecutionOrder.Count).IsEqualTo(4);
    }
}
