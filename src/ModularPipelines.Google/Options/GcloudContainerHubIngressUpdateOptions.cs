using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "ingress", "update")]
public record GcloudContainerHubIngressUpdateOptions : GcloudOptions
{
    [CommandSwitch("--config-membership")]
    public string? ConfigMembership { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}