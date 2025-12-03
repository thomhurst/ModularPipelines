using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "ingress", "enable")]
public record GcloudContainerHubIngressEnableOptions : GcloudOptions
{
    [CliOption("--config-membership")]
    public string? ConfigMembership { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}