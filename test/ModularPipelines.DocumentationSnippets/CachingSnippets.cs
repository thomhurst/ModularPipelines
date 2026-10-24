using ModularPipelines;

namespace ModularPipelines.DocumentationSnippets.Caching;

// docs/docs/how-to/module-caching.md: Enable a local cache
[CacheInputs("src/**/*.cs", "*.csproj")]
[ProducesArtifact("application", "artifacts/publish/**/*")]
public sealed class BuildModule : Module<BuildOutput>
{
    protected override Task<BuildOutput> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        // Build the application here, then return its output location.
        return Task.FromResult(new BuildOutput("artifacts/publish"));
    }
}

public sealed record BuildOutput(string OutputPath);
