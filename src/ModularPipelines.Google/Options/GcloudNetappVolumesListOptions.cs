using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "volumes", "list")]
public record GcloudNetappVolumesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}