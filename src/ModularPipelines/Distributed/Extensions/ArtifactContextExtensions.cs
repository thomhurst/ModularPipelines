using ModularPipelines.Context;

namespace ModularPipelines.Distributed.Extensions;

/// <summary>
/// Extension methods for accessing artifact operations from module context.
/// </summary>
public static class ArtifactContextExtensions
{
    /// <summary>
    /// Gets the artifact context for publishing and downloading artifacts in distributed mode.
    /// </summary>
    public static IArtifactContext Artifacts(this IPipelineContext context)
    {
        return context.GetService<IArtifactContext>();
    }
}
