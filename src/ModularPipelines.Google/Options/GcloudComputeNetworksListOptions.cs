using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "list")]
public record GcloudComputeNetworksListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }
}