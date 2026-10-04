using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Engine;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class CapabilityConditionDefaultPipelineTests
{
    [Test]
    [Timeout(30_000)]
    public async Task Worker_Default_Condition_Uses_Provider_Capability_Advertised_For_Routing(CancellationToken cancellationToken)
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var runId = Guid.NewGuid().ToString("N");
        var provider = new GpuProvider();

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
            if (index == 1)
            {
                builder.Services.AddSingleton<ICapabilityProvider>(provider);
            }

            builder.AddModule<GpuModule>();
            return builder;
        }

        await using var master = await CreateBuilder(0).BuildAsync();
        await using var worker = await CreateBuilder(1).BuildAsync();
        await Task.WhenAll(master.RunAsync(cancellationToken), worker.RunAsync(cancellationToken));

        var results = master.Services.GetRequiredService<IModuleResultRegistry>();
        var result = (ModuleResult<int>) results.GetResult(typeof(GpuModule))!;
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(result.Value).IsEqualTo(1);
        await Assert.That(provider.CallCount).IsEqualTo(1);
    }

    [RunIf<OnGpu>]
    private sealed class GpuModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(context.Services.GetRequiredService<IOptions<DistributedOptions>>().Value.InstanceIndex);
    }

    private sealed class OnGpu : ICapabilityCondition
    {
        public Capability Capability => Capability.Gpu;
    }

    private sealed class GpuProvider : ICapabilityProvider
    {
        private int _callCount;

        public int CallCount => Volatile.Read(ref _callCount);

        public Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _callCount);
            return Task.FromResult<IEnumerable<Capability>>([Capability.Gpu]);
        }
    }
}
