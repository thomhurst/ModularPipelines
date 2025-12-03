using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "vpcsc-config", "allow")]
public record GcloudArtifactsVpcscConfigAllowOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}