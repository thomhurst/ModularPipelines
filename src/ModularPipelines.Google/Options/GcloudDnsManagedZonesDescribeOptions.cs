using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "managed-zones", "describe")]
public record GcloudDnsManagedZonesDescribeOptions(
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}