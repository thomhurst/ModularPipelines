using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "managed-zones", "describe")]
public record GcloudDnsManagedZonesDescribeOptions(
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}