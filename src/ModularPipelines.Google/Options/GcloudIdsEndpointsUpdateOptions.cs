using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ids", "endpoints", "update")]
public record GcloudIdsEndpointsUpdateOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--max-wait")]
    public string? MaxWait { get; set; }

    [CommandSwitch("--threat-exceptions")]
    public string[]? ThreatExceptions { get; set; }
}