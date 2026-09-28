using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Engine;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class CapabilityRouteSkipPipelineTests
{
    [Test]
    [Timeout(30_000)]
    public async Task Master_Skips_Module_Whose_Conditions_No_Worker_Can_Satisfy(CancellationToken cancellationToken)
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var runId = Guid.NewGuid().ToString("N");
        PipelineBuilder CreateBuilder(int index)
        {
            var builder = TestPipelineBuilder.Create();
            builder.AddDistributedMode(options =>
            {
                options.InstanceIndex = index;
                options.RunId = runId;
                options.TotalInstances = 2;
                options.MinimumWorkerCount = 1;
                options.WorkerRegistrationTimeout = TimeSpan.FromSeconds(10);
                options.ModuleResultTimeout = TimeSpan.FromSeconds(10);
            });
            builder.Services.AddSingleton(coordinator);
            builder.Services.AddSingleton<IDistributedMasterCoordinator>(coordinator);
            builder.Services.AddSingleton<IDistributedWorkerCoordinator>(coordinator);
            builder.AddModule<LinuxAndWindowsUnlessFalseModule>();
            return builder;
        }

        await using var master = await CreateBuilder(0).BuildAsync();
        await using var worker = await CreateBuilder(1).BuildAsync();
        await Task.WhenAll(master.RunAsync(cancellationToken), worker.RunAsync(cancellationToken));

        var results = master.Services.GetRequiredService<IModuleResultRegistry>();
        await Assert.That(results.GetResult(typeof(LinuxAndWindowsUnlessFalseModule))!.Status)
            .IsEqualTo(ModuleStatus.Skipped);
    }

    // With both planning conditions false on the master, the module needs Linux and Windows at once.
    [RunIfAny<OnLinux, PlanningFalseCondition>]
    [RunIfAny<OnWindows, PlanningFalseCondition>]
    private sealed class LinuxAndWindowsUnlessFalseModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("No worker can satisfy this module's conditions.");
    }

    private sealed class PlanningFalseCondition : IRunCondition, IPlanningSafe
    {
        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) => Task.FromResult(false);
    }
}
