using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

/// <summary>
/// A pipeline requirement that ensures the current operating system is Linux.
/// </summary>
[ExcludeFromCodeCoverage]
public class LinuxRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<RequirementDecision> MustAsync(IPipelineContext context)
    {
        return Task.FromResult(RequirementDecision.Of(
            passed: context.Environment.OperatingSystem == System.Runtime.InteropServices.OSPlatform.Linux,
            reason: "Linux is required"
        ));
    }
}
