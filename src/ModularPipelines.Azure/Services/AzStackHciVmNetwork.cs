using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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