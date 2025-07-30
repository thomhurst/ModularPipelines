using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

/// <summary>
/// A pipeline requirement that ensures the current operating system is Linux.
/// </summary>
[ExcludeFromCodeCoverage]
public class LinuxRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<RequirementDecision> MustAsync(IPipelineHookContext context)
    {
        return RequirementDecision.Of(
            passed: context.Environment.OperatingSystem == OperatingSystemIdentifier.Linux,
            reason: "Linux is required"
        ).AsTask();
    }
}