using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using ModularPipelines.Context;

namespace ModularPipelines.Requirements;

[ExcludeFromCodeCoverage]
public class WindowsRequirement : IPipelineRequirement
{
    public Task<bool> MustAsync(IPipelineContext context)
    {
        return Task.FromResult(context.Environment.OperatingSystem == OSPlatform.Windows);
    }
}