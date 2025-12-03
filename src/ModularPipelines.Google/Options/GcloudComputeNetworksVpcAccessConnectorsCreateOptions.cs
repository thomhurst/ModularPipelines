using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "vpc-access", "connectors", "create")]
public record GcloudComputeNetworksVpcAccessConnectorsCreateOptions(
[property: CliArgument] string Connector,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--max-instances")]
    public string? MaxInstances { get; set; }

    [CliOption("--min-instances")]
    public string? MinInstances { get; set; }

    [CliOption("--max-throughput")]
    public string? MaxThroughput { get; set; }

    [CliOption("--min-throughput")]
    public string? MinThroughput { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--range")]
    public string? Range { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-project")]
    public string? SubnetProject { get; set; }
}