using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disk-types", "describe")]
public record GcloudComputeDiskTypesDescribeOptions(
[property: CliArgument] string DiskType
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}