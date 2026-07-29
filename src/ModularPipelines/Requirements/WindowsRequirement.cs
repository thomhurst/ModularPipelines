using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

/// <summary>
/// A pipeline requirement that ensures the current operating system is Windows.
/// </summary>
[ExcludeFromCodeCoverage]
public class WindowsRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<RequirementDecision> MustAsync(IPipelineContext context)
    {
        if (OperatingSystem.IsWindows())
        {
            return Task.FromResult(RequirementDecision.Passed);
        }

        return Task.FromResult(RequirementDecision.Failed("Windows is required"));
    }
}
