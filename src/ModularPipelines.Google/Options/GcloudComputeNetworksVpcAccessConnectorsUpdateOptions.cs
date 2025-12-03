using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "vpc-access", "connectors", "update")]
public record GcloudComputeNetworksVpcAccessConnectorsUpdateOptions(
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
}