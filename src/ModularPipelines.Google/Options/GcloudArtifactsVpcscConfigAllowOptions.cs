using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "vpcsc-config", "allow")]
public record GcloudArtifactsVpcscConfigAllowOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}