using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.SmokeTests;

[RequiresCapability(Capability.Names.Windows)]
public sealed class WindowsProbeModule : Module<string>
{
    public static bool GeneratingWorkflow { get; set; }

    protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        if (!GeneratingWorkflow && !OperatingSystem.IsWindows())
        {
            throw new InvalidOperationException("The Windows module ran on another operating system.");
        }

        return Task.FromResult("windows-ok");
    }
}
