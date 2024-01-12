using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ids", "endpoints", "delete")]
public record GcloudIdsEndpointsDeleteOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--max-wait")]
    public string? MaxWait { get; set; }
}