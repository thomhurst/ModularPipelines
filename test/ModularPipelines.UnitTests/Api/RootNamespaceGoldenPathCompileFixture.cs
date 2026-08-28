using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace RootNamespaceConsumer;

internal static class RootNamespaceGoldenPathCompileFixture
{
    public static async Task ConfigureAndRunAsync(PipelineBuilder builder)
    {
        builder.AddModule<GoldenPathModule>();
        builder.ConfigurePipelineOptions(options => options);
        await builder.RunAsync();
    }

    private sealed class GoldenPathModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }
}
