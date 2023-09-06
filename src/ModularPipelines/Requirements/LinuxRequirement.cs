using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using ModularPipelines.Context;

namespace ModularPipelines.Requirements;

[ExcludeFromCodeCoverage]
public class LinuxRequirement : IPipelineRequirement
{
    public Task<bool> MustAsync(IModuleContext context)
    {
        return Task.FromResult(context.Environment.OperatingSystem == OSPlatform.OSX);
    }
}