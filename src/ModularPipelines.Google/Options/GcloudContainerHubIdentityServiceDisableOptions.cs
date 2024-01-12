using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "identity-service", "disable")]
public record GcloudContainerHubIdentityServiceDisableOptions : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}