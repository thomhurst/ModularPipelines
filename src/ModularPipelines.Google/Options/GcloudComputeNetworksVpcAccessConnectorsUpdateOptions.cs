using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "vpc-access", "connectors", "update")]
public record GcloudComputeNetworksVpcAccessConnectorsUpdateOptions(
[property: PositionalArgument] string Connector,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--max-instances")]
    public string? MaxInstances { get; set; }

    [CommandSwitch("--min-instances")]
    public string? MinInstances { get; set; }
}