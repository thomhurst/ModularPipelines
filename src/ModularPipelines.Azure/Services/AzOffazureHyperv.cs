using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("offure")]
public class AzOffazureHyperv
{
    public AzOffazureHyperv(
        AzOffazureHypervCluster cluster,
        AzOffazureHypervHost host,
        AzOffazureHypervMachine machine,
        AzOffazureHypervRunAsAccount runAsAccount,
        AzOffazureHypervSite site
    )
    {
        Cluster = cluster;
        Host = host;
        Machine = machine;
        RunAsAccount = runAsAccount;
        Site = site;
    }

    public AzOffazureHypervCluster Cluster { get; }

    public AzOffazureHypervHost Host { get; }

    public AzOffazureHypervMachine Machine { get; }

    public AzOffazureHypervRunAsAccount RunAsAccount { get; }

    public AzOffazureHypervSite Site { get; }
}