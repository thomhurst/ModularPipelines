using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "subnets", "list")]
public record GcloudComputeNetworksSubnetsListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliOption("--regions")]
    public string[]? Regions { get; set; }
}