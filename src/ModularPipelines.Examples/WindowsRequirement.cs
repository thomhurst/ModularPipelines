using System.Runtime.InteropServices;
using ModularPipelines.Context;
using ModularPipelines.Requirements;

namespace ModularPipelines.Examples;

public class WindowsRequirement : IPipelineRequirement
{
    public async Task<bool> MustAsync(IPipelineContext context)
    {
        await Task.Yield();
        return context.Environment.OperatingSystem == OSPlatform.Windows;
    }
}