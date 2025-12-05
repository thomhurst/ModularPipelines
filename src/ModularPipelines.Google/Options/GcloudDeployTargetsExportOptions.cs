using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "targets", "export")]
public record GcloudDeployTargetsExportOptions(
[property: CliArgument] string Target,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}