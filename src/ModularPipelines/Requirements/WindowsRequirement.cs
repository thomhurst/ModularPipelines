using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Requirements;

[ExcludeFromCodeCoverage]
public class WindowsRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<bool> MustAsync(IPipelineContext context)
    {
        return Task.FromResult(context.Environment.OperatingSystem == OperatingSystemIdentifier.Windows);
    }
}