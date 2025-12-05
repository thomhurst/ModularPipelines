using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "ingress", "disable")]
public record GcloudContainerFleetIngressDisableOptions : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}