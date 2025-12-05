using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-connections", "create")]
public record GcloudVmwarePrivateConnectionsCreateOptions(
[property: CliArgument] string PrivateConnection,
[property: CliArgument] string Location,
[property: CliOption("--service-project")] string ServiceProject,
[property: CliOption("--type")] string Type,
[property: CliOption("--vmware-engine-network")] string VmwareEngineNetwork
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--routing-mode")]
    public string? RoutingMode { get; set; }

    [CliOption("--service-network")]
    public string? ServiceNetwork { get; set; }
}