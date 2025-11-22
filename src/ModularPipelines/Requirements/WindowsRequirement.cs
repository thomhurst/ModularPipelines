using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

/// <summary>
/// A pipeline requirement that ensures the current operating system is Windows.
/// </summary>
[ExcludeFromCodeCoverage]
public class WindowsRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<RequirementDecision> MustAsync(IPipelineHookContext context)
    {
        if (OperatingSystem.IsWindows())
        {
            return RequirementDecision.Passed.AsTask();
        }

        return RequirementDecision.Failed("Windows is required").AsTask();
    }
}
