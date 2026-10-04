using ModularPipelines;

namespace RootNamespaceConsumer;

internal static class RootNamespaceGoldenPathCompileFixture
{
    public static async Task ConfigureAndRunAsync(PipelineBuilder builder)
    {
        builder.AddCapabilities(Capability.Docker);
        builder.AddCapabilityProvider<GoldenPathCapabilityProvider>();
        _ = CapabilityRequirement.AllOf(Capability.Docker);
        builder.AddModule<GoldenPathModule>();
        builder.ConfigureOptions(options => options);
        await builder.RunAsync();
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
