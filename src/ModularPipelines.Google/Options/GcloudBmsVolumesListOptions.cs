using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "volumes", "list")]
public record GcloudBmsVolumesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}