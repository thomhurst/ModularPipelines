using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzOffazure
{
    public AzOffazure(
        AzOffazureHyperv hyperv,
        AzOffazureVmware vmware
    )
    {
        Hyperv = hyperv;
        Vmware = vmware;
    }

    public AzOffazureHyperv Hyperv { get; }

    public AzOffazureVmware Vmware { get; }
}