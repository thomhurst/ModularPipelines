using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci-vm")]
public class AzStackHciVmNetwork
{
    public AzStackHciVmNetwork(
        AzStackHciVmNetworkLnet lnet,
        AzStackHciVmNetworkNic nic
    )
    {
        Lnet = lnet;
        Nic = nic;
    }

    public AzStackHciVmNetworkLnet Lnet { get; }

    public AzStackHciVmNetworkNic Nic { get; }
}