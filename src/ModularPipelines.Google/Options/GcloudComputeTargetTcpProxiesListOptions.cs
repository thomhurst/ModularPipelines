using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-tcp-proxies", "list")]
public record GcloudComputeTargetTcpProxiesListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--regions")]
    public string[]? Regions { get; set; }
}