using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "identity-service", "disable")]
public record GcloudContainerFleetIdentityServiceDisableOptions : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}