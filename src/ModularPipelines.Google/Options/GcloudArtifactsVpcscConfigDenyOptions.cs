using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "vpcsc-config", "deny")]
public record GcloudArtifactsVpcscConfigDenyOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}