using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Requirements;

namespace ModularPipelines.Examples;

public class WindowsRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public async Task<RequirementDecision> MustAsync(IPipelineHookContext context)
    {
        await Task.Yield();
        return context.Environment.OperatingSystem == OperatingSystemIdentifier.Windows;
    }
}