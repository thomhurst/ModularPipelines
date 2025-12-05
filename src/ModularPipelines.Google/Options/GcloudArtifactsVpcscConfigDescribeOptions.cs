using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "vpcsc-config", "describe")]
public record GcloudArtifactsVpcscConfigDescribeOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}