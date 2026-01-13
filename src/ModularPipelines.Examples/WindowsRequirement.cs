using System.Runtime.InteropServices;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Requirements;

namespace ModularPipelines.Examples;

public class WindowsRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<RequirementDecision> MustAsync(IPipelineHookContext context)
    {
        return Task.FromResult<RequirementDecision>(context.Environment.OperatingSystem == OSPlatform.Windows);
    }
}