using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "create")]
public record GcloudVmwarePrivateCloudsCreateOptions(
[property: CliArgument] string PrivateCloud,
[property: CliArgument] string Location,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--management-range")] string ManagementRange,
[property: CliOption("--node-type-config")] string[] NodeTypeConfig,
[property: CliOption("--vmware-engine-network")] string VmwareEngineNetwork
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--preferred-zone")]
    public string? PreferredZone { get; set; }

    [CliOption("--secondary-zone")]
    public string? SecondaryZone { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}