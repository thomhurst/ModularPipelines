using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Requirements;

[ExcludeFromCodeCoverage]
public class LinuxRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<bool> MustAsync(IPipelineContext context)
    {
        return Task.FromResult(context.Environment.OperatingSystem == OperatingSystemIdentifier.Linux);
    }
}