using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "ingress", "enable")]
public record GcloudContainerFleetIngressEnableOptions : GcloudOptions
{
    [CommandSwitch("--config-membership")]
    public string? ConfigMembership { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}