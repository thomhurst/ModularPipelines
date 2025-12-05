using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "get-nat-ip-info")]
public record GcloudComputeRoutersGetNatIpInfoOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--nat-name")]
    public string? NatName { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}