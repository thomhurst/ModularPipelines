using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("offure")]
public class AzOffazureVmware
{
    public AzOffazureVmware(
        AzOffazureVmwareMachine machine,
        AzOffazureVmwareRunAsAccount runAsAccount,
        AzOffazureVmwareSite site,
        AzOffazureVmwareVcenter vcenter
    )
    {
        Machine = machine;
        RunAsAccount = runAsAccount;
        Site = site;
        Vcenter = vcenter;
    }

    public AzOffazureVmwareMachine Machine { get; }

    public AzOffazureVmwareRunAsAccount RunAsAccount { get; }

    public AzOffazureVmwareSite Site { get; }

    public AzOffazureVmwareVcenter Vcenter { get; }
}