using ModularPipelines;

namespace RootNamespaceConsumer;

internal static class RootNamespaceGoldenPathCompileFixture
{
    public static (CommandLine Command, KeyValue Pair, CliOptionValue? Optional, CliValuePair Operands,
        CliValueGroup Group, CliOptionValueRangeAttribute Range, CliOptionValueRegularExpressionAttribute Pattern)
        CreateCliContracts()
        => (new("tool", ["--flag"]), new("key", "value"), CliOptionValue.Bare,
            new("first", "second"), new(["first", "second"]), new(1, 3), new(".*"));

    public static async Task ConfigureAndRunAsync(PipelineBuilder builder)
    {
        builder.AddCapabilities(Capability.Docker);
        builder.AddCapabilityProvider<GoldenPathCapabilityProvider>();
        _ = CapabilityRequirement.AllOf(Capability.Docker);
        builder.AddPlugin<GoldenPathPlugin>();
        builder.ConfigureOptions(options => options);
        await builder.RunAsync();
    }

    public static async Task<(PipelinePlan Plan, PipelinePlanWave Wave, PipelinePlanModule PlannedModule, IModule Module, ModuleConfiguration Configuration)> InspectPlanAsync(IPipeline pipeline)
    {
        var plan = await pipeline.PlanAsync();
        var wave = plan.Waves[0];
        var plannedModule = wave.Modules[0];
        var module = plannedModule.Module;
        return (plan, wave, plannedModule, module, module.Configuration);
    }

    private sealed class GoldenPathPlugin : IModularPipelinesPlugin
    {
        public string Name => "Golden path";

        public void Configure(PipelineBuilder builder) => builder.AddModule<GoldenPathModule>();
    }

    private sealed class GoldenPathInitializer : ICancellableInitializer
    {
        public Task InitializeAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class GoldenPathCapabilityProvider : ICapabilityProvider
    {
        public Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken)
            => Task.FromResult<IEnumerable<Capability>>([Capability.Gpu]);
    }

    private sealed class GoldenPathModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }
}
