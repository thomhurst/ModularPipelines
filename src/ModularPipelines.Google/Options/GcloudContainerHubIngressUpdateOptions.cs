using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "ingress", "update")]
public record GcloudContainerHubIngressUpdateOptions : GcloudOptions
{
    [CliOption("--config-membership")]
    public string? ConfigMembership { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}