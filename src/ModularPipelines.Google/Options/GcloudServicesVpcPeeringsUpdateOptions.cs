using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "vpc-peerings", "update")]
public record GcloudServicesVpcPeeringsUpdateOptions(
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--ranges")]
    public string? Ranges { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }
}