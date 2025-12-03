using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "export")]
public record GcloudComputeInstancesExportOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}