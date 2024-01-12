using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "managed-zones", "delete")]
public record GcloudDnsManagedZonesDeleteOptions(
[property: PositionalArgument] string ZoneName
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}