using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "subnets", "expand-ip-range")]
public record GcloudComputeNetworksSubnetsExpandIpRangeOptions(
[property: CliArgument] string Name,
[property: CliOption("--prefix-length")] string PrefixLength
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}