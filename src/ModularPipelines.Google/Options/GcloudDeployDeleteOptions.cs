using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "delete")]
public record GcloudDeployDeleteOptions(
[property: CliOption("--file")] string File
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}