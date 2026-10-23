using ModularPipelines.Build.Modules;
using ModularPipelines.Extensions;

namespace ModularPipelines.Build.Helpers;

internal sealed class PackageVersionOutput(string[] args)
{
    private readonly string? _outputPath = args is ["--write-package-version", var outputPath] ? outputPath : null;

    public string[] PipelineArguments => _outputPath is null ? args : [];

    public async Task<bool> TryWriteAsync(PipelineBuilder builder)
    {
        if (_outputPath is null)
        {
            return false;
        }

        // Resolve the version without registering or running release modules.
        builder.AddCapabilities(new Capability("ci-master"));
        builder.AddModule<NugetVersionGeneratorModule>();
        await using var pipeline = await builder.BuildAsync().ConfigureAwait(false);
        var summary = await pipeline.RunAsync().ConfigureAwait(false);
        var version = summary.GetResult<NugetVersionGeneratorModule, string>().Value;
        await File.AppendAllTextAsync(_outputPath, $"Pipeline__PackageVersion={version}{Environment.NewLine}").ConfigureAwait(false);
        return true;
    }
}
