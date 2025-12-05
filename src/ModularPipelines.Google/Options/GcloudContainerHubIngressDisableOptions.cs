using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "ingress", "disable")]
public record GcloudContainerHubIngressDisableOptions : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}