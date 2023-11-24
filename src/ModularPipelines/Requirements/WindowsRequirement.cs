using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

[ExcludeFromCodeCoverage]
public class WindowsRequirement : IPipelineRequirement
{
    public Task<RequirementDecision> MustAsync(IPipelineHookContext context)
    {
        if (OperatingSystem.IsWindows())
        {
            return RequirementDecision.Passed.AsTask();
        }

        return RequirementDecision.Failed("Windows is required").AsTask();
    }
}