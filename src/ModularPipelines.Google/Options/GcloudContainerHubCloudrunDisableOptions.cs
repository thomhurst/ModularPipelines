using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "cloudrun", "disable")]
public record GcloudContainerHubCloudrunDisableOptions : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}