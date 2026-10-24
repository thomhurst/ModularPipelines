using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

// Only the Redis fallback declares an artifact for the framework to upload.
// External transports publish during BuildSolutionsModule and delete their archive.
[RequiresCapability("ci-master")]
[DependsOn<BuildSolutionsModule>]
[RunIf<ModularPipelines.OnLinux>]
[RunIf<UsesRedisBuildOutput>]
[ProducesArtifact("build-output", "../../" + BuildOutputArchive.FileName)]
public class PublishRedisBuildOutputModule : Module<None>
{
    protected override Task<None> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
        Task.FromResult(None.Value);
}

public sealed class UsesRedisBuildOutput : IRunCondition, IPlanningSafe
{
    public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        Task.FromResult(context.Services.GetRequiredService<BuildOutputSharing>().UsesRedisFallback);
}
