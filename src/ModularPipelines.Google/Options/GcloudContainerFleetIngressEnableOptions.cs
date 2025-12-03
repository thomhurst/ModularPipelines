using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "ingress", "enable")]
public record GcloudContainerFleetIngressEnableOptions : GcloudOptions
{
    [CliOption("--config-membership")]
    public string? ConfigMembership { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}