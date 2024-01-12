using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "vpcsc-config", "describe")]
public record GcloudArtifactsVpcscConfigDescribeOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}