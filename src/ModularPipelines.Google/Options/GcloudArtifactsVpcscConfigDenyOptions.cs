using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "vpcsc-config", "deny")]
public record GcloudArtifactsVpcscConfigDenyOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}