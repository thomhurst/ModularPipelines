using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "ingress", "update")]
public record GcloudContainerFleetIngressUpdateOptions : GcloudOptions
{
    [CliOption("--config-membership")]
    public string? ConfigMembership { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}