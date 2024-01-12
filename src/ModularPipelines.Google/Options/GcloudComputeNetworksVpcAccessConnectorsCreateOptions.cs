using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "vpc-access", "connectors", "create")]
public record GcloudComputeNetworksVpcAccessConnectorsCreateOptions(
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

    [CommandSwitch("--max-throughput")]
    public string? MaxThroughput { get; set; }

    [CommandSwitch("--min-throughput")]
    public string? MinThroughput { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--range")]
    public string? Range { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-project")]
    public string? SubnetProject { get; set; }
}