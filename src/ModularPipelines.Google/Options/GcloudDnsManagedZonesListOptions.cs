using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "managed-zones", "list")]
public record GcloudDnsManagedZonesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}