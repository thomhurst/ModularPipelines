using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "get-config")]
public record GcloudDeployGetConfigOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}