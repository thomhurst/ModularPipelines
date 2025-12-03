using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "apply")]
public record GcloudDeployApplyOptions(
[property: CliOption("--file")] string File
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}