using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "private-endpoint", "asg", "remove")]
public record AzNetworkPrivateEndpointAsgRemoveOptions(
[property: CommandSwitch("--asg-id")] string AsgId,
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}