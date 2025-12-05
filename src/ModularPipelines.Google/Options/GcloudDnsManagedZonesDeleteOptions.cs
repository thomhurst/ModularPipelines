using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "managed-zones", "delete")]
public record GcloudDnsManagedZonesDeleteOptions(
[property: CliArgument] string ZoneName
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}