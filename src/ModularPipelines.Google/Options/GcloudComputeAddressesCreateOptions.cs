using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "addresses", "create")]
public record GcloudComputeAddressesCreateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--network-tier")]
    public string? NetworkTier { get; set; }

    [CliOption("--prefix-length")]
    public string? PrefixLength { get; set; }

    [CliOption("--purpose")]
    public string? Purpose { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--addresses")]
    public string[]? Addresses { get; set; }

    [CliOption("--ip-version")]
    public string? IpVersion { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}