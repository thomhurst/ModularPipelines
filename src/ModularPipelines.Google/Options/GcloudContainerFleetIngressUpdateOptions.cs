using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "ingress", "update")]
public record GcloudContainerFleetIngressUpdateOptions : GcloudOptions
{
    [CommandSwitch("--config-membership")]
    public string? ConfigMembership { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}