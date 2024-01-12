using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "url-maps", "validate")]
public record GcloudComputeUrlMapsValidateOptions : GcloudOptions
{
    [CommandSwitch("--load-balancing-scheme")]
    public string? LoadBalancingScheme { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}