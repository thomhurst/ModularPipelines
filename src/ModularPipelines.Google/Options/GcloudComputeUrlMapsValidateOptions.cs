using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "url-maps", "validate")]
public record GcloudComputeUrlMapsValidateOptions : GcloudOptions
{
    [CliOption("--load-balancing-scheme")]
    public string? LoadBalancingScheme { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}