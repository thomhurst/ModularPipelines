using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "targets", "list")]
public record GcloudDeployTargetsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}